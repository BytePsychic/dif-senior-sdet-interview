using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIF.Api.Models.Domain;
using DIF.Api.Services.Interfaces;

namespace DIF.Api.Services.Implementations;

/// <summary>
/// Mock implementation of IAlertService for testing.
/// Logs alerts and errors in memory without actual Slack/Email integration.
/// </summary>
public class MockAlertService : IAlertService
{
    private readonly ConcurrentBag<ApiError> _errors;
    private readonly ConcurrentBag<ApiRequestLog> _requestLogs;
    private readonly ConcurrentBag<string> _slackMessages;
    private readonly ConcurrentBag<(List<string> Recipients, string Subject, string Body)> _emails;

    public MockAlertService()
    {
        _errors = new ConcurrentBag<ApiError>();
        _requestLogs = new ConcurrentBag<ApiRequestLog>();
        _slackMessages = new ConcurrentBag<string>();
        _emails = new ConcurrentBag<(List<string>, string, string)>();
    }

    /// <inheritdoc />
    public async Task SendCriticalAlertAsync(string message, ApiError error)
    {
        error.Severity = ErrorSeverity.Critical;
        await LogApiErrorAsync(error);

        // Send immediate Slack notification
        var slackMessage = $"🚨 CRITICAL: {message}\n" +
                          $"Distributor: {error.Distributor}\n" +
                          $"Endpoint: {error.Endpoint}\n" +
                          $"Error: {error.ErrorMessage}\n" +
                          $"Order ID: {error.OrderId}";
        
        await SendSlackNotificationAsync("#dif-alerts", slackMessage);

        // Send email
        await SendEmailNotificationAsync(
            new List<string> { "ops-team@freshprints.com", "engineering@freshprints.com" },
            $"[CRITICAL] DIF Alert: {message}",
            slackMessage);
    }

    /// <inheritdoc />
    public async Task SendWarningAlertAsync(string message, ApiError error)
    {
        error.Severity = ErrorSeverity.Warning;
        await LogApiErrorAsync(error);

        // Warning alerts are batched - just add to queue
        var slackMessage = $"⚠️ WARNING: {message}\n" +
                          $"Distributor: {error.Distributor}\n" +
                          $"Endpoint: {error.Endpoint}\n" +
                          $"Error: {error.ErrorMessage}";
        
        _slackMessages.Add(slackMessage);
    }

    /// <inheritdoc />
    public async Task SendInfoNotificationAsync(string message, ApiError error)
    {
        error.Severity = ErrorSeverity.Info;
        await LogApiErrorAsync(error);
        // Info notifications are logged but not actively alerted
    }

    /// <inheritdoc />
    public Task<ApiError> LogApiErrorAsync(ApiError error)
    {
        if (error.ErrorId == Guid.Empty)
        {
            error.ErrorId = Guid.NewGuid();
        }
        
        if (error.Timestamp == default)
        {
            error.Timestamp = DateTime.UtcNow;
        }

        _errors.Add(error);
        return Task.FromResult(error);
    }

    /// <inheritdoc />
    public Task<List<ApiError>> GetRecentErrorsAsync(
        string? distributorId = null, 
        ErrorSeverity? severity = null, 
        int hours = 24)
    {
        var cutoff = DateTime.UtcNow.AddHours(-hours);
        
        var query = _errors.Where(e => e.Timestamp >= cutoff);

        if (!string.IsNullOrEmpty(distributorId))
        {
            query = query.Where(e => e.Distributor.Equals(distributorId, StringComparison.OrdinalIgnoreCase));
        }

        if (severity.HasValue)
        {
            query = query.Where(e => e.Severity == severity.Value);
        }

        return Task.FromResult(query.OrderByDescending(e => e.Timestamp).ToList());
    }

    /// <inheritdoc />
    public Task<Dictionary<ErrorSeverity, int>> GetErrorCountsBySeverityAsync(int hours = 24)
    {
        var cutoff = DateTime.UtcNow.AddHours(-hours);
        
        var counts = _errors
            .Where(e => e.Timestamp >= cutoff)
            .GroupBy(e => e.Severity)
            .ToDictionary(g => g.Key, g => g.Count());

        // Ensure all severity levels are present
        foreach (ErrorSeverity severity in Enum.GetValues(typeof(ErrorSeverity)))
        {
            if (!counts.ContainsKey(severity))
            {
                counts[severity] = 0;
            }
        }

        return Task.FromResult(counts);
    }

    /// <inheritdoc />
    public Task ResolveErrorAsync(Guid errorId, string resolutionNotes)
    {
        var error = _errors.FirstOrDefault(e => e.ErrorId == errorId);
        if (error != null)
        {
            error.IsResolved = true;
            error.ResolutionNotes = resolutionNotes;
            error.ResolvedAt = DateTime.UtcNow;
        }
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task LogApiRequestAsync(ApiRequestLog log)
    {
        if (log.LogId == Guid.Empty)
        {
            log.LogId = Guid.NewGuid();
        }
        
        if (log.Timestamp == default)
        {
            log.Timestamp = DateTime.UtcNow;
        }

        _requestLogs.Add(log);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<ApiRequestStats> GetRequestStatsAsync(string distributorId, int hours = 24)
    {
        var cutoff = DateTime.UtcNow.AddHours(-hours);
        
        var logs = _requestLogs
            .Where(l => l.Timestamp >= cutoff && 
                       l.Distributor.Equals(distributorId, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var stats = new ApiRequestStats
        {
            TotalRequests = logs.Count,
            SuccessfulRequests = logs.Count(l => l.Success),
            FailedRequests = logs.Count(l => !l.Success),
            AverageResponseTimeMs = logs.Any() ? logs.Average(l => l.LatencyMs) : 0,
            RateLimitHits = logs.Count(l => l.ResponseCode == 429),
            PeriodStart = cutoff,
            PeriodEnd = DateTime.UtcNow
        };

        return Task.FromResult(stats);
    }

    /// <inheritdoc />
    public Task<bool> SendSlackNotificationAsync(string channel, string message)
    {
        _slackMessages.Add($"[{channel}] {message}");
        Console.WriteLine($"[MOCK SLACK] {channel}: {message}");
        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<bool> SendEmailNotificationAsync(List<string> recipients, string subject, string body)
    {
        _emails.Add((recipients, subject, body));
        Console.WriteLine($"[MOCK EMAIL] To: {string.Join(", ", recipients)} | Subject: {subject}");
        return Task.FromResult(true);
    }

    /// <summary>
    /// Gets all logged Slack messages (for testing purposes).
    /// </summary>
    public IReadOnlyCollection<string> GetSlackMessages() => _slackMessages.ToArray();

    /// <summary>
    /// Gets all logged emails (for testing purposes).
    /// </summary>
    public IReadOnlyCollection<(List<string> Recipients, string Subject, string Body)> GetEmails() => _emails.ToArray();
}


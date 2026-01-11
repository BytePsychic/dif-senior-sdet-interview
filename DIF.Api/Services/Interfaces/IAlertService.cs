using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIF.Api.Models.Domain;

namespace DIF.Api.Services.Interfaces;

/// <summary>
/// Service interface for API error monitoring and alerting.
/// Handles logging, notifications, and alert management.
/// </summary>
public interface IAlertService
{
    /// <summary>
    /// Sends a critical alert (order placement failures, auth issues, server errors).
    /// Triggers immediate Slack + Email notifications.
    /// </summary>
    /// <param name="message">Alert message.</param>
    /// <param name="error">API error details.</param>
    /// <returns>Task.</returns>
    Task SendCriticalAlertAsync(string message, ApiError error);

    /// <summary>
    /// Sends a warning alert (tracking update failures, rate limits).
    /// Batched and sent every 2 hours.
    /// </summary>
    /// <param name="message">Alert message.</param>
    /// <param name="error">API error details.</param>
    /// <returns>Task.</returns>
    Task SendWarningAlertAsync(string message, ApiError error);

    /// <summary>
    /// Sends an info-level notification (validation errors).
    /// Logged but not actively alerted.
    /// </summary>
    /// <param name="message">Notification message.</param>
    /// <param name="error">API error details.</param>
    /// <returns>Task.</returns>
    Task SendInfoNotificationAsync(string message, ApiError error);

    /// <summary>
    /// Logs an API error to the error tracking system.
    /// </summary>
    /// <param name="error">API error to log.</param>
    /// <returns>The logged error with assigned ID.</returns>
    Task<ApiError> LogApiErrorAsync(ApiError error);

    /// <summary>
    /// Gets recent API errors with optional filtering.
    /// </summary>
    /// <param name="distributorId">Filter by distributor (optional).</param>
    /// <param name="severity">Filter by severity (optional).</param>
    /// <param name="hours">Hours to look back (default 24).</param>
    /// <returns>List of matching errors.</returns>
    Task<List<ApiError>> GetRecentErrorsAsync(
        string? distributorId = null, 
        ErrorSeverity? severity = null, 
        int hours = 24);

    /// <summary>
    /// Gets error count by severity in the last N hours.
    /// </summary>
    /// <param name="hours">Hours to look back.</param>
    /// <returns>Dictionary of severity to count.</returns>
    Task<Dictionary<ErrorSeverity, int>> GetErrorCountsBySeverityAsync(int hours = 24);

    /// <summary>
    /// Marks an error as resolved.
    /// </summary>
    /// <param name="errorId">Error ID.</param>
    /// <param name="resolutionNotes">Notes about how the error was resolved.</param>
    /// <returns>Task.</returns>
    Task ResolveErrorAsync(Guid errorId, string resolutionNotes);

    /// <summary>
    /// Logs an API request for monitoring purposes.
    /// </summary>
    /// <param name="log">Request log entry.</param>
    /// <returns>Task.</returns>
    Task LogApiRequestAsync(ApiRequestLog log);

    /// <summary>
    /// Gets API request statistics for a distributor.
    /// </summary>
    /// <param name="distributorId">Distributor ID.</param>
    /// <param name="hours">Hours to look back.</param>
    /// <returns>Request statistics.</returns>
    Task<ApiRequestStats> GetRequestStatsAsync(string distributorId, int hours = 24);

    /// <summary>
    /// Sends a Slack notification.
    /// </summary>
    /// <param name="channel">Slack channel.</param>
    /// <param name="message">Message to send.</param>
    /// <returns>True if sent successfully.</returns>
    Task<bool> SendSlackNotificationAsync(string channel, string message);

    /// <summary>
    /// Sends an email notification.
    /// </summary>
    /// <param name="recipients">Email recipients.</param>
    /// <param name="subject">Email subject.</param>
    /// <param name="body">Email body.</param>
    /// <returns>True if sent successfully.</returns>
    Task<bool> SendEmailNotificationAsync(List<string> recipients, string subject, string body);
}

/// <summary>
/// API request statistics.
/// </summary>
public class ApiRequestStats
{
    /// <summary>
    /// Total number of requests.
    /// </summary>
    public int TotalRequests { get; set; }

    /// <summary>
    /// Number of successful requests.
    /// </summary>
    public int SuccessfulRequests { get; set; }

    /// <summary>
    /// Number of failed requests.
    /// </summary>
    public int FailedRequests { get; set; }

    /// <summary>
    /// Success rate as percentage.
    /// </summary>
    public double SuccessRate => TotalRequests > 0 ? (double)SuccessfulRequests / TotalRequests * 100 : 0;

    /// <summary>
    /// Average response time in milliseconds.
    /// </summary>
    public double AverageResponseTimeMs { get; set; }

    /// <summary>
    /// Number of rate limit hits.
    /// </summary>
    public int RateLimitHits { get; set; }

    /// <summary>
    /// Time period start.
    /// </summary>
    public DateTime PeriodStart { get; set; }

    /// <summary>
    /// Time period end.
    /// </summary>
    public DateTime PeriodEnd { get; set; }
}


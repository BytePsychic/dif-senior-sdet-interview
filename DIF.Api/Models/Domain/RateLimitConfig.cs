using System;

namespace DIF.Api.Models.Domain;

/// <summary>
/// Configuration for rate limiting per distributor.
/// S&amp;S has a hard limit of 60 requests/minute with recommended 90% threshold (54 req/min).
/// </summary>
public class RateLimitConfig
{
    /// <summary>
    /// Distributor ID this configuration applies to.
    /// </summary>
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Distributor name for display.
    /// </summary>
    public string DistributorName { get; set; } = string.Empty;

    /// <summary>
    /// Maximum requests allowed per minute (hard limit).
    /// S&amp;S: 60 requests/minute
    /// </summary>
    public int RequestsPerMinute { get; set; }

    /// <summary>
    /// Threshold percentage at which to start queuing (default 90%).
    /// </summary>
    public int ThresholdPercentage { get; set; } = 90;

    /// <summary>
    /// Calculated threshold request count (RequestsPerMinute * ThresholdPercentage / 100).
    /// </summary>
    public int ThresholdRequestCount => (int)(RequestsPerMinute * ThresholdPercentage / 100.0);

    /// <summary>
    /// Additional burst allowance for priority requests.
    /// </summary>
    public int BurstAllowance { get; set; }

    /// <summary>
    /// Current number of requests made in the current window.
    /// </summary>
    public int CurrentRequestCount { get; set; }

    /// <summary>
    /// Start of the current rate limit window.
    /// </summary>
    public DateTime WindowStart { get; set; }

    /// <summary>
    /// Number of requests remaining in current window.
    /// </summary>
    public int RemainingRequests => Math.Max(0, RequestsPerMinute - CurrentRequestCount);

    /// <summary>
    /// Whether we're approaching the rate limit (above threshold).
    /// </summary>
    public bool IsApproachingLimit => CurrentRequestCount >= ThresholdRequestCount;

    /// <summary>
    /// Whether we've hit the rate limit.
    /// </summary>
    public bool IsRateLimited => CurrentRequestCount >= RequestsPerMinute;

    /// <summary>
    /// Time until the window resets.
    /// </summary>
    public TimeSpan TimeUntilReset => WindowStart.AddMinutes(1) - DateTime.UtcNow;

    /// <summary>
    /// Current queue depth (pending requests).
    /// </summary>
    public int QueueDepth { get; set; }

    /// <summary>
    /// Whether the queue is at alert threshold (>100 pending).
    /// </summary>
    public bool QueueAtAlertThreshold => QueueDepth > 100;

    /// <summary>
    /// Last time a rate limit (429) was received.
    /// </summary>
    public DateTime? LastRateLimitHit { get; set; }

    /// <summary>
    /// Total rate limit hits in the last 24 hours.
    /// </summary>
    public int RateLimitHitsLast24Hours { get; set; }
}

/// <summary>
/// Request priority levels for the rate limit queue.
/// </summary>
public enum RequestPriority
{
    /// <summary>
    /// P0 - Order placement (highest priority).
    /// </summary>
    OrderPlacement = 0,

    /// <summary>
    /// P1 - Tracking updates.
    /// </summary>
    TrackingUpdate = 1,

    /// <summary>
    /// P2 - Product data (lowest priority).
    /// </summary>
    ProductData = 2
}

/// <summary>
/// Represents a queued API request waiting for rate limit availability.
/// </summary>
public class QueuedRequest
{
    /// <summary>
    /// Unique identifier for the queued request.
    /// </summary>
    public Guid RequestId { get; set; }

    /// <summary>
    /// Distributor ID this request is for.
    /// </summary>
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Priority level of the request.
    /// </summary>
    public RequestPriority Priority { get; set; }

    /// <summary>
    /// Timestamp when the request was queued.
    /// </summary>
    public DateTime QueuedAt { get; set; }

    /// <summary>
    /// API endpoint to call.
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// HTTP method.
    /// </summary>
    public string HttpMethod { get; set; } = string.Empty;

    /// <summary>
    /// Request payload if applicable.
    /// </summary>
    public string? Payload { get; set; }

    /// <summary>
    /// Correlation ID for tracing.
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;

    /// <summary>
    /// Number of retry attempts made.
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    /// Maximum retry attempts allowed.
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Time to wait for this request (position in queue).
    /// </summary>
    public TimeSpan EstimatedWaitTime { get; set; }
}

/// <summary>
/// Retry configuration for failed requests.
/// </summary>
public class RetryConfig
{
    /// <summary>
    /// Maximum number of retry attempts.
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Delay intervals for retries (1s, 5s, 15s).
    /// </summary>
    public int[] RetryDelaysSeconds { get; set; } = { 1, 5, 15 };

    /// <summary>
    /// Whether to use exponential backoff.
    /// </summary>
    public bool UseExponentialBackoff { get; set; } = true;

    /// <summary>
    /// Maximum delay between retries in seconds.
    /// </summary>
    public int MaxDelaySeconds { get; set; } = 60;

    /// <summary>
    /// Get the delay for a specific retry attempt.
    /// </summary>
    public TimeSpan GetRetryDelay(int attemptNumber)
    {
        if (attemptNumber < 0 || attemptNumber >= RetryDelaysSeconds.Length)
        {
            return TimeSpan.FromSeconds(MaxDelaySeconds);
        }
        return TimeSpan.FromSeconds(RetryDelaysSeconds[attemptNumber]);
    }
}


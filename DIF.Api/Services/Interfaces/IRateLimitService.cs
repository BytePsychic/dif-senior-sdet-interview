using System;
using System.Threading.Tasks;
using DIF.Api.Models.Domain;

namespace DIF.Api.Services.Interfaces;

/// <summary>
/// Service interface for rate limiting management.
/// Prevents rate limit violations by managing request flow.
/// </summary>
public interface IRateLimitService
{
    /// <summary>
    /// Checks if a request can be made to the specified distributor.
    /// Takes into account current request count and priority.
    /// </summary>
    /// <param name="distributorId">Distributor ID.</param>
    /// <param name="priority">Request priority level.</param>
    /// <returns>True if request can proceed, false if should be queued.</returns>
    Task<bool> CanMakeRequestAsync(string distributorId, RequestPriority priority);

    /// <summary>
    /// Records a request being made to update the rate limit counter.
    /// </summary>
    /// <param name="distributorId">Distributor ID.</param>
    /// <returns>Task.</returns>
    Task RecordRequestAsync(string distributorId);

    /// <summary>
    /// Gets the current rate limit status for a distributor.
    /// </summary>
    /// <param name="distributorId">Distributor ID.</param>
    /// <returns>Rate limit configuration with current status.</returns>
    Task<RateLimitConfig> GetRateLimitStatusAsync(string distributorId);

    /// <summary>
    /// Queues a request for later execution when rate limited.
    /// </summary>
    /// <param name="request">Queued request details.</param>
    /// <returns>Task.</returns>
    Task QueueRequestAsync(QueuedRequest request);

    /// <summary>
    /// Gets the next request from the queue for a distributor.
    /// Returns requests in priority order (P0 > P1 > P2).
    /// </summary>
    /// <param name="distributorId">Distributor ID.</param>
    /// <returns>Next queued request or null if empty.</returns>
    Task<QueuedRequest?> DequeueRequestAsync(string distributorId);

    /// <summary>
    /// Gets the current queue depth for a distributor.
    /// </summary>
    /// <param name="distributorId">Distributor ID.</param>
    /// <returns>Number of requests in queue.</returns>
    Task<int> GetQueueDepthAsync(string distributorId);

    /// <summary>
    /// Records a rate limit (429) response from the distributor.
    /// </summary>
    /// <param name="distributorId">Distributor ID.</param>
    /// <returns>Task.</returns>
    Task RecordRateLimitHitAsync(string distributorId);

    /// <summary>
    /// Gets the retry delay based on attempt number.
    /// Uses configured delays: 1s, 5s, 15s.
    /// </summary>
    /// <param name="attemptNumber">Current attempt number (0-based).</param>
    /// <returns>Delay to wait before retry.</returns>
    TimeSpan GetRetryDelay(int attemptNumber);

    /// <summary>
    /// Resets the rate limit window for a distributor.
    /// Called when the window expires.
    /// </summary>
    /// <param name="distributorId">Distributor ID.</param>
    /// <returns>Task.</returns>
    Task ResetWindowAsync(string distributorId);

    /// <summary>
    /// Gets the remaining requests in the current window from API response header.
    /// </summary>
    /// <param name="distributorId">Distributor ID.</param>
    /// <param name="remainingFromHeader">Value from X-Rate-Limit-Remaining header.</param>
    /// <returns>Task.</returns>
    Task UpdateRemainingFromHeaderAsync(string distributorId, int remainingFromHeader);
}


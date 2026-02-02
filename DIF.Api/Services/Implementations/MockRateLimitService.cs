using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIF.Api.Models.Domain;
using DIF.Api.Services.Interfaces;

namespace DIF.Api.Services.Implementations;

/// <summary>
/// Mock implementation of IRateLimitService for testing.
/// Simulates rate limiting behavior without actual Redis queue.
/// </summary>
public class MockRateLimitService : IRateLimitService
{
    private readonly ConcurrentDictionary<string, RateLimitConfig> _rateLimits;
    private readonly ConcurrentDictionary<string, List<QueuedRequest>> _queues;
    private readonly int[] _retryDelays = { 1, 5, 15 };

    public MockRateLimitService()
    {
        _rateLimits = new ConcurrentDictionary<string, RateLimitConfig>();
        _queues = new ConcurrentDictionary<string, List<QueuedRequest>>();

        // Initialize default rate limits for known distributors
        InitializeRateLimits();
    }

    private void InitializeRateLimits()
    {
        var distributors = new[]
        {
            ("ss", "S&S Activewear", 60),
            ("img", "IMG", 100),
            ("sanmar", "SanMar", 120),
            ("staton", "Staton", 60),
            ("carolina", "Carolina Made", 60)
        };

        foreach (var (id, name, rpm) in distributors)
        {
            _rateLimits[id] = new RateLimitConfig
            {
                DistributorId = id,
                DistributorName = name,
                RequestsPerMinute = rpm,
                ThresholdPercentage = 90,
                BurstAllowance = 5,
                CurrentRequestCount = 0,
                WindowStart = DateTime.UtcNow,
                QueueDepth = 0
            };
            _queues[id] = new List<QueuedRequest>();
        }
    }

    /// <inheritdoc />
    public Task<bool> CanMakeRequestAsync(string distributorId, RequestPriority priority)
    {
        var config = GetOrCreateConfig(distributorId);
        
        // Reset window if needed
        if ((DateTime.UtcNow - config.WindowStart).TotalSeconds >= 60)
        {
            config.CurrentRequestCount = 0;
            config.WindowStart = DateTime.UtcNow;
        }

        // P0 (Order placement) can use burst allowance
        var limit = priority == RequestPriority.OrderPlacement
            ? config.RequestsPerMinute + config.BurstAllowance
            : config.ThresholdRequestCount;

        return Task.FromResult(config.CurrentRequestCount <= limit);
    }

    /// <inheritdoc />
    public Task RecordRequestAsync(string distributorId)
    {
        var config = GetOrCreateConfig(distributorId);
        
        // Reset window if needed
        if ((DateTime.UtcNow - config.WindowStart).TotalSeconds >= 60)
        {
            config.CurrentRequestCount = 0;
            config.WindowStart = DateTime.UtcNow;
        }

        config.CurrentRequestCount++;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<RateLimitConfig> GetRateLimitStatusAsync(string distributorId)
    {
        var config = GetOrCreateConfig(distributorId);
        
        // Reset window if needed
        if ((DateTime.UtcNow - config.WindowStart).TotalSeconds >= 60)
        {
            config.CurrentRequestCount = 0;
            config.WindowStart = DateTime.UtcNow;
        }

        // Update queue depth
        if (_queues.TryGetValue(distributorId, out var queue))
        {
            config.QueueDepth = queue.Count;
        }

        return Task.FromResult(config);
    }

    /// <inheritdoc />
    public Task QueueRequestAsync(QueuedRequest request)
    {
        var queue = _queues.GetOrAdd(request.DistributorId, _ => new List<QueuedRequest>());
        
        lock (queue)
        {
            request.QueuedAt = DateTime.UtcNow;
            queue.Add(request);
            
            // Update queue depth in rate limit config
            if (_rateLimits.TryGetValue(request.DistributorId, out var config))
            {
                config.QueueDepth = queue.Count;
            }
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<QueuedRequest?> DequeueRequestAsync(string distributorId)
    {
        if (!_queues.TryGetValue(distributorId, out var queue))
        {
            return Task.FromResult<QueuedRequest?>(null);
        }

        lock (queue)
        {
            if (queue.Count == 0)
            {
                return Task.FromResult<QueuedRequest?>(null);
            }

            // Get highest priority request (lowest enum value)
            var request = queue
                .OrderBy(r => r.Priority)
                .ThenBy(r => r.QueuedAt)
                .FirstOrDefault();

            if (request != null)
            {
                queue.Remove(request);
                
                // Update queue depth
                if (_rateLimits.TryGetValue(distributorId, out var config))
                {
                    config.QueueDepth = queue.Count;
                }
            }

            return Task.FromResult(request);
        }
    }

    /// <inheritdoc />
    public Task<int> GetQueueDepthAsync(string distributorId)
    {
        if (_queues.TryGetValue(distributorId, out var queue))
        {
            lock (queue)
            {
                return Task.FromResult(queue.Count);
            }
        }
        return Task.FromResult(0);
    }

    /// <inheritdoc />
    public Task RecordRateLimitHitAsync(string distributorId)
    {
        var config = GetOrCreateConfig(distributorId);
        config.LastRateLimitHit = DateTime.UtcNow;
        config.RateLimitHitsLast24Hours++;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public TimeSpan GetRetryDelay(int attemptNumber)
    {
        var index = Math.Min(attemptNumber, _retryDelays.Length - 1);
        return TimeSpan.FromSeconds(_retryDelays[index]);
    }

    /// <inheritdoc />
    public Task ResetWindowAsync(string distributorId)
    {
        var config = GetOrCreateConfig(distributorId);
        config.CurrentRequestCount = 0;
        config.WindowStart = DateTime.UtcNow;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UpdateRemainingFromHeaderAsync(string distributorId, int remainingFromHeader)
    {
        var config = GetOrCreateConfig(distributorId);
        // Calculate current count from remaining
        config.CurrentRequestCount = config.RequestsPerMinute - remainingFromHeader;
        return Task.CompletedTask;
    }

    private RateLimitConfig GetOrCreateConfig(string distributorId)
    {
        return _rateLimits.GetOrAdd(distributorId, id => new RateLimitConfig
        {
            DistributorId = id,
            DistributorName = id.ToUpper(),
            RequestsPerMinute = 60,
            ThresholdPercentage = 90,
            BurstAllowance = 5,
            CurrentRequestCount = 0,
            WindowStart = DateTime.UtcNow
        });
    }
}


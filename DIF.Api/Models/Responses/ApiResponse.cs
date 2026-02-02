using System;
using System.Collections.Generic;

namespace DIF.Api.Models.Responses;

/// <summary>
/// Standard API response wrapper.
/// </summary>
/// <typeparam name="T">Type of the data payload.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Whether the request was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Response data payload.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Human-readable message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// List of error messages if the request failed.
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Timestamp of the response.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Correlation ID for request tracing.
    /// </summary>
    public string? CorrelationId { get; set; }

    /// <summary>
    /// Creates a successful response with data.
    /// </summary>
    public static ApiResponse<T> Ok(T data, string message = "Success")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    /// <summary>
    /// Creates a failed response with errors.
    /// </summary>
    public static ApiResponse<T> Fail(string message, params string[] errors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = new List<string>(errors)
        };
    }

    /// <summary>
    /// Creates a failed response with a list of errors.
    /// </summary>
    public static ApiResponse<T> Fail(string message, List<string> errors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
}

/// <summary>
/// Non-generic API response for operations without data.
/// </summary>
public class ApiResponse : ApiResponse<object>
{
    /// <summary>
    /// Creates a successful response without data.
    /// </summary>
    public static ApiResponse Ok(string message = "Success")
    {
        return new ApiResponse
        {
            Success = true,
            Message = message
        };
    }

    /// <summary>
    /// Creates a new failed response.
    /// </summary>
    public new static ApiResponse Fail(string message, params string[] errors)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            Errors = new List<string>(errors)
        };
    }
}

/// <summary>
/// Paginated response wrapper.
/// </summary>
/// <typeparam name="T">Type of items in the list.</typeparam>
public class PaginatedResponse<T>
{
    /// <summary>
    /// Whether the request was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// List of items for the current page.
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// Current page number (1-based).
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of items across all pages.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

    /// <summary>
    /// Whether there is a next page.
    /// </summary>
    public bool HasNextPage => Page < TotalPages;

    /// <summary>
    /// Whether there is a previous page.
    /// </summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>
    /// Human-readable message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of the response.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a paginated response.
    /// </summary>
    public static PaginatedResponse<T> Create(List<T> items, int page, int pageSize, int totalItems, string message = "Success")
    {
        return new PaginatedResponse<T>
        {
            Success = true,
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            Message = message
        };
    }
}

/// <summary>
/// Response for order placement.
/// </summary>
public class OrderResponse
{
    /// <summary>
    /// Internal order ID.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Distributor order ID.
    /// </summary>
    public string DistributorOrderId { get; set; } = string.Empty;

    /// <summary>
    /// PO number.
    /// </summary>
    public string PoNumber { get; set; } = string.Empty;

    /// <summary>
    /// Order status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Warehouse code.
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// Warehouse name.
    /// </summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// Expected delivery date.
    /// </summary>
    public DateTime? ExpectedDeliveryDate { get; set; }

    /// <summary>
    /// Order cost breakdown.
    /// </summary>
    public OrderCostResponse? Costs { get; set; }

    /// <summary>
    /// Order timestamp.
    /// </summary>
    public DateTime OrderTimestamp { get; set; }
}

/// <summary>
/// Response for order costs.
/// </summary>
public class OrderCostResponse
{
    /// <summary>
    /// Merchandise subtotal.
    /// </summary>
    public decimal Subtotal { get; set; }

    /// <summary>
    /// Shipping cost.
    /// </summary>
    public decimal Shipping { get; set; }

    /// <summary>
    /// Tax amount.
    /// </summary>
    public decimal? Tax { get; set; }

    /// <summary>
    /// Small order fee if applicable.
    /// </summary>
    public decimal? SmallOrderFee { get; set; }

    /// <summary>
    /// Total cost.
    /// </summary>
    public decimal Total { get; set; }
}

/// <summary>
/// Response for tracking information.
/// </summary>
public class TrackingResponse
{
    /// <summary>
    /// Shipment ID.
    /// </summary>
    public Guid ShipmentId { get; set; }

    /// <summary>
    /// Order ID.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Tracking number.
    /// </summary>
    public string TrackingNumber { get; set; } = string.Empty;

    /// <summary>
    /// Tracking URL.
    /// </summary>
    public string TrackingUrl { get; set; } = string.Empty;

    /// <summary>
    /// Carrier name.
    /// </summary>
    public string Carrier { get; set; } = string.Empty;

    /// <summary>
    /// Current status.
    /// </summary>
    public string CurrentStatus { get; set; } = string.Empty;

    /// <summary>
    /// Ship date.
    /// </summary>
    public DateTime? ShipDate { get; set; }

    /// <summary>
    /// Estimated delivery date.
    /// </summary>
    public DateTime? EstimatedDelivery { get; set; }

    /// <summary>
    /// Actual delivery date.
    /// </summary>
    public DateTime? ActualDeliveryDate { get; set; }

    /// <summary>
    /// Number of boxes.
    /// </summary>
    public int NumBoxes { get; set; }

    /// <summary>
    /// Total weight.
    /// </summary>
    public decimal TotalWeight { get; set; }

    /// <summary>
    /// Origin warehouse.
    /// </summary>
    public string OriginWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// Destination address.
    /// </summary>
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// Shipment leg type (L1, L2, L3).
    /// </summary>
    public string LegType { get; set; } = string.Empty;

    /// <summary>
    /// Last updated timestamp.
    /// </summary>
    public DateTime LastUpdated { get; set; }
}

/// <summary>
/// Response for rate limit status.
/// </summary>
public class RateLimitStatusResponse
{
    /// <summary>
    /// Distributor ID.
    /// </summary>
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Distributor name.
    /// </summary>
    public string DistributorName { get; set; } = string.Empty;

    /// <summary>
    /// Maximum requests per minute.
    /// </summary>
    public int RequestsPerMinute { get; set; }

    /// <summary>
    /// Current request count in window.
    /// </summary>
    public int CurrentRequestCount { get; set; }

    /// <summary>
    /// Remaining requests in window.
    /// </summary>
    public int RemainingRequests { get; set; }

    /// <summary>
    /// Whether approaching limit.
    /// </summary>
    public bool IsApproachingLimit { get; set; }

    /// <summary>
    /// Whether rate limited.
    /// </summary>
    public bool IsRateLimited { get; set; }

    /// <summary>
    /// Seconds until window resets.
    /// </summary>
    public int SecondsUntilReset { get; set; }

    /// <summary>
    /// Current queue depth.
    /// </summary>
    public int QueueDepth { get; set; }
}

/// <summary>
/// Response for health check.
/// </summary>
public class HealthResponse
{
    /// <summary>
    /// Overall health status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// API version.
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Server timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Individual component health statuses.
    /// </summary>
    public Dictionary<string, ComponentHealth> Components { get; set; } = new();
}

/// <summary>
/// Health status of an individual component.
/// </summary>
public class ComponentHealth
{
    /// <summary>
    /// Component status (Healthy, Degraded, Unhealthy).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Description or error message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Last successful check.
    /// </summary>
    public DateTime? LastSuccessful { get; set; }

    /// <summary>
    /// Response time in milliseconds.
    /// </summary>
    public long? ResponseTimeMs { get; set; }
}


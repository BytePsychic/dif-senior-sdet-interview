using System;

namespace DIF.Api.Models.Domain;

/// <summary>
/// Represents an API error logged for monitoring and alerting.
/// Used for tracking API failures, timeouts, and rate limit issues.
/// </summary>
public class ApiError
{
    /// <summary>
    /// Unique identifier for the error.
    /// </summary>
    public Guid ErrorId { get; set; }

    /// <summary>
    /// Timestamp when the error occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Distributor ID where the error occurred.
    /// </summary>
    public string Distributor { get; set; } = string.Empty;

    /// <summary>
    /// API endpoint that failed.
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// HTTP method used (GET, POST, etc.).
    /// </summary>
    public string HttpMethod { get; set; } = string.Empty;

    /// <summary>
    /// HTTP status code returned.
    /// </summary>
    public int? HttpStatusCode { get; set; }

    /// <summary>
    /// Error type classification.
    /// CRITICAL: 401, 500, 503, order placement failures
    /// WARNING: 429 (rate limit)
    /// INFO: 400, 404 (validation errors)
    /// </summary>
    public string ErrorType { get; set; } = string.Empty;

    /// <summary>
    /// Error severity level.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Error message or description.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Original request payload (sanitized of sensitive data).
    /// </summary>
    public string RequestPayload { get; set; } = string.Empty;

    /// <summary>
    /// Response body if available.
    /// </summary>
    public string Response { get; set; } = string.Empty;

    /// <summary>
    /// Associated order ID if applicable.
    /// </summary>
    public Guid? OrderId { get; set; }

    /// <summary>
    /// Number of retry attempts made.
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    /// Response latency in milliseconds.
    /// </summary>
    public long? LatencyMs { get; set; }

    /// <summary>
    /// Whether this error has been resolved.
    /// </summary>
    public bool IsResolved { get; set; }

    /// <summary>
    /// Resolution notes if resolved.
    /// </summary>
    public string ResolutionNotes { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp when the error was resolved.
    /// </summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>
    /// Correlation ID for tracing across services.
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;

    /// <summary>
    /// User or service that initiated the request.
    /// </summary>
    public string InitiatedBy { get; set; } = string.Empty;
}

/// <summary>
/// Error severity levels for alerting.
/// </summary>
public enum ErrorSeverity
{
    /// <summary>
    /// Informational - validation errors, not found, etc.
    /// </summary>
    Info = 0,

    /// <summary>
    /// Warning - rate limits, temporary issues.
    /// </summary>
    Warning = 1,

    /// <summary>
    /// Critical - order failures, auth issues, server errors.
    /// Triggers immediate Slack + Email alerts.
    /// </summary>
    Critical = 2
}

/// <summary>
/// Represents an API request log entry.
/// </summary>
public class ApiRequestLog
{
    /// <summary>
    /// Unique identifier for the log entry.
    /// </summary>
    public Guid LogId { get; set; }

    /// <summary>
    /// Timestamp of the request.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Distributor ID.
    /// </summary>
    public string Distributor { get; set; } = string.Empty;

    /// <summary>
    /// API endpoint called.
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// HTTP method.
    /// </summary>
    public string HttpMethod { get; set; } = string.Empty;

    /// <summary>
    /// HTTP status code returned.
    /// </summary>
    public int ResponseCode { get; set; }

    /// <summary>
    /// Response latency in milliseconds.
    /// </summary>
    public long LatencyMs { get; set; }

    /// <summary>
    /// Whether the request was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Correlation ID for tracing.
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>
/// Represents an order exception (backorder, substitution, discontinuation).
/// </summary>
public class OrderException
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public Guid ExceptionId { get; set; }

    /// <summary>
    /// Order ID this exception belongs to.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Type of exception.
    /// </summary>
    public OrderExceptionType ExceptionType { get; set; }

    /// <summary>
    /// SKU affected by the exception.
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Quantity affected.
    /// </summary>
    public int QuantityAffected { get; set; }

    /// <summary>
    /// Substitute SKU if applicable.
    /// </summary>
    public string? SubstituteSku { get; set; }

    /// <summary>
    /// Expected availability date for backorders.
    /// </summary>
    public DateTime? ExpectedAvailability { get; set; }

    /// <summary>
    /// Description of the exception.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp when the exception was recorded.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Whether the exception has been resolved.
    /// </summary>
    public bool IsResolved { get; set; }
}

/// <summary>
/// Types of order exceptions.
/// </summary>
public enum OrderExceptionType
{
    /// <summary>
    /// Item is on backorder.
    /// </summary>
    Backorder = 0,

    /// <summary>
    /// Item was substituted with another SKU.
    /// </summary>
    Substitution = 1,

    /// <summary>
    /// Item has been discontinued.
    /// </summary>
    Discontinued = 2,

    /// <summary>
    /// Partial shipment due to stock issues.
    /// </summary>
    PartialShipment = 3,

    /// <summary>
    /// Other exception type.
    /// </summary>
    Other = 99
}


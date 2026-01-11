using System;
using System.Collections.Generic;

namespace DIF.Api.Models.Domain;

/// <summary>
/// Represents a distributor in the integration framework.
/// Fresh Prints works with 7 major distributors: S&amp;S, IMG, Sanmar, Staton, 
/// Carolina Made, LA Apparel, and Driving Impressions.
/// </summary>
public class Distributor
{
    /// <summary>
    /// Unique identifier for the distributor.
    /// </summary>
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Display name of the distributor.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Short code for the distributor.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Base URL for the distributor's API.
    /// </summary>
    public string ApiBaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Whether API integration is available for this distributor.
    /// Note: LA Apparel and Driving Impressions lack APIs.
    /// </summary>
    public bool HasApiIntegration { get; set; }

    /// <summary>
    /// Whether the distributor is currently active/enabled.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Rate limit configuration for this distributor.
    /// </summary>
    public RateLimitConfig? RateLimitConfig { get; set; }

    /// <summary>
    /// Supported shipping methods.
    /// </summary>
    public List<ShippingOption> ShippingOptions { get; set; } = new();

    /// <summary>
    /// Warehouses operated by this distributor.
    /// </summary>
    public List<Warehouse> Warehouses { get; set; } = new();

    /// <summary>
    /// Contact email for the distributor.
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// Contact phone for the distributor.
    /// </summary>
    public string ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// Account number with the distributor.
    /// </summary>
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// API version being used.
    /// </summary>
    public string ApiVersion { get; set; } = string.Empty;

    /// <summary>
    /// Last time we successfully connected to the API.
    /// </summary>
    public DateTime? LastSuccessfulConnection { get; set; }

    /// <summary>
    /// Current API health status.
    /// </summary>
    public ApiHealthStatus HealthStatus { get; set; } = ApiHealthStatus.Unknown;

    /// <summary>
    /// Notes about the distributor integration.
    /// </summary>
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// API health status for a distributor.
/// </summary>
public enum ApiHealthStatus
{
    /// <summary>
    /// Health status unknown (not yet checked).
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// API is healthy and responding normally.
    /// </summary>
    Healthy = 1,

    /// <summary>
    /// API is degraded (slow responses or partial failures).
    /// </summary>
    Degraded = 2,

    /// <summary>
    /// API is unhealthy (not responding or major failures).
    /// </summary>
    Unhealthy = 3,

    /// <summary>
    /// No API available for this distributor.
    /// </summary>
    NoApi = 4
}

/// <summary>
/// Represents a shipping option/method available from a distributor.
/// </summary>
public class ShippingOption
{
    /// <summary>
    /// Internal identifier.
    /// </summary>
    public Guid ShippingOptionId { get; set; }

    /// <summary>
    /// Shipping method code (e.g., "1" for Ground in S&amp;S).
    /// </summary>
    public string MethodCode { get; set; } = string.Empty;

    /// <summary>
    /// Display name (e.g., "Ground", "Next Day", "2nd Day").
    /// </summary>
    public string MethodName { get; set; } = string.Empty;

    /// <summary>
    /// Carrier name (e.g., "UPS", "FedEx").
    /// </summary>
    public string Carrier { get; set; } = string.Empty;

    /// <summary>
    /// Estimated transit days.
    /// </summary>
    public int EstimatedTransitDays { get; set; }

    /// <summary>
    /// Estimated cost for this shipping method.
    /// </summary>
    public decimal? EstimatedCost { get; set; }

    /// <summary>
    /// Distributor ID this option belongs to.
    /// </summary>
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Whether this option is currently available.
    /// </summary>
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Description of the shipping option.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Represents a shipping estimate request.
/// </summary>
public class ShippingEstimateRequest
{
    /// <summary>
    /// Distributor to get estimate from.
    /// </summary>
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Origin warehouse code (if known).
    /// </summary>
    public string? OriginWarehouseCode { get; set; }

    /// <summary>
    /// Destination ZIP code.
    /// </summary>
    public string DestinationZip { get; set; } = string.Empty;

    /// <summary>
    /// List of SKUs and quantities for the estimate.
    /// </summary>
    public List<ShippingEstimateItem> Items { get; set; } = new();

    /// <summary>
    /// Desired shipping speed if any preference.
    /// </summary>
    public string? PreferredShippingMethod { get; set; }
}

/// <summary>
/// Item in a shipping estimate request.
/// </summary>
public class ShippingEstimateItem
{
    /// <summary>
    /// SKU identifier.
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Quantity.
    /// </summary>
    public int Quantity { get; set; }
}

/// <summary>
/// Response for a shipping estimate.
/// </summary>
public class ShippingEstimate
{
    /// <summary>
    /// Distributor ID.
    /// </summary>
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Origin warehouse code.
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// Destination ZIP.
    /// </summary>
    public string DestinationZip { get; set; } = string.Empty;

    /// <summary>
    /// Available shipping options with costs.
    /// </summary>
    public List<ShippingOptionEstimate> Options { get; set; } = new();

    /// <summary>
    /// Estimated days in transit for each option.
    /// </summary>
    public DateTime EstimatedAt { get; set; }
}

/// <summary>
/// Individual shipping option estimate.
/// </summary>
public class ShippingOptionEstimate
{
    /// <summary>
    /// Shipping method code.
    /// </summary>
    public string MethodCode { get; set; } = string.Empty;

    /// <summary>
    /// Shipping method name.
    /// </summary>
    public string MethodName { get; set; } = string.Empty;

    /// <summary>
    /// Carrier name.
    /// </summary>
    public string Carrier { get; set; } = string.Empty;

    /// <summary>
    /// Estimated cost.
    /// </summary>
    public decimal EstimatedCost { get; set; }

    /// <summary>
    /// Estimated transit days.
    /// </summary>
    public int EstimatedTransitDays { get; set; }

    /// <summary>
    /// Estimated delivery date.
    /// </summary>
    public DateTime? EstimatedDeliveryDate { get; set; }
}


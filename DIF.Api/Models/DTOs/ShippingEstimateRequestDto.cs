using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIF.Api.Models.DTOs;

/// <summary>
/// DTO for requesting shipping estimates.
/// </summary>
public class ShippingEstimateRequestDto
{
    /// <summary>
    /// Distributor ID to get estimate from.
    /// </summary>
    [Required]
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Origin warehouse code (optional - will auto-select if not provided).
    /// </summary>
    public string? OriginWarehouseCode { get; set; }

    /// <summary>
    /// Destination ZIP code.
    /// </summary>
    [Required]
    public string DestinationZip { get; set; } = string.Empty;

    /// <summary>
    /// Items to estimate shipping for.
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<ShippingEstimateItemDto> Items { get; set; } = new();

    /// <summary>
    /// Preferred shipping method (optional).
    /// </summary>
    public string? PreferredShippingMethod { get; set; }
}

/// <summary>
/// DTO for an item in a shipping estimate request.
/// </summary>
public class ShippingEstimateItemDto
{
    /// <summary>
    /// SKU identifier.
    /// </summary>
    [Required]
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Quantity.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}


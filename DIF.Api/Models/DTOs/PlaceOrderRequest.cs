using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIF.Api.Models.DTOs;

/// <summary>
/// Request DTO for placing an order with a distributor.
/// Mirrors the S&amp;S API order placement structure.
/// </summary>
public class PlaceOrderRequest
{
    /// <summary>
    /// Distributor ID to place the order with (e.g., "ss", "img", "sanmar").
    /// </summary>
    [Required]
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Shipping destination address (printer location).
    /// </summary>
    [Required]
    public ShippingAddressDto ShippingAddress { get; set; } = new();

    /// <summary>
    /// Shipping method code.
    /// S&amp;S: "1"=Ground, "2"=NextDay, "3"=2ndDay
    /// </summary>
    [Required]
    public string ShippingMethod { get; set; } = "1";

    /// <summary>
    /// Whether to auto-select the best warehouse.
    /// Set to true for split-ship capability.
    /// </summary>
    public bool AutoselectWarehouse { get; set; } = true;

    /// <summary>
    /// Specific warehouse codes to limit selection (optional).
    /// </summary>
    public List<string>? AutoselectWarehouseWarehouses { get; set; }

    /// <summary>
    /// Fresh Prints purchase order number.
    /// </summary>
    [Required]
    public string PoNumber { get; set; } = string.Empty;

    /// <summary>
    /// Email address for order confirmation.
    /// </summary>
    public string? EmailConfirmation { get; set; }

    /// <summary>
    /// Whether this is a test order (will be auto-cancelled).
    /// </summary>
    public bool TestOrder { get; set; }

    /// <summary>
    /// Line items to order.
    /// </summary>
    [Required]
    [MinLength(1, ErrorMessage = "At least one line item is required")]
    public List<OrderLineDto> Lines { get; set; } = new();

    /// <summary>
    /// Payment profile to use (optional).
    /// </summary>
    public PaymentProfileDto? PaymentProfile { get; set; }
}

/// <summary>
/// DTO for an order line item.
/// </summary>
public class OrderLineDto
{
    /// <summary>
    /// SKU identifier (e.g., "B00760003").
    /// </summary>
    [Required]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// Quantity to order.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Qty { get; set; }
}

/// <summary>
/// DTO for payment profile.
/// </summary>
public class PaymentProfileDto
{
    /// <summary>
    /// Payment profile ID.
    /// </summary>
    public string ProfileId { get; set; } = string.Empty;

    /// <summary>
    /// Use default payment method.
    /// </summary>
    public bool UseDefault { get; set; } = true;
}


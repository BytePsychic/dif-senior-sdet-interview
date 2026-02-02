using System;
using System.Collections.Generic;

namespace DIF.Api.Models.Domain;

/// <summary>
/// Represents an order placed with a distributor.
/// Maps to S&amp;S API order placement and response data.
/// </summary>
public class Order
{
    /// <summary>
    /// Internal unique identifier for the order.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// The order ID/reference number assigned by the distributor (e.g., S&amp;S orderNumber).
    /// </summary>
    public string DistributorOrderId { get; set; } = string.Empty;

    /// <summary>
    /// Fresh Prints purchase order number (e.g., FPORDER12345).
    /// </summary>
    public string PoNumber { get; set; } = string.Empty;

    /// <summary>
    /// The distributor ID this order was placed with.
    /// </summary>
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Line items in this order.
    /// </summary>
    public List<OrderLine> Lines { get; set; } = new();

    /// <summary>
    /// Shipping destination address (printer location).
    /// </summary>
    public ShippingAddress ShippingAddress { get; set; } = new();

    /// <summary>
    /// Shipping method code (e.g., "1"=Ground, "2"=NextDay, "3"=2ndDay).
    /// </summary>
    public string ShippingMethod { get; set; } = string.Empty;

    /// <summary>
    /// Warehouse abbreviation code where the order ships from.
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// Warehouse company name.
    /// </summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp when the order was placed.
    /// </summary>
    public DateTime OrderTimestamp { get; set; }

    /// <summary>
    /// Expected delivery date from distributor.
    /// </summary>
    public DateTime? ExpectedDeliveryDate { get; set; }

    /// <summary>
    /// Cost breakdown for this order.
    /// </summary>
    public OrderCosts Costs { get; set; } = new();

    /// <summary>
    /// Current order status (e.g., "Placed", "Processing", "Shipped", "Delivered").
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Whether split-ship is enabled (always true for S&amp;S via autoselectWarehouse).
    /// </summary>
    public bool SplitShipEnabled { get; set; } = true;

    /// <summary>
    /// Email address for order confirmation.
    /// </summary>
    public string EmailConfirmation { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if this is a test order (created and auto-cancelled).
    /// </summary>
    public bool IsTestOrder { get; set; }

    /// <summary>
    /// Shipping carrier name (e.g., "UPS").
    /// </summary>
    public string ShippingCarrier { get; set; } = string.Empty;

    /// <summary>
    /// Total number of boxes in the shipment.
    /// </summary>
    public int TotalBoxes { get; set; }

    /// <summary>
    /// Total weight of the shipment in pounds.
    /// </summary>
    public decimal TotalWeight { get; set; }

    /// <summary>
    /// Internal GUID assigned by distributor.
    /// </summary>
    public string InternalGuid { get; set; } = string.Empty;

    /// <summary>
    /// Invoice date when available.
    /// </summary>
    public DateTime? InvoiceDate { get; set; }

    /// <summary>
    /// Ship date when the order was shipped.
    /// </summary>
    public DateTime? ShipDate { get; set; }

    /// <summary>
    /// Tracking number for the shipment.
    /// </summary>
    public string TrackingNumber { get; set; } = string.Empty;

    /// <summary>
    /// Delivery status from tracking.
    /// </summary>
    public string DeliveryStatus { get; set; } = string.Empty;
}

/// <summary>
/// Represents a line item in an order.
/// </summary>
public class OrderLine
{
    /// <summary>
    /// SKU identifier for the product.
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Global Trade Item Number (barcode).
    /// </summary>
    public string Gtin { get; set; } = string.Empty;

    /// <summary>
    /// Quantity ordered.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Quantity actually shipped (may differ if backorder).
    /// </summary>
    public int QuantityShipped { get; set; }

    /// <summary>
    /// Unit price per item.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Line total (Price * Quantity).
    /// </summary>
    public decimal LineTotal => Price * Quantity;

    /// <summary>
    /// Style code for the product.
    /// </summary>
    public string StyleCode { get; set; } = string.Empty;

    /// <summary>
    /// Color name.
    /// </summary>
    public string Color { get; set; } = string.Empty;

    /// <summary>
    /// Size.
    /// </summary>
    public string Size { get; set; } = string.Empty;
}

/// <summary>
/// Represents cost breakdown for an order.
/// </summary>
public class OrderCosts
{
    /// <summary>
    /// Order item ID for cost tracking.
    /// </summary>
    public Guid OrderItemId { get; set; }

    /// <summary>
    /// Merchandise subtotal (blank cost total).
    /// </summary>
    public decimal Subtotal { get; set; }

    /// <summary>
    /// L1 shipping cost (distributor to printer).
    /// Note: S&amp;S only provides total, not itemized per-SKU.
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
    /// Total order cost.
    /// </summary>
    public decimal Total => Subtotal + Shipping + (Tax ?? 0) + (SmallOrderFee ?? 0);

    /// <summary>
    /// Per-SKU blank costs stored as JSON or dictionary.
    /// </summary>
    public Dictionary<string, decimal> BlankCostPerSku { get; set; } = new();

    /// <summary>
    /// Payment method used.
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Warehouse ID from which order shipped.
    /// </summary>
    public string WarehouseId { get; set; } = string.Empty;

    /// <summary>
    /// Warehouse cutoff datetime if known.
    /// </summary>
    public DateTime? CutoffDatetime { get; set; }

    /// <summary>
    /// Additional surcharges as JSON array.
    /// </summary>
    public List<Surcharge> Surcharges { get; set; } = new();
}

/// <summary>
/// Represents a surcharge on an order.
/// </summary>
public class Surcharge
{
    /// <summary>
    /// Surcharge type/name.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Surcharge amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Description of the surcharge.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Represents a shipping address (printer destination).
/// </summary>
public class ShippingAddress
{
    /// <summary>
    /// Customer/printer name.
    /// </summary>
    public string Customer { get; set; } = string.Empty;

    /// <summary>
    /// Street address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Address line 2 (suite, unit, etc.).
    /// </summary>
    public string Address2 { get; set; } = string.Empty;

    /// <summary>
    /// City name.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State code (e.g., "IL").
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// ZIP/postal code.
    /// </summary>
    public string Zip { get; set; } = string.Empty;

    /// <summary>
    /// Country code (default "US").
    /// </summary>
    public string Country { get; set; } = "US";

    /// <summary>
    /// Phone number for delivery.
    /// </summary>
    public string Phone { get; set; } = string.Empty;
}


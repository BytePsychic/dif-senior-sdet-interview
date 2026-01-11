using System;
using System.Collections.Generic;

namespace DIF.Api.Models.Domain;

/// <summary>
/// Represents tracking information for a shipment.
/// Used for L1 (distributor to printer) tracking data from S&amp;S API.
/// </summary>
public class TrackingShipment
{
    /// <summary>
    /// Internal unique identifier for the shipment.
    /// </summary>
    public Guid ShipmentId { get; set; }

    /// <summary>
    /// Reference to the order this shipment belongs to.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Order item ID for granular tracking.
    /// </summary>
    public Guid? OrderItemId { get; set; }

    /// <summary>
    /// Fresh Prints purchase order number.
    /// </summary>
    public string PurchaseOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Tracking number from the carrier (e.g., UPS tracking number).
    /// </summary>
    public string TrackingNumber { get; set; } = string.Empty;

    /// <summary>
    /// Tracking URL for the shipment.
    /// </summary>
    public string TrackingUrl { get; set; } = string.Empty;

    /// <summary>
    /// Carrier name (e.g., "UPS", "FedEx").
    /// </summary>
    public string Carrier { get; set; } = string.Empty;

    /// <summary>
    /// Shipping type/method (e.g., "Ground", "1" for S&amp;S Ground).
    /// </summary>
    public string ShippingType { get; set; } = string.Empty;

    /// <summary>
    /// Shipment leg type: L1 (distributor to printer), L2 (printer to customer), L3 (returns).
    /// </summary>
    public string LegType { get; set; } = "L1";

    /// <summary>
    /// Number of boxes in the shipment.
    /// </summary>
    public int NumBoxes { get; set; }

    /// <summary>
    /// Total weight of the shipment in pounds.
    /// </summary>
    public decimal TotalWeight { get; set; }

    /// <summary>
    /// Date when the shipment was shipped (label created).
    /// </summary>
    public DateTime? ShipDate { get; set; }

    /// <summary>
    /// Estimated delivery date.
    /// </summary>
    public DateTime? EstimatedDelivery { get; set; }

    /// <summary>
    /// Actual delivery date/time.
    /// </summary>
    public DateTime? ActualDeliveryDate { get; set; }

    /// <summary>
    /// Origin warehouse ZIP code.
    /// </summary>
    public string OriginWarehouseZip { get; set; } = string.Empty;

    /// <summary>
    /// Origin warehouse address.
    /// </summary>
    public string OriginWarehouseAddress { get; set; } = string.Empty;

    /// <summary>
    /// Destination printer ZIP code.
    /// </summary>
    public string DestinationPrinterZip { get; set; } = string.Empty;

    /// <summary>
    /// Destination printer full address.
    /// </summary>
    public string DestinationPrinterAddress { get; set; } = string.Empty;

    /// <summary>
    /// Current tracking status (e.g., "In Transit", "Out for Delivery", "Delivered").
    /// S&amp;S deliveryStatus options: Picked Up, Shipped, Shipped - Delivered, Shipped - Exception, 
    /// Shipped - In Transit, Shipped - Out For Delivery, etc.
    /// </summary>
    public string CurrentStatus { get; set; } = string.Empty;

    /// <summary>
    /// History of status changes with timestamps and locations.
    /// </summary>
    public List<StatusHistoryEntry> StatusHistory { get; set; } = new();

    /// <summary>
    /// Whether delivery has been confirmed.
    /// </summary>
    public bool DeliveryConfirmed { get; set; }

    /// <summary>
    /// Flag indicating potential misshipment (box count or weight mismatch).
    /// </summary>
    public bool MisshipmentFlag { get; set; }

    /// <summary>
    /// Number of boxes delivered (for misshipment detection).
    /// </summary>
    public int? BoxesDelivered { get; set; }

    /// <summary>
    /// Weight of delivered shipment (for misshipment detection).
    /// </summary>
    public decimal? WeightDelivered { get; set; }

    /// <summary>
    /// Signature on delivery if available.
    /// </summary>
    public string DeliverySignature { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of the last status update.
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// Current location during transit if available.
    /// </summary>
    public string CurrentLocation { get; set; } = string.Empty;

    /// <summary>
    /// Distributor ID this tracking belongs to.
    /// </summary>
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Distributor order ID for reference.
    /// </summary>
    public string DistributorOrderId { get; set; } = string.Empty;
}

/// <summary>
/// Represents a single entry in the tracking status history.
/// </summary>
public class StatusHistoryEntry
{
    /// <summary>
    /// Status at this point in time.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of this status.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Location where this status was recorded.
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Additional description or notes.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Represents a delivery confirmation record.
/// </summary>
public class DeliveryConfirmation
{
    /// <summary>
    /// Shipment ID this confirmation belongs to.
    /// </summary>
    public Guid ShipmentId { get; set; }

    /// <summary>
    /// Actual delivery date/time.
    /// </summary>
    public DateTime DeliveryDateTime { get; set; }

    /// <summary>
    /// Number of boxes delivered.
    /// </summary>
    public int BoxesDelivered { get; set; }

    /// <summary>
    /// Weight of delivered shipment.
    /// </summary>
    public decimal WeightDelivered { get; set; }

    /// <summary>
    /// Confirmed delivery address/location.
    /// </summary>
    public string DeliveryLocation { get; set; } = string.Empty;

    /// <summary>
    /// Name of person who signed for delivery.
    /// </summary>
    public string SignedBy { get; set; } = string.Empty;

    /// <summary>
    /// Whether there's a box count mismatch.
    /// </summary>
    public bool BoxCountMismatch { get; set; }

    /// <summary>
    /// Whether there's a weight mismatch.
    /// </summary>
    public bool WeightMismatch { get; set; }

    /// <summary>
    /// Expected boxes from shipment.
    /// </summary>
    public int ExpectedBoxes { get; set; }

    /// <summary>
    /// Expected weight from shipment.
    /// </summary>
    public decimal ExpectedWeight { get; set; }
}


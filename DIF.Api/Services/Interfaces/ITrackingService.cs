using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIF.Api.Models.Domain;

namespace DIF.Api.Services.Interfaces;

/// <summary>
/// Service interface for tracking and shipment operations.
/// Handles L1 tracking from distributors to printers.
/// </summary>
public interface ITrackingService
{
    /// <summary>
    /// Gets tracking information for an order.
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    /// <returns>Tracking shipment information if found.</returns>
    Task<TrackingShipment?> GetTrackingByOrderIdAsync(Guid orderId);

    /// <summary>
    /// Gets tracking information by tracking number.
    /// </summary>
    /// <param name="trackingNumber">Carrier tracking number.</param>
    /// <returns>Tracking shipment information if found.</returns>
    Task<TrackingShipment?> GetTrackingByNumberAsync(string trackingNumber);

    /// <summary>
    /// Gets all tracking records for an order (may have multiple shipments).
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    /// <returns>List of tracking shipments.</returns>
    Task<List<TrackingShipment>> GetAllTrackingForOrderAsync(Guid orderId);

    /// <summary>
    /// Gets all shipments with pending/in-transit status that need updates.
    /// </summary>
    /// <returns>List of pending shipments.</returns>
    Task<List<TrackingShipment>> GetPendingShipmentsAsync();

    /// <summary>
    /// Updates tracking status for a specific tracking number.
    /// Simulates polling the distributor/carrier API.
    /// </summary>
    /// <param name="trackingNumber">Tracking number to update.</param>
    /// <returns>Updated tracking shipment.</returns>
    Task<TrackingShipment?> UpdateTrackingStatusAsync(string trackingNumber);

    /// <summary>
    /// Batch updates tracking status for all pending shipments.
    /// Used for 2x daily status updates.
    /// </summary>
    /// <returns>Number of shipments updated.</returns>
    Task<int> BatchUpdateTrackingStatusAsync();

    /// <summary>
    /// Processes delivery confirmation for a shipment.
    /// Captures delivery details and checks for misshipments.
    /// </summary>
    /// <param name="trackingNumber">Tracking number.</param>
    /// <returns>Delivery confirmation details.</returns>
    Task<DeliveryConfirmation?> ProcessDeliveryConfirmationAsync(string trackingNumber);

    /// <summary>
    /// Gets delivery confirmation for a shipment.
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    /// <returns>Delivery confirmation if delivered.</returns>
    Task<DeliveryConfirmation?> GetDeliveryConfirmationAsync(Guid orderId);

    /// <summary>
    /// Posts tracking to V3 system when shipment is created.
    /// </summary>
    /// <param name="shipment">Tracking shipment to post.</param>
    /// <returns>True if posted successfully.</returns>
    Task<bool> PostTrackingToV3Async(TrackingShipment shipment);

    /// <summary>
    /// Gets shipments with potential misshipment flags.
    /// </summary>
    /// <returns>List of shipments with misshipment issues.</returns>
    Task<List<TrackingShipment>> GetMisshipmentAlertsAsync();
}


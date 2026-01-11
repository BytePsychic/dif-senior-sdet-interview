using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DIF.Api.Models.Domain;
using DIF.Api.Models.DTOs;
using DIF.Api.Models.Responses;
using DIF.Api.Services.Interfaces;

namespace DIF.Api.Controllers;

/// <summary>
/// Controller for tracking and shipment operations.
/// Handles L1 tracking from distributors to printers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TrackingController : ControllerBase
{
    private readonly ITrackingService _trackingService;

    public TrackingController(ITrackingService trackingService)
    {
        _trackingService = trackingService;
    }

    /// <summary>
    /// Gets tracking information for an order.
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    /// <returns>Tracking information.</returns>
    /// <response code="200">Tracking found.</response>
    /// <response code="404">Tracking not found.</response>
    [HttpGet("{orderId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TrackingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TrackingResponse>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TrackingResponse>>> GetTrackingByOrderId(Guid orderId)
    {
        var shipment = await _trackingService.GetTrackingByOrderIdAsync(orderId);
        
        if (shipment == null)
        {
            return NotFound(ApiResponse<TrackingResponse>.Fail($"Tracking for order {orderId} not found"));
        }

        var response = MapToTrackingResponse(shipment);
        return Ok(ApiResponse<TrackingResponse>.Ok(response));
    }

    /// <summary>
    /// Gets tracking information by tracking number.
    /// </summary>
    /// <param name="trackingNumber">Carrier tracking number.</param>
    /// <returns>Tracking information.</returns>
    [HttpGet("shipment/{trackingNumber}")]
    [ProducesResponseType(typeof(ApiResponse<TrackingShipment>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TrackingShipment>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TrackingShipment>>> GetTrackingByNumber(string trackingNumber)
    {
        var shipment = await _trackingService.GetTrackingByNumberAsync(trackingNumber);
        
        if (shipment == null)
        {
            return NotFound(ApiResponse<TrackingShipment>.Fail($"Tracking for {trackingNumber} not found"));
        }

        return Ok(ApiResponse<TrackingShipment>.Ok(shipment));
    }

    /// <summary>
    /// Gets all tracking records for an order (may have multiple shipments).
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    /// <returns>List of tracking shipments.</returns>
    [HttpGet("{orderId:guid}/all")]
    [ProducesResponseType(typeof(ApiResponse<System.Collections.Generic.List<TrackingShipment>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<System.Collections.Generic.List<TrackingShipment>>>> GetAllTrackingForOrder(Guid orderId)
    {
        var shipments = await _trackingService.GetAllTrackingForOrderAsync(orderId);
        return Ok(ApiResponse<System.Collections.Generic.List<TrackingShipment>>.Ok(shipments));
    }

    /// <summary>
    /// Triggers tracking status update for pending shipments.
    /// Simulates the 2x daily polling of distributor/carrier APIs.
    /// </summary>
    /// <param name="request">Optional filter for specific orders or tracking numbers.</param>
    /// <returns>Number of shipments updated.</returns>
    [HttpPost("update")]
    [ProducesResponseType(typeof(ApiResponse<TrackingUpdateResult>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<TrackingUpdateResult>>> TriggerTrackingUpdate([FromBody] TrackingUpdateRequestDto? request = null)
    {
        int updatedCount;

        if (request?.TrackingNumbers != null && request.TrackingNumbers.Count > 0)
        {
            // Update specific tracking numbers
            updatedCount = 0;
            foreach (var trackingNumber in request.TrackingNumbers)
            {
                var result = await _trackingService.UpdateTrackingStatusAsync(trackingNumber);
                if (result != null) updatedCount++;
            }
        }
        else
        {
            // Batch update all pending shipments
            updatedCount = await _trackingService.BatchUpdateTrackingStatusAsync();
        }

        var result1 = new TrackingUpdateResult
        {
            ShipmentsUpdated = updatedCount,
            UpdatedAt = DateTime.UtcNow
        };

        return Ok(ApiResponse<TrackingUpdateResult>.Ok(result1, $"Updated {updatedCount} shipments"));
    }

    /// <summary>
    /// Gets delivery confirmation for an order.
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    /// <returns>Delivery confirmation details.</returns>
    [HttpGet("{orderId:guid}/delivery-confirmation")]
    [ProducesResponseType(typeof(ApiResponse<DeliveryConfirmation>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<DeliveryConfirmation>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<DeliveryConfirmation>>> GetDeliveryConfirmation(Guid orderId)
    {
        var confirmation = await _trackingService.GetDeliveryConfirmationAsync(orderId);
        
        if (confirmation == null)
        {
            return NotFound(ApiResponse<DeliveryConfirmation>.Fail($"Delivery confirmation for order {orderId} not found"));
        }

        return Ok(ApiResponse<DeliveryConfirmation>.Ok(confirmation));
    }

    /// <summary>
    /// Gets all pending shipments that haven't been delivered yet.
    /// </summary>
    /// <returns>List of pending shipments.</returns>
    [HttpGet("pending")]
    [ProducesResponseType(typeof(ApiResponse<System.Collections.Generic.List<TrackingShipment>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<System.Collections.Generic.List<TrackingShipment>>>> GetPendingShipments()
    {
        var shipments = await _trackingService.GetPendingShipmentsAsync();
        return Ok(ApiResponse<System.Collections.Generic.List<TrackingShipment>>.Ok(shipments, $"Found {shipments.Count} pending shipments"));
    }

    /// <summary>
    /// Gets shipments flagged for potential misshipment issues.
    /// </summary>
    /// <returns>List of shipments with misshipment flags.</returns>
    [HttpGet("misshipments")]
    [ProducesResponseType(typeof(ApiResponse<System.Collections.Generic.List<TrackingShipment>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<System.Collections.Generic.List<TrackingShipment>>>> GetMisshipmentAlerts()
    {
        var shipments = await _trackingService.GetMisshipmentAlertsAsync();
        return Ok(ApiResponse<System.Collections.Generic.List<TrackingShipment>>.Ok(shipments, $"Found {shipments.Count} misshipment alerts"));
    }

    private static TrackingResponse MapToTrackingResponse(TrackingShipment shipment)
    {
        return new TrackingResponse
        {
            ShipmentId = shipment.ShipmentId,
            OrderId = shipment.OrderId,
            TrackingNumber = shipment.TrackingNumber,
            TrackingUrl = shipment.TrackingUrl,
            Carrier = shipment.Carrier,
            CurrentStatus = shipment.CurrentStatus,
            ShipDate = shipment.ShipDate,
            EstimatedDelivery = shipment.EstimatedDelivery,
            ActualDeliveryDate = shipment.ActualDeliveryDate,
            NumBoxes = shipment.NumBoxes,
            TotalWeight = shipment.TotalWeight,
            OriginWarehouse = shipment.OriginWarehouseZip,
            Destination = shipment.DestinationPrinterZip,
            LegType = shipment.LegType,
            LastUpdated = shipment.LastUpdated
        };
    }
}

/// <summary>
/// Result of a tracking update operation.
/// </summary>
public class TrackingUpdateResult
{
    /// <summary>
    /// Number of shipments that were updated.
    /// </summary>
    public int ShipmentsUpdated { get; set; }

    /// <summary>
    /// Timestamp of the update.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}


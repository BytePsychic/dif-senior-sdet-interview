using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIF.Api.Mocks;
using DIF.Api.Models.Domain;
using DIF.Api.Services.Interfaces;

namespace DIF.Api.Services.Implementations;

/// <summary>
/// Mock implementation of ITrackingService for testing.
/// Returns realistic tracking data without actual API calls.
/// </summary>
public class MockTrackingService : ITrackingService
{
    private readonly List<TrackingShipment> _shipments;
    private readonly Dictionary<Guid, DeliveryConfirmation> _deliveryConfirmations;
    private static readonly Random _random = new();

    public MockTrackingService()
    {
        _shipments = MockDataFactory.CreateSampleTracking(20);
        _deliveryConfirmations = new Dictionary<Guid, DeliveryConfirmation>();
        
        // Create delivery confirmations for delivered shipments
        foreach (var shipment in _shipments.Where(s => s.CurrentStatus == "Delivered"))
        {
            _deliveryConfirmations[shipment.OrderId] = MockDataFactory.CreateDeliveryConfirmation(shipment);
        }
    }

    /// <inheritdoc />
    public Task<TrackingShipment?> GetTrackingByOrderIdAsync(Guid orderId)
    {
        var shipment = _shipments.FirstOrDefault(s => s.OrderId == orderId);
        return Task.FromResult(shipment);
    }

    /// <inheritdoc />
    public Task<TrackingShipment?> GetTrackingByNumberAsync(string trackingNumber)
    {
        var shipment = _shipments.FirstOrDefault(s => 
            s.TrackingNumber.Equals(trackingNumber, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(shipment);
    }

    /// <inheritdoc />
    public Task<List<TrackingShipment>> GetAllTrackingForOrderAsync(Guid orderId)
    {
        var shipments = _shipments.Where(s => s.OrderId == orderId).ToList();
        return Task.FromResult(shipments);
    }

    /// <inheritdoc />
    public Task<List<TrackingShipment>> GetPendingShipmentsAsync()
    {
        var pendingStatuses = new[] { "Label Created", "Picked Up", "In Transit", "Out for Delivery" };
        var pending = _shipments
            .Where(s => pendingStatuses.Contains(s.CurrentStatus))
            .ToList();
        return Task.FromResult(pending);
    }

    /// <inheritdoc />
    public Task<TrackingShipment?> UpdateTrackingStatusAsync(string trackingNumber)
    {
        var shipment = _shipments.FirstOrDefault(s => 
            s.TrackingNumber.Equals(trackingNumber, StringComparison.OrdinalIgnoreCase));

        if (shipment == null)
        {
            return Task.FromResult<TrackingShipment?>(null);
        }

        // Simulate status progression
        var newStatus = shipment.CurrentStatus switch
        {
            "Label Created" => "Picked Up",
            "Picked Up" => "In Transit",
            "In Transit" => _random.Next(0, 3) == 0 ? "Out for Delivery" : "In Transit",
            "Out for Delivery" => "Delivered",
            _ => shipment.CurrentStatus
        };

        if (newStatus != shipment.CurrentStatus)
        {
            shipment.CurrentStatus = newStatus;
            shipment.LastUpdated = DateTime.UtcNow;
            shipment.StatusHistory.Add(new StatusHistoryEntry
            {
                Status = newStatus,
                Timestamp = DateTime.UtcNow,
                Location = GetRandomLocation(),
                Description = GetStatusDescription(newStatus)
            });

            if (newStatus == "Delivered")
            {
                shipment.ActualDeliveryDate = DateTime.UtcNow;
                shipment.DeliveryConfirmed = true;
                _deliveryConfirmations[shipment.OrderId] = MockDataFactory.CreateDeliveryConfirmation(shipment);
            }
        }

        return Task.FromResult<TrackingShipment?>(shipment);
    }

    /// <inheritdoc />
    public Task<int> BatchUpdateTrackingStatusAsync()
    {
        var pending = _shipments.Where(s => s.CurrentStatus != "Delivered").ToList();
        var updatedCount = 0;

        foreach (var shipment in pending)
        {
            // 30% chance to update each shipment
            if (_random.Next(0, 10) < 3)
            {
                var oldStatus = shipment.CurrentStatus;
                UpdateTrackingStatusAsync(shipment.TrackingNumber).Wait();
                if (shipment.CurrentStatus != oldStatus)
                {
                    updatedCount++;
                }
            }
        }

        return Task.FromResult(updatedCount);
    }

    /// <inheritdoc />
    public Task<DeliveryConfirmation?> ProcessDeliveryConfirmationAsync(string trackingNumber)
    {
        var shipment = _shipments.FirstOrDefault(s => 
            s.TrackingNumber.Equals(trackingNumber, StringComparison.OrdinalIgnoreCase));

        if (shipment == null || shipment.CurrentStatus != "Delivered")
        {
            return Task.FromResult<DeliveryConfirmation?>(null);
        }

        if (!_deliveryConfirmations.ContainsKey(shipment.OrderId))
        {
            _deliveryConfirmations[shipment.OrderId] = MockDataFactory.CreateDeliveryConfirmation(shipment);
        }

        return Task.FromResult<DeliveryConfirmation?>(_deliveryConfirmations[shipment.OrderId]);
    }

    /// <inheritdoc />
    public Task<DeliveryConfirmation?> GetDeliveryConfirmationAsync(Guid orderId)
    {
        _deliveryConfirmations.TryGetValue(orderId, out var confirmation);
        return Task.FromResult(confirmation);
    }

    /// <inheritdoc />
    public Task<bool> PostTrackingToV3Async(TrackingShipment shipment)
    {
        // Simulate posting to V3 - always succeeds in mock
        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<List<TrackingShipment>> GetMisshipmentAlertsAsync()
    {
        var misshipments = _shipments.Where(s => s.MisshipmentFlag).ToList();
        return Task.FromResult(misshipments);
    }

    private static string GetRandomLocation()
    {
        var locations = new[]
        {
            "Bolingbrook, IL",
            "Hodgkins, IL", 
            "Chicago, IL",
            "Indianapolis, IN",
            "Columbus, OH",
            "Louisville, KY"
        };
        return locations[_random.Next(locations.Length)];
    }

    private static string GetStatusDescription(string status)
    {
        return status switch
        {
            "Picked Up" => "Package picked up by carrier",
            "In Transit" => "In transit to destination",
            "Out for Delivery" => "Out for delivery",
            "Delivered" => "Delivered - Left at front door",
            _ => status
        };
    }
}


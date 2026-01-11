using System;
using System.Collections.Generic;

namespace DIF.Api.Models.DTOs;

/// <summary>
/// DTO for triggering tracking updates.
/// </summary>
public class TrackingUpdateRequestDto
{
    /// <summary>
    /// Specific order IDs to update tracking for.
    /// If empty, updates all pending shipments.
    /// </summary>
    public List<Guid>? OrderIds { get; set; }

    /// <summary>
    /// Specific tracking numbers to update.
    /// </summary>
    public List<string>? TrackingNumbers { get; set; }

    /// <summary>
    /// Filter by distributor ID.
    /// </summary>
    public string? DistributorId { get; set; }

    /// <summary>
    /// Only update shipments with specific statuses.
    /// </summary>
    public List<string>? StatusFilter { get; set; }
}

/// <summary>
/// DTO for getting tracking by order ID.
/// </summary>
public class TrackingQueryDto
{
    /// <summary>
    /// Order ID to get tracking for.
    /// </summary>
    public Guid? OrderId { get; set; }

    /// <summary>
    /// Tracking number to look up.
    /// </summary>
    public string? TrackingNumber { get; set; }

    /// <summary>
    /// PO number to look up.
    /// </summary>
    public string? PoNumber { get; set; }

    /// <summary>
    /// Distributor order ID.
    /// </summary>
    public string? DistributorOrderId { get; set; }
}


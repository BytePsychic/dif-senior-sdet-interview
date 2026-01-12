package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * Response model for pending shipment.
 * Matches the structure from GET /api/tracking/pending.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class PendingShipment {
    
    private String shipmentId;
    private String orderId;
    private String trackingNumber;
    private String carrier;
    private String currentStatus;
    private String estimatedDelivery;
}

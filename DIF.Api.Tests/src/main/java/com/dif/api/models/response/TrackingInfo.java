package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

/**
 * Response model for basic tracking information.
 * Matches the structure from GET /api/tracking/{orderId}.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class TrackingInfo {
    
    private String shipmentId;
    private String orderId;
    private String trackingNumber;
    private String trackingUrl;
    private String carrier;
    private String currentStatus;
    private String shipDate;
    private String estimatedDelivery;
    private String actualDeliveryDate;
    private int numBoxes;
    private BigDecimal totalWeight;
    private String originWarehouse;
    private String destination;
    private String legType;
    private String lastUpdated;
}

package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * Response model for tracking summary in list endpoints.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class TrackingSummary {
    
    private String shipmentId;
    private String orderId;
    private String trackingNumber;
    private String carrier;
    private String currentStatus;
    private String legType;
    private String estimatedDelivery;
}

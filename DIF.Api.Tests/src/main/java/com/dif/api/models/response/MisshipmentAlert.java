package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

/**
 * Response model for misshipment alert.
 * Matches the structure from GET /api/tracking/misshipments.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class MisshipmentAlert {
    
    private String shipmentId;
    private String orderId;
    private String trackingNumber;
    private String currentStatus;
    private boolean misshipmentFlag;
    private int numBoxes;
    private Integer boxesDelivered;
    private BigDecimal totalWeight;
    private BigDecimal weightDelivered;
}

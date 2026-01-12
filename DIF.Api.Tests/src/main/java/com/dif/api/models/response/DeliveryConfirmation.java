package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

/**
 * Response model for delivery confirmation.
 * Matches the structure from GET /api/tracking/{orderId}/delivery-confirmation.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class DeliveryConfirmation {
    
    private String shipmentId;
    private String deliveryDateTime;
    private int boxesDelivered;
    private BigDecimal weightDelivered;
    private String deliveryLocation;
    private String signedBy;
    private boolean boxCountMismatch;
    private boolean weightMismatch;
    private int expectedBoxes;
    private BigDecimal expectedWeight;
}

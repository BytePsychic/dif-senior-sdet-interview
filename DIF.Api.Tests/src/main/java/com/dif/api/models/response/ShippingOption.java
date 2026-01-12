package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

/**
 * Response model for shipping option data.
 * Matches the shipping option structure from GET /api/distributors/{id}/shipping-options.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class ShippingOption {
    
    private String shippingOptionId;
    private String methodCode;
    private String methodName;
    private String carrier;
    private int estimatedTransitDays;
    private BigDecimal estimatedCost;
    private String distributorId;
    private boolean isAvailable;
    private String description;
}

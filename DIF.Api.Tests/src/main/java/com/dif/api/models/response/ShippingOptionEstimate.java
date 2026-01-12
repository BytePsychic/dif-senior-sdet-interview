package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

/**
 * Response model for individual shipping option estimate.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class ShippingOptionEstimate {
    
    private String methodCode;
    private String methodName;
    private String carrier;
    private BigDecimal estimatedCost;
    private int estimatedTransitDays;
    private String estimatedDeliveryDate;
}

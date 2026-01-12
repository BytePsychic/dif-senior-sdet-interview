package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

/**
 * Response model for shipping estimate data.
 * Matches the structure from POST /api/distributors/{id}/shipping-estimate.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class ShippingEstimate {
    
    private String distributorId;
    private String warehouseCode;
    private String destinationZip;
    private List<ShippingOptionEstimate> options;
    private String estimatedAt;
}

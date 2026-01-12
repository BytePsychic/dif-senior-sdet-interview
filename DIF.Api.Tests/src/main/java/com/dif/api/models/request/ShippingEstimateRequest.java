package com.dif.api.models.request;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

/**
 * Request model for shipping estimate.
 * Used with POST /api/distributors/{id}/shipping-estimate.
 */
@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class ShippingEstimateRequest {
    
    private String distributorId;
    private String originWarehouseCode;
    private String destinationZip;
    private List<ShippingEstimateItem> items;
    private String preferredShippingMethod;
}

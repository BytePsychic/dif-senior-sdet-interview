package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * Response model for warehouse data.
 * Matches the warehouse structure from GET /api/distributors/{id}/warehouses.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class Warehouse {
    
    private String warehouseId;
    private String warehouseCode;
    private String warehouseName;
    private String address;
    private String city;
    private String state;
    private String zip;
    private String country;
    private String cutoffTime;
    private String timezone;
    private String distributorId;
    private boolean isActive;
}

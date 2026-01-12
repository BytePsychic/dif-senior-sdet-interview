package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * Response model for inventory/stock data.
 * Matches the inventory structure from GET /api/products/{sku}/inventory.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class InventoryStock {
    
    private String stockId;
    private String sku;
    private String warehouseCode;
    private String warehouseName;
    private int quantityAvailable;
    private int quantityReserved;
    private int quantityOnBackorder;
    private boolean inStock;
    private String expectedRestockDate;
    private String distributorId;
    private String lastUpdated;
}

package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

/**
 * Response model for product data.
 * Matches the product structure from GET /api/products endpoints.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class Product {
    
    private String productId;
    private String sku;
    private String styleCode;
    private String styleName;
    private String brandName;
    private String gtin;
    private String color;
    private String colorCode;
    private String size;
    private String sizeCode;
    private String imageUrl;
    private BigDecimal blankCost;
    private BigDecimal msrp;
    private String description;
    private String category;
    private BigDecimal weight;
    private String distributorId;
    private boolean isActive;
    private boolean isDiscontinued;
    private String lastUpdated;
}

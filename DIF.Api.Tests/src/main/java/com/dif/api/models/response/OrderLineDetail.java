package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

/**
 * Response model for order line detail.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class OrderLineDetail {
    
    private String sku;
    private String gtin;
    private int quantity;
    private int quantityShipped;
    private BigDecimal price;
    private BigDecimal lineTotal;
    private String styleCode;
    private String color;
    private String size;
}

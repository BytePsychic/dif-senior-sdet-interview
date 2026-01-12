package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * Response model for order placement.
 * Matches the order response structure from POST /api/orders.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class OrderResponse {
    
    private String orderId;
    private String distributorOrderId;
    private String poNumber;
    private String status;
    private String warehouseCode;
    private String warehouseName;
    private String expectedDeliveryDate;
    private String orderTimestamp;
    private OrderCosts costs;
}

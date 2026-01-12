package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

/**
 * Response model for order summary in list endpoint.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class OrderSummary {
    
    private String orderId;
    private String distributorOrderId;
    private String poNumber;
    private String distributorId;
    private String status;
    private String warehouseCode;
    private String warehouseName;
    private String orderTimestamp;
    private String expectedDeliveryDate;
    private String shippingCarrier;
    private String trackingNumber;
    private int totalBoxes;
    private BigDecimal totalWeight;
}

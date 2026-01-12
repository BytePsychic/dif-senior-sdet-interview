package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.util.List;

/**
 * Response model for order details.
 * Matches the structure from GET /api/orders/{orderId}.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class OrderDetail {
    
    private String orderId;
    private String distributorOrderId;
    private String poNumber;
    private String distributorId;
    private List<OrderLineDetail> lines;
    private ShippingAddressResponse shippingAddress;
    private String shippingMethod;
    private String warehouseCode;
    private String warehouseName;
    private String orderTimestamp;
    private String expectedDeliveryDate;
    private String status;
    private boolean splitShipEnabled;
    private String shippingCarrier;
    private int totalBoxes;
    private BigDecimal totalWeight;
    private String trackingNumber;
    private String shipDate;
    private String deliveryStatus;
}

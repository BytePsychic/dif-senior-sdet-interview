package com.dif.api.models.request;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

/**
 * Request model for placing an order.
 * Used with POST /api/orders.
 */
@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class PlaceOrderRequest {
    
    private String distributorId;
    private ShippingAddress shippingAddress;
    private String shippingMethod;
    private boolean autoselectWarehouse;
    private List<String> autoselectWarehouseWarehouses;
    private String poNumber;
    private String emailConfirmation;
    private boolean testOrder;
    private List<OrderLine> lines;
    private PaymentProfile paymentProfile;
}

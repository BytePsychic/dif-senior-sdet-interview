package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.util.List;

/**
 * Response model for detailed tracking information.
 * Matches the structure from GET /api/tracking/shipment/{trackingNumber}.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class TrackingDetail {
    
    private String shipmentId;
    private String orderId;
    private String orderItemId;
    private String purchaseOrderNumber;
    private String trackingNumber;
    private String trackingUrl;
    private String carrier;
    private String shippingType;
    private String legType;
    private int numBoxes;
    private BigDecimal totalWeight;
    private String shipDate;
    private String estimatedDelivery;
    private String actualDeliveryDate;
    private String originWarehouseZip;
    private String originWarehouseAddress;
    private String destinationPrinterZip;
    private String destinationPrinterAddress;
    private String currentStatus;
    private List<TrackingStatusEvent> statusHistory;
    private boolean deliveryConfirmed;
    private boolean misshipmentFlag;
    private Integer boxesDelivered;
    private BigDecimal weightDelivered;
    private String deliverySignature;
    private String lastUpdated;
    private String currentLocation;
    private String distributorId;
    private String distributorOrderId;
}

package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.util.List;
import java.util.Map;

/**
 * Response model for order cost breakdown.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class OrderCosts {
    
    private String orderItemId;
    private BigDecimal subtotal;
    private BigDecimal shipping;
    private BigDecimal tax;
    private BigDecimal smallOrderFee;
    private BigDecimal total;
    private Map<String, BigDecimal> blankCostPerSku;
    private String paymentMethod;
    private String warehouseId;
    private String cutoffDatetime;
    private List<Surcharge> surcharges;
}

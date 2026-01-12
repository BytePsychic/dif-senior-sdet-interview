package com.dif.api.builders;

import com.dif.api.models.request.OrderLine;
import com.dif.api.models.request.PaymentProfile;
import com.dif.api.models.request.PlaceOrderRequest;
import com.dif.api.models.request.ShippingAddress;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * Builder class for creating PlaceOrderRequest objects.
 * Provides a fluent API for constructing order requests.
 */
public class PlaceOrderRequestBuilder {
    
    private String distributorId = "ss";
    private ShippingAddress shippingAddress;
    private String shippingMethod = "1";
    private boolean autoselectWarehouse = true;
    private List<String> autoselectWarehouseWarehouses;
    private String poNumber;
    private String emailConfirmation;
    private boolean testOrder = false;
    private List<OrderLine> lines = new ArrayList<>();
    private PaymentProfile paymentProfile;
    
    private PlaceOrderRequestBuilder() {
        // Private constructor - use builder() to create instance
    }
    
    /**
     * Creates a new builder instance.
     * @return New PlaceOrderRequestBuilder
     */
    public static PlaceOrderRequestBuilder builder() {
        return new PlaceOrderRequestBuilder();
    }
    
    /**
     * Sets the distributor ID.
     * @param distributorId Distributor ID (e.g., "ss", "img", "sanmar")
     * @return This builder
     */
    public PlaceOrderRequestBuilder withDistributorId(String distributorId) {
        this.distributorId = distributorId;
        return this;
    }
    
    /**
     * Sets the shipping address.
     * @param address ShippingAddress object
     * @return This builder
     */
    public PlaceOrderRequestBuilder withShippingAddress(ShippingAddress address) {
        this.shippingAddress = address;
        return this;
    }
    
    /**
     * Sets the shipping method.
     * @param method Shipping method code (e.g., "1"=Ground, "2"=NextDay, "3"=2ndDay)
     * @return This builder
     */
    public PlaceOrderRequestBuilder withShippingMethod(String method) {
        this.shippingMethod = method;
        return this;
    }
    
    /**
     * Sets whether to auto-select warehouse.
     * @param autoselect True to auto-select, false otherwise
     * @return This builder
     */
    public PlaceOrderRequestBuilder withAutoselectWarehouse(boolean autoselect) {
        this.autoselectWarehouse = autoselect;
        return this;
    }
    
    /**
     * Sets the warehouse codes to limit auto-selection.
     * @param warehouseCodes Warehouse codes (e.g., "IL", "CA", "KS")
     * @return This builder
     */
    public PlaceOrderRequestBuilder withWarehouseCodes(String... warehouseCodes) {
        this.autoselectWarehouseWarehouses = Arrays.asList(warehouseCodes);
        return this;
    }
    
    /**
     * Sets the PO number.
     * @param poNumber Purchase order number
     * @return This builder
     */
    public PlaceOrderRequestBuilder withPoNumber(String poNumber) {
        this.poNumber = poNumber;
        return this;
    }
    
    /**
     * Sets the email for order confirmation.
     * @param email Email address
     * @return This builder
     */
    public PlaceOrderRequestBuilder withEmailConfirmation(String email) {
        this.emailConfirmation = email;
        return this;
    }
    
    /**
     * Sets whether this is a test order.
     * @param testOrder True for test order (will be auto-cancelled)
     * @return This builder
     */
    public PlaceOrderRequestBuilder withTestOrder(boolean testOrder) {
        this.testOrder = testOrder;
        return this;
    }
    
    /**
     * Adds a line item to the order.
     * @param sku SKU identifier
     * @param quantity Quantity to order
     * @return This builder
     */
    public PlaceOrderRequestBuilder withLine(String sku, int quantity) {
        this.lines.add(OrderLine.builder()
                .identifier(sku)
                .qty(quantity)
                .build());
        return this;
    }
    
    /**
     * Sets the order lines.
     * @param lines List of order lines
     * @return This builder
     */
    public PlaceOrderRequestBuilder withLines(List<OrderLine> lines) {
        this.lines = lines;
        return this;
    }
    
    /**
     * Sets the payment profile.
     * @param paymentProfile PaymentProfile object
     * @return This builder
     */
    public PlaceOrderRequestBuilder withPaymentProfile(PaymentProfile paymentProfile) {
        this.paymentProfile = paymentProfile;
        return this;
    }
    
    /**
     * Sets the payment profile to use default.
     * @return This builder
     */
    public PlaceOrderRequestBuilder withDefaultPayment() {
        this.paymentProfile = PaymentProfile.builder()
                .profileId("")
                .useDefault(true)
                .build();
        return this;
    }
    
    /**
     * Builds the PlaceOrderRequest.
     * @return Constructed PlaceOrderRequest
     */
    public PlaceOrderRequest build() {
        return PlaceOrderRequest.builder()
                .distributorId(distributorId)
                .shippingAddress(shippingAddress)
                .shippingMethod(shippingMethod)
                .autoselectWarehouse(autoselectWarehouse)
                .autoselectWarehouseWarehouses(autoselectWarehouseWarehouses)
                .poNumber(poNumber)
                .emailConfirmation(emailConfirmation)
                .testOrder(testOrder)
                .lines(lines)
                .paymentProfile(paymentProfile)
                .build();
    }
    
    /**
     * Creates a default shipping address for testing (Chicago).
     * @return Default ShippingAddress
     */
    public static ShippingAddress createDefaultShippingAddress() {
        return ShippingAddress.builder()
                .customer("Test Printer #1")
                .address("123 Main Street")
                .address2("Suite 100")
                .city("Chicago")
                .state("IL")
                .zip("60601")
                .country("US")
                .phone("312-555-1234")
                .build();
    }
    
    /**
     * Creates a California shipping address for testing.
     * @return California ShippingAddress
     */
    public static ShippingAddress createCaliforniaShippingAddress() {
        return ShippingAddress.builder()
                .customer("Test Printer CA")
                .address("456 Hollywood Blvd")
                .city("Los Angeles")
                .state("CA")
                .zip("90028")
                .country("US")
                .phone("323-555-5678")
                .build();
    }
}

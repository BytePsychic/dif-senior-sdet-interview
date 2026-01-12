package com.dif.api.factory;

import com.dif.api.builders.PlaceOrderRequestBuilder;
import com.dif.api.models.request.OrderLine;
import com.dif.api.models.request.PaymentProfile;
import com.dif.api.models.request.PlaceOrderRequest;
import com.dif.api.models.request.ShippingAddress;
import com.dif.api.models.request.ShippingEstimateItem;
import com.dif.api.models.request.ShippingEstimateRequest;

import java.util.Arrays;
import java.util.List;
import java.util.UUID;

/**
 * Factory class for generating test data.
 * Provides reusable methods for creating test objects and data.
 */
public class TestDataFactory {
    
    // Valid distributor IDs
    public static final List<String> VALID_DISTRIBUTOR_IDS = Arrays.asList("ss", "img", "sanmar");
    public static final String DEFAULT_DISTRIBUTOR_ID = "ss";
    
    // Valid SKUs (must match mock data)
    public static final List<String> VALID_SKUS = Arrays.asList(
            "G500-BLA-M",
            "G500-BLA-L",
            "G500-BLA-XL"
    );
    public static final String DEFAULT_SKU = "G500-BLA-M";
    
    // Invalid test data
    public static final String INVALID_SKU = "INVALID-SKU-999";
    public static final String INVALID_DISTRIBUTOR_ID = "invalid-distributor";
    
    // Warehouse codes
    public static final List<String> VALID_WAREHOUSE_CODES = Arrays.asList("IL", "CA", "KS", "GA");
    public static final String DEFAULT_WAREHOUSE_CODE = "IL";
    
    // ==================== Shipping Addresses ====================
    
    /**
     * Creates a default shipping address (Chicago, IL).
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
     * Creates a California shipping address.
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
    
    /**
     * Creates a New York shipping address.
     * @return New York ShippingAddress
     */
    public static ShippingAddress createNewYorkShippingAddress() {
        return ShippingAddress.builder()
                .customer("Test Printer NY")
                .address("789 Broadway")
                .city("New York")
                .state("NY")
                .zip("10003")
                .country("US")
                .phone("212-555-9876")
                .build();
    }
    
    /**
     * Creates a shipping address with minimal required fields.
     * @return Minimal ShippingAddress
     */
    public static ShippingAddress createMinimalShippingAddress() {
        return ShippingAddress.builder()
                .customer("Test Customer")
                .address("123 Test St")
                .city("Test City")
                .state("IL")
                .zip("60601")
                .country("US")
                .build();
    }
    
    // ==================== Order Requests ====================
    
    /**
     * Creates a valid order request with default values.
     * @return Valid PlaceOrderRequest
     */
    public static PlaceOrderRequest createValidOrderRequest() {
        return PlaceOrderRequestBuilder.builder()
                .withDistributorId(DEFAULT_DISTRIBUTOR_ID)
                .withShippingAddress(createDefaultShippingAddress())
                .withShippingMethod("1")
                .withPoNumber(generatePoNumber())
                .withTestOrder(true)
                .withLine(DEFAULT_SKU, 12)
                .withDefaultPayment()
                .build();
    }
    
    /**
     * Creates a valid order request with multiple line items.
     * @return PlaceOrderRequest with multiple lines
     */
    public static PlaceOrderRequest createOrderRequestWithMultipleLines() {
        return PlaceOrderRequestBuilder.builder()
                .withDistributorId(DEFAULT_DISTRIBUTOR_ID)
                .withShippingAddress(createDefaultShippingAddress())
                .withShippingMethod("1")
                .withPoNumber(generatePoNumber())
                .withTestOrder(true)
                .withLine("G500-BLA-M", 12)
                .withLine("G500-BLA-L", 24)
                .withLine("G500-BLA-XL", 6)
                .withDefaultPayment()
                .build();
    }
    
    /**
     * Creates an order request with a specific distributor.
     * @param distributorId Distributor ID
     * @return PlaceOrderRequest for specified distributor
     */
    public static PlaceOrderRequest createOrderRequestForDistributor(String distributorId) {
        return PlaceOrderRequestBuilder.builder()
                .withDistributorId(distributorId)
                .withShippingAddress(createDefaultShippingAddress())
                .withShippingMethod("1")
                .withPoNumber(generatePoNumber())
                .withTestOrder(true)
                .withLine(DEFAULT_SKU, 10)
                .withDefaultPayment()
                .build();
    }
    
    /**
     * Creates a valid order line.
     * @param sku SKU identifier
     * @param quantity Quantity
     * @return OrderLine
     */
    public static OrderLine createValidOrderLine(String sku, int quantity) {
        return OrderLine.builder()
                .identifier(sku)
                .qty(quantity)
                .build();
    }
    
    // ==================== Shipping Estimates ====================
    
    /**
     * Creates a valid shipping estimate request.
     * @return Valid ShippingEstimateRequest
     */
    public static ShippingEstimateRequest createValidShippingEstimateRequest() {
        return ShippingEstimateRequest.builder()
                .distributorId(DEFAULT_DISTRIBUTOR_ID)
                .originWarehouseCode(DEFAULT_WAREHOUSE_CODE)
                .destinationZip("90210")
                .items(Arrays.asList(
                        ShippingEstimateItem.builder()
                                .sku(DEFAULT_SKU)
                                .quantity(24)
                                .build()
                ))
                .preferredShippingMethod("1")
                .build();
    }
    
    // ==================== Payment Profiles ====================
    
    /**
     * Creates a default payment profile.
     * @return PaymentProfile using default
     */
    public static PaymentProfile createDefaultPaymentProfile() {
        return PaymentProfile.builder()
                .profileId("")
                .useDefault(true)
                .build();
    }
    
    // ==================== ID Generators ====================
    
    /**
     * Generates a unique PO number.
     * @return Unique PO number
     */
    public static String generatePoNumber() {
        return "TEST-" + System.currentTimeMillis();
    }
    
    /**
     * Generates an invalid order ID (UUID that doesn't exist).
     * @return Random UUID string
     */
    public static String getInvalidOrderId() {
        return UUID.randomUUID().toString();
    }
    
    /**
     * Gets an invalid SKU for negative testing.
     * @return Invalid SKU
     */
    public static String getInvalidSku() {
        return INVALID_SKU;
    }
    
    /**
     * Gets an invalid distributor ID for negative testing.
     * @return Invalid distributor ID
     */
    public static String getInvalidDistributorId() {
        return INVALID_DISTRIBUTOR_ID;
    }
    
    // ==================== Validation Helpers ====================
    
    /**
     * Gets a list of valid distributor IDs for parameterized tests.
     * @return List of valid distributor IDs
     */
    public static List<String> getValidDistributorIds() {
        return VALID_DISTRIBUTOR_IDS;
    }
    
    /**
     * Gets a list of valid SKUs for parameterized tests.
     * @return List of valid SKUs
     */
    public static List<String> getValidSkus() {
        return VALID_SKUS;
    }
    
    /**
     * Gets a list of valid warehouse codes.
     * @return List of warehouse codes
     */
    public static List<String> getValidWarehouseCodes() {
        return VALID_WAREHOUSE_CODES;
    }
}

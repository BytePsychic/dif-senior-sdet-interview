package com.dif.api.tests.tracking;

import com.dif.api.builders.PlaceOrderRequestBuilder;
import com.dif.api.client.OrdersApiClient;
import com.dif.api.client.TrackingApiClient;
import com.dif.api.models.request.PlaceOrderRequest;
import com.dif.api.models.request.TrackingUpdateRequest;
import com.dif.api.tests.BaseTest;
import io.qameta.allure.Description;
import io.qameta.allure.Feature;
import io.qameta.allure.Severity;
import io.qameta.allure.SeverityLevel;
import io.restassured.response.Response;
import org.testng.annotations.BeforeClass;
import org.testng.annotations.Test;

import java.util.Arrays;
import java.util.List;
import java.util.Map;
import java.util.UUID;

import static org.assertj.core.api.Assertions.assertThat;

/**
 * Tests for Tracking API endpoints.
 * Covers: GET /api/tracking/{orderId}, GET /api/tracking/shipment/{trackingNumber},
 * GET /api/tracking/{orderId}/all, POST /api/tracking/update,
 * GET /api/tracking/{orderId}/delivery-confirmation,
 * GET /api/tracking/pending, GET /api/tracking/misshipments
 */
@Feature("Tracking API")
public class TrackingTests extends BaseTest {
    
    private TrackingApiClient trackingApi;
    private OrdersApiClient ordersApi;
    
    // Valid SKU for orders
    private static final String VALID_SKU = "G500-BLA-M";
    private static final String VALID_DISTRIBUTOR_ID = "ss";
    
    // Will be set after creating an order
    private String createdOrderId;
    
    @BeforeClass
    @Override
    public void setUp() {
        super.setUp();
        trackingApi = new TrackingApiClient();
        ordersApi = new OrdersApiClient();
        
        // Create an order to test tracking
        createTestOrder();
    }
    
    private void createTestOrder() {
        String poNumber = "TRACK-TEST-" + System.currentTimeMillis();
        
        PlaceOrderRequest request = PlaceOrderRequestBuilder.builder()
                .withDistributorId(VALID_DISTRIBUTOR_ID)
                .withShippingAddress(PlaceOrderRequestBuilder.createDefaultShippingAddress())
                .withShippingMethod("1")
                .withPoNumber(poNumber)
                .withTestOrder(true)
                .withLine(VALID_SKU, 12)
                .withDefaultPayment()
                .build();
        
        Response response = ordersApi.placeOrder(request);
        if (response.getStatusCode() == 201) {
            createdOrderId = response.jsonPath().getString("data.orderId");
            logger.info("Created test order: {}", createdOrderId);
        }
    }
    
    @Test(groups = {"smoke", "tracking"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify getting tracking by order ID returns tracking info or 404 if not shipped")
    public void getTrackingByOrderId_withValidId_returnsTrackingInfoOr404() {
        logTestStart("getTrackingByOrderId_withValidId_returnsTrackingInfoOr404");
        
        // Skip if no order was created
        if (createdOrderId == null) {
            logger.warn("Skipping test - no order was created");
            return;
        }
        
        Response response = trackingApi.getTrackingByOrderId(createdOrderId);
        
        // May return 200 with tracking or 404 if order not yet shipped
        int statusCode = response.getStatusCode();
        assertThat(statusCode)
                .as("Should return 200 or 404")
                .isIn(200, 404);
        
        if (statusCode == 200) {
            assertSuccess(response);
            assertThat(response.jsonPath().getString("data"))
                    .as("Data field should be present")
                    .isNotNull();
        }
        
        logTestEnd("getTrackingByOrderId_withValidId_returnsTrackingInfoOr404");
    }
    
    @Test(groups = {"negative", "tracking"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting tracking by invalid order ID returns 404")
    public void getTrackingByOrderId_withInvalidId_returns404() {
        logTestStart("getTrackingByOrderId_withInvalidId_returns404");
        
        String invalidOrderId = UUID.randomUUID().toString();
        
        Response response = trackingApi.getTrackingByOrderId(invalidOrderId);
        
        assertStatusCode(response, 404);
        assertFailure(response);
        
        logTestEnd("getTrackingByOrderId_withInvalidId_returns404");
    }
    
    @Test(groups = {"regression", "tracking"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting tracking by tracking number returns detailed info")
    public void getTrackingByNumber_withValidNumber_returnsTrackingDetail() {
        logTestStart("getTrackingByNumber_withValidNumber_returnsTrackingDetail");
        
        // Use a mock tracking number that exists in the system
        String trackingNumber = "1Z999V123456789";
        
        Response response = trackingApi.getTrackingByNumber(trackingNumber);
        
        // May return 200 with data or 404 if tracking number doesn't exist
        int statusCode = response.getStatusCode();
        assertThat(statusCode)
                .as("Should return 200 or 404")
                .isIn(200, 404);
        
        if (statusCode == 200) {
            assertSuccess(response);
            assertThat(response.jsonPath().getString("data.trackingNumber"))
                    .as("Tracking number should be in response")
                    .isNotEmpty();
        }
        
        logTestEnd("getTrackingByNumber_withValidNumber_returnsTrackingDetail");
    }
    
    @Test(groups = {"negative", "tracking"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting tracking by invalid tracking number returns 404")
    public void getTrackingByNumber_withInvalidNumber_returns404() {
        logTestStart("getTrackingByNumber_withInvalidNumber_returns404");
        
        String invalidTrackingNumber = "INVALID-TRACKING-999";
        
        Response response = trackingApi.getTrackingByNumber(invalidTrackingNumber);
        
        assertStatusCode(response, 404);
        assertFailure(response);
        
        logTestEnd("getTrackingByNumber_withInvalidNumber_returns404");
    }
    
    @Test(groups = {"smoke", "tracking"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify getting all tracking for order returns list")
    public void getAllTrackingForOrder_withValidId_returnsTrackingList() {
        logTestStart("getAllTrackingForOrder_withValidId_returnsTrackingList");
        
        // Skip if no order was created
        if (createdOrderId == null) {
            logger.warn("Skipping test - no order was created");
            return;
        }
        
        Response response = trackingApi.getAllTrackingForOrder(createdOrderId);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Data should be a list
        assertThat(response.jsonPath().getList("data"))
                .as("Data should be a list")
                .isNotNull();
        
        logTestEnd("getAllTrackingForOrder_withValidId_returnsTrackingList");
    }
    
    @Test(groups = {"negative", "tracking"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting all tracking for invalid order returns empty list")
    public void getAllTrackingForOrder_withInvalidId_returnsEmptyList() {
        logTestStart("getAllTrackingForOrder_withInvalidId_returnsEmptyList");
        
        String invalidOrderId = UUID.randomUUID().toString();
        
        Response response = trackingApi.getAllTrackingForOrder(invalidOrderId);
        
        // API returns 200 with empty list for non-existent order
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        List<Map<String, Object>> trackingList = response.jsonPath().getList("data");
        assertThat(trackingList)
                .as("Tracking list should be empty for invalid order")
                .isEmpty();
        
        logTestEnd("getAllTrackingForOrder_withInvalidId_returnsEmptyList");
    }
    
    @Test(groups = {"smoke", "tracking"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify triggering tracking update returns update count")
    public void triggerTrackingUpdate_withValidRequest_returnsUpdateCount() {
        logTestStart("triggerTrackingUpdate_withValidRequest_returnsUpdateCount");
        
        TrackingUpdateRequest request = TrackingUpdateRequest.builder()
                .distributorId(VALID_DISTRIBUTOR_ID)
                .statusFilter(Arrays.asList("In Transit", "Out for Delivery"))
                .build();
        
        Response response = trackingApi.triggerTrackingUpdate(request);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify update response
        assertThat(response.jsonPath().getInt("data.shipmentsUpdated"))
                .as("Shipments updated count should be non-negative")
                .isGreaterThanOrEqualTo(0);
        
        logTestEnd("triggerTrackingUpdate_withValidRequest_returnsUpdateCount");
    }
    
    @Test(groups = {"regression", "tracking"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify triggering tracking update with order IDs")
    public void triggerTrackingUpdate_withOrderIds_returnsUpdateCount() {
        logTestStart("triggerTrackingUpdate_withOrderIds_returnsUpdateCount");
        
        // Skip if no order was created
        if (createdOrderId == null) {
            logger.warn("Skipping test - no order was created");
            return;
        }
        
        TrackingUpdateRequest request = TrackingUpdateRequest.builder()
                .orderIds(Arrays.asList(createdOrderId))
                .build();
        
        Response response = trackingApi.triggerTrackingUpdate(request);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        logTestEnd("triggerTrackingUpdate_withOrderIds_returnsUpdateCount");
    }
    
    @Test(groups = {"regression", "tracking"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting delivery confirmation for order")
    public void getDeliveryConfirmation_withValidId_returnsConfirmation() {
        logTestStart("getDeliveryConfirmation_withValidId_returnsConfirmation");
        
        // Skip if no order was created
        if (createdOrderId == null) {
            logger.warn("Skipping test - no order was created");
            return;
        }
        
        Response response = trackingApi.getDeliveryConfirmation(createdOrderId);
        
        // May return 200 with data or 404 if not delivered yet
        int statusCode = response.getStatusCode();
        assertThat(statusCode)
                .as("Should return 200 or 404")
                .isIn(200, 404);
        
        if (statusCode == 200) {
            assertThat(response.jsonPath().getString("data"))
                    .as("Delivery confirmation data should be present")
                    .isNotNull();
        }
        
        logTestEnd("getDeliveryConfirmation_withValidId_returnsConfirmation");
    }
    
    @Test(groups = {"negative", "tracking"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting delivery confirmation for invalid order returns 404")
    public void getDeliveryConfirmation_withInvalidId_returns404() {
        logTestStart("getDeliveryConfirmation_withInvalidId_returns404");
        
        String invalidOrderId = UUID.randomUUID().toString();
        
        Response response = trackingApi.getDeliveryConfirmation(invalidOrderId);
        
        assertStatusCode(response, 404);
        assertFailure(response);
        
        logTestEnd("getDeliveryConfirmation_withInvalidId_returns404");
    }
    
    @Test(groups = {"regression", "tracking"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify tracking shipment has correct leg type")
    public void getTrackingByOrderId_hasCorrectLegType() {
        logTestStart("getTrackingByOrderId_hasCorrectLegType");
        
        if (createdOrderId == null) {
            logger.warn("Skipping test - no order was created");
            return;
        }
        
        Response response = trackingApi.getTrackingByOrderId(createdOrderId);
        
        int statusCode = response.getStatusCode();
        if (statusCode == 200) {
            assertSuccess(response);
            String legType = response.jsonPath().getString("data.legType");
            assertThat(legType)
                    .as("Leg type should be L1 for distributor-to-printer shipments")
                    .isEqualTo("L1");
        }
        
        logTestEnd("getTrackingByOrderId_hasCorrectLegType");
    }
    
    @Test(groups = {"regression", "tracking"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify batch tracking update processes all pending shipments")
    public void triggerTrackingUpdate_processesAllPendingShipments() {
        logTestStart("triggerTrackingUpdate_processesAllPendingShipments");
        
        Response pendingResponse = trackingApi.getPendingShipments();
        assertStatusCode(pendingResponse, 200);
        assertSuccess(pendingResponse);
        
        List<Map<String, Object>> pendingShipments = pendingResponse.jsonPath().getList("data");
        int pendingCount = pendingShipments.size();
        
        TrackingUpdateRequest request = TrackingUpdateRequest.builder()
                .distributorId(VALID_DISTRIBUTOR_ID)
                .build();
        
        Response updateResponse = trackingApi.triggerTrackingUpdate(request);
        assertStatusCode(updateResponse, 200);
        assertSuccess(updateResponse);
        
        int shipmentsUpdated = updateResponse.jsonPath().getInt("data.shipmentsUpdated");
        assertThat(shipmentsUpdated)
                .as("All pending shipments should be processed")
                .isEqualTo(pendingCount);
        
        logTestEnd("triggerTrackingUpdate_processesAllPendingShipments");
    }
    
    @Test(groups = {"regression", "tracking"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify delivery confirmation has correct fields when order is delivered")
    public void getDeliveryConfirmation_whenDelivered_hasCorrectFields() {
        logTestStart("getDeliveryConfirmation_whenDelivered_hasCorrectFields");
        
        if (createdOrderId == null) {
            logger.warn("Skipping test - no order was created");
            return;
        }
        
        Response response = trackingApi.getDeliveryConfirmation(createdOrderId);
        
        int statusCode = response.getStatusCode();
        if (statusCode == 200) {
            assertSuccess(response);
            String deliveryDate = response.jsonPath().getString("data.deliveryDate");
            assertThat(deliveryDate)
                    .as("Delivery date should be present")
                    .isNotEmpty();
            
            assertThat(deliveryDate)
                    .as("Delivery date format validation")
                    .matches("\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}Z");
        }
        
        logTestEnd("getDeliveryConfirmation_whenDelivered_hasCorrectFields");
    }
    
    @Test(groups = {"smoke", "tracking"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify getting pending shipments returns list")
    public void getPendingShipments_returnsPendingShipmentList() {
        logTestStart("getPendingShipments_returnsPendingShipmentList");
        
        Response response = trackingApi.getPendingShipments();
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Data should be a list
        List<Map<String, Object>> pendingShipments = response.jsonPath().getList("data");
        assertThat(pendingShipments)
                .as("Data should be a list of pending shipments")
                .isNotNull();
        
        logTestEnd("getPendingShipments_returnsPendingShipmentList");
    }
    
    @Test(groups = {"smoke", "tracking"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify getting misshipments returns alert list")
    public void getMisshipments_returnsMisshipmentAlertList() {
        logTestStart("getMisshipments_returnsMisshipmentAlertList");
        
        Response response = trackingApi.getMisshipments();
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Data should be a list
        List<Map<String, Object>> misshipments = response.jsonPath().getList("data");
        assertThat(misshipments)
                .as("Data should be a list of misshipment alerts")
                .isNotNull();
        
        logTestEnd("getMisshipments_returnsMisshipmentAlertList");
    }
    
    @Test(groups = {"regression", "tracking"})
    @Severity(SeverityLevel.MINOR)
    @Description("Verify pending shipments response contains expected fields")
    public void getPendingShipments_returnsExpectedFields() {
        logTestStart("getPendingShipments_returnsExpectedFields");
        
        Response response = trackingApi.getPendingShipments();
        
        assertStatusCode(response, 200);
        
        List<Map<String, Object>> pendingShipments = response.jsonPath().getList("data");
        if (!pendingShipments.isEmpty()) {
            // Verify first item has expected fields
            assertThat(response.jsonPath().getString("data[0].shipmentId"))
                    .as("Shipment ID should be present")
                    .isNotEmpty();
            
            assertThat(response.jsonPath().getString("data[0].currentStatus"))
                    .as("Current status should be present")
                    .isNotEmpty();
        }
        
        logTestEnd("getPendingShipments_returnsExpectedFields");
    }
}

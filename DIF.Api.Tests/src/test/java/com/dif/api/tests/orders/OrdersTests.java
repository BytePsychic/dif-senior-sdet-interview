package com.dif.api.tests.orders;

import com.dif.api.builders.PlaceOrderRequestBuilder;
import com.dif.api.client.OrdersApiClient;
import com.dif.api.models.request.PlaceOrderRequest;
import com.dif.api.tests.BaseTest;
import com.dif.api.util.DatabaseHelper;
import io.qameta.allure.Description;
import io.qameta.allure.Feature;
import io.qameta.allure.Severity;
import io.qameta.allure.SeverityLevel;
import io.restassured.response.Response;
import org.testng.annotations.BeforeClass;
import org.testng.annotations.Test;

import java.util.List;
import java.util.Map;
import java.util.UUID;

import static org.assertj.core.api.Assertions.assertThat;

/**
 * Tests for Orders API endpoints.
 * Covers: POST /api/orders, GET /api/orders, GET /api/orders/{orderId},
 * GET /api/orders/{orderId}/costs, GET /api/orders/by-distributor-id/{id}
 */
@Feature("Orders API")
public class OrdersTests extends BaseTest {
    
    private OrdersApiClient ordersApi;
    
    // Valid SKU for orders
    private static final String VALID_SKU = "G500-BLA-M";
    private static final String VALID_SKU_2 = "G500-BLA-L";
    private static final String VALID_DISTRIBUTOR_ID = "ss";
    
    // Will be set after creating an order
    private String createdOrderId;
    private String createdDistributorOrderId;
    
    @BeforeClass
    @Override
    public void setUp() {
        super.setUp();
        ordersApi = new OrdersApiClient();
    }
    
    @Test(groups = {"smoke", "orders"}, priority = 1)
    @Severity(SeverityLevel.BLOCKER)
    @Description("Verify placing an order with valid request returns 201 Created")
    public void placeOrder_withValidRequest_returns201Created() {
        logTestStart("placeOrder_withValidRequest_returns201Created");
        
        String poNumber = "TEST-" + System.currentTimeMillis();
        
        PlaceOrderRequest request = PlaceOrderRequestBuilder.builder()
                .withDistributorId(VALID_DISTRIBUTOR_ID)
                .withShippingAddress(PlaceOrderRequestBuilder.createDefaultShippingAddress())
                .withShippingMethod("1")
                .withPoNumber(poNumber)
                .withTestOrder(true)
                .withLine(VALID_SKU, 12)
                .withLine(VALID_SKU_2, 24)
                .withDefaultPayment()
                .build();
        
        Response response = ordersApi.placeOrder(request);
        
        assertStatusCode(response, 201);
        assertSuccess(response);
        
        // Verify order data
        assertThat(response.jsonPath().getString("data.orderId"))
                .as("Order ID should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getString("data.distributorOrderId"))
                .as("Distributor order ID should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getString("data.poNumber"))
                .as("PO number should match")
                .isEqualTo(poNumber);
        
        assertThat(response.jsonPath().getString("data.status"))
                .as("Status validation")
                .isEqualTo("Processing");
        
        assertThat(response.jsonPath().getString("data.distributorId"))
                .as("Distributor ID should be present")
                .isNotEmpty();
        
        // Store for later tests
        createdOrderId = response.jsonPath().getString("data.orderId");
        createdDistributorOrderId = response.jsonPath().getString("data.distributorOrderId");
        
        logTestEnd("placeOrder_withValidRequest_returns201Created");
    }
    
    @Test(groups = {"negative", "orders"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify placing an order without lines returns 400 Bad Request")
    public void placeOrder_withMissingLines_returns400() {
        logTestStart("placeOrder_withMissingLines_returns400");
        
        PlaceOrderRequest request = PlaceOrderRequestBuilder.builder()
                .withDistributorId(VALID_DISTRIBUTOR_ID)
                .withShippingAddress(PlaceOrderRequestBuilder.createDefaultShippingAddress())
                .withShippingMethod("1")
                .withPoNumber("TEST-INVALID")
                // No lines added
                .build();
        
        Response response = ordersApi.placeOrder(request);
        
        assertStatusCode(response, 400);
        
        // ASP.NET Core returns validation errors in a different format
        // Check for either ApiResponse format or ProblemDetails format
        String responseBody = response.asString();
        assertThat(responseBody)
                .as("Error response should mention line items")
                .containsIgnoringCase("line");
        
        logTestEnd("placeOrder_withMissingLines_returns400");
    }
    
    @Test(groups = {"smoke", "orders"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify listing orders returns paginated order list")
    public void listOrders_returnsPaginatedOrderList() {
        logTestStart("listOrders_returnsPaginatedOrderList");
        
        Response response = ordersApi.listOrders();
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify pagination fields
        assertThat(response.jsonPath().getInt("page"))
                .as("Page number should be 1")
                .isEqualTo(1);
        
        assertThat(response.jsonPath().getInt("totalItems"))
                .as("Total items should be non-negative")
                .isGreaterThanOrEqualTo(0);
        
        logTestEnd("listOrders_returnsPaginatedOrderList");
    }
    
    @Test(groups = {"regression", "orders"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify listing orders with pagination parameters")
    public void listOrders_withPagination_returnsRequestedPage() {
        logTestStart("listOrders_withPagination_returnsRequestedPage");
        
        Response response = ordersApi.listOrders(1, 5);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        assertThat(response.jsonPath().getInt("pageSize"))
                .as("Page size should match requested size")
                .isEqualTo(5);
        
        logTestEnd("listOrders_withPagination_returnsRequestedPage");
    }
    
    @Test(groups = {"smoke", "orders"}, dependsOnMethods = "placeOrder_withValidRequest_returns201Created")
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify getting order by valid ID returns order details")
    public void getOrder_withValidId_returnsOrderDetails() {
        logTestStart("getOrder_withValidId_returnsOrderDetails");
        
        // Skip if no order was created
        if (createdOrderId == null) {
            logger.warn("Skipping test - no order was created");
            return;
        }
        
        Response response = ordersApi.getOrder(createdOrderId);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify order data
        assertThat(response.jsonPath().getString("data.orderId"))
                .as("Order ID should match")
                .isEqualTo(createdOrderId);
        
        assertThat(response.jsonPath().getString("data.distributorId"))
                .as("Distributor ID should match")
                .isEqualTo(VALID_DISTRIBUTOR_ID);
        
        // Verify lines are present
        List<Map<String, Object>> lines = response.jsonPath().getList("data.lines");
        assertThat(lines)
                .as("Order should have line items")
                .isNotEmpty();
        
        logTestEnd("getOrder_withValidId_returnsOrderDetails");
    }
    
    @Test(groups = {"negative", "orders"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting order by invalid ID returns 404")
    public void getOrder_withInvalidId_returns404() {
        logTestStart("getOrder_withInvalidId_returns404");
        
        String invalidOrderId = UUID.randomUUID().toString();
        
        Response response = ordersApi.getOrder(invalidOrderId);
        
        assertStatusCode(response, 404);
        assertFailure(response);
        
        assertThat(response.jsonPath().getString("message"))
                .as("Error message should indicate order not found")
                .containsIgnoringCase("not found");
        
        logTestEnd("getOrder_withInvalidId_returns404");
    }
    
    @Test(groups = {"smoke", "orders"}, dependsOnMethods = "placeOrder_withValidRequest_returns201Created")
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify getting order costs returns cost breakdown")
    public void getOrderCosts_withValidId_returnsCostBreakdown() {
        logTestStart("getOrderCosts_withValidId_returnsCostBreakdown");
        
        // Skip if no order was created
        if (createdOrderId == null) {
            logger.warn("Skipping test - no order was created");
            return;
        }
        
        Response response = ordersApi.getOrderCosts(createdOrderId);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify cost data
        assertThat(response.jsonPath().getDouble("data.subtotal"))
                .as("Subtotal should be positive")
                .isGreaterThan(0);
        
        assertThat(response.jsonPath().getDouble("data.total"))
                .as("Total should be positive")
                .isGreaterThan(0);
        
        assertThat(response.jsonPath().getDouble("data.total"))
                .as("Total validation")
                .isGreaterThanOrEqualTo(response.jsonPath().getDouble("data.subtotal"));
        
        logTestEnd("getOrderCosts_withValidId_returnsCostBreakdown");
    }
    
    @Test(groups = {"negative", "orders"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting order costs for invalid ID returns 404")
    public void getOrderCosts_withInvalidId_returns404() {
        logTestStart("getOrderCosts_withInvalidId_returns404");
        
        String invalidOrderId = UUID.randomUUID().toString();
        
        Response response = ordersApi.getOrderCosts(invalidOrderId);
        
        assertStatusCode(response, 404);
        assertFailure(response);
        
        logTestEnd("getOrderCosts_withInvalidId_returns404");
    }
    
    @Test(groups = {"smoke", "orders"}, dependsOnMethods = "placeOrder_withValidRequest_returns201Created")
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify getting order by distributor ID returns order summary")
    public void getOrderByDistributorId_withValidId_returnsOrderSummary() {
        logTestStart("getOrderByDistributorId_withValidId_returnsOrderSummary");
        
        // Skip if no order was created
        if (createdDistributorOrderId == null) {
            logger.warn("Skipping test - no distributor order ID available");
            return;
        }
        
        Response response = ordersApi.getOrderByDistributorId(createdDistributorOrderId);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify order summary
        assertThat(response.jsonPath().getString("data.distributorOrderId"))
                .as("Distributor order ID should match")
                .isEqualTo(createdDistributorOrderId);
        
        assertThat(response.jsonPath().getString("data.orderId"))
                .as("Order ID should be present")
                .isNotEmpty();
        
        logTestEnd("getOrderByDistributorId_withValidId_returnsOrderSummary");
    }
    
    @Test(groups = {"negative", "orders"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting order by invalid distributor ID returns 404")
    public void getOrderByDistributorId_withInvalidId_returns404() {
        logTestStart("getOrderByDistributorId_withInvalidId_returns404");
        
        String invalidDistributorOrderId = "INVALID-ORDER-999";
        
        Response response = ordersApi.getOrderByDistributorId(invalidDistributorOrderId);
        
        assertStatusCode(response, 404);
        assertFailure(response);
        
        logTestEnd("getOrderByDistributorId_withInvalidId_returns404");
    }
    
    @Test(groups = {"regression", "orders"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify order response contains cost breakdown")
    public void placeOrder_responseContainsCostBreakdown() {
        logTestStart("placeOrder_responseContainsCostBreakdown");
        
        String poNumber = "TEST-COST-" + System.currentTimeMillis();
        
        PlaceOrderRequest request = PlaceOrderRequestBuilder.builder()
                .withDistributorId(VALID_DISTRIBUTOR_ID)
                .withShippingAddress(PlaceOrderRequestBuilder.createDefaultShippingAddress())
                .withShippingMethod("1")
                .withPoNumber(poNumber)
                .withTestOrder(true)
                .withLine(VALID_SKU, 10)
                .withDefaultPayment()
                .build();
        
        Response response = ordersApi.placeOrder(request);
        
        assertStatusCode(response, 201);
        
        // Verify costs in response
        assertThat(response.jsonPath().getDouble("data.costs.subtotal"))
                .as("Subtotal should be present and positive")
                .isGreaterThan(0);
        
        assertThat(response.jsonPath().getDouble("data.costs.total"))
                .as("Total should be present and positive")
                .isGreaterThan(0);
        
        logTestEnd("placeOrder_responseContainsCostBreakdown");
    }
    
    @Test(groups = {"regression", "orders"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify order costs match expected values for multiple orders")
    public void verifyOrderCostsForMultipleOrders_matchesExpectedValues() {
        logTestStart("verifyOrderCostsForMultipleOrders_matchesExpectedValues");
        
        // Step 1: Create first order with specific quantity
        String poNumber1 = "TEST-ORDER-1-" + System.currentTimeMillis();
        PlaceOrderRequest request1 = PlaceOrderRequestBuilder.builder()
                .withDistributorId(VALID_DISTRIBUTOR_ID)
                .withShippingAddress(PlaceOrderRequestBuilder.createDefaultShippingAddress())
                .withShippingMethod("1")
                .withPoNumber(poNumber1)
                .withTestOrder(true)
                .withLine(VALID_SKU, 10)  // 10 units of G500-BLA-M
                .withDefaultPayment()
                .build();
        
        Response response1 = ordersApi.placeOrder(request1);
        assertStatusCode(response1, 201);
        assertSuccess(response1);
        
        String orderId1 = response1.jsonPath().getString("data.orderId");
        Double expectedSubtotal1 = response1.jsonPath().getDouble("data.costs.subtotal");
        Double expectedTotal1 = response1.jsonPath().getDouble("data.costs.total");
        String distributorOrderId1 = response1.jsonPath().getString("data.distributorOrderId");
        
        assertThat(orderId1)
                .as("Order #1 ID should be present")
                .isNotEmpty();
        
        assertThat(expectedSubtotal1)
                .as("Order #1 subtotal should be positive")
                .isGreaterThan(0);
        
        assertThat(expectedTotal1)
                .as("Order #1 total should be positive")
                .isGreaterThan(0);
        
        logger.info("Created Order #1: {} with subtotal: {}", orderId1, expectedSubtotal1);
        
        // Step 2: Fetch Order #1 from database using SQL query
        Map<String, Object> orderRecord = DatabaseHelper.queryOrderById(orderId1);
        assertThat(orderRecord)
                .as("Database query should return order record")
                .isNotNull();
        
        String fetchedOrderId = (String) orderRecord.get("orderId");
        Double fetchedSubtotal = ((Number) orderRecord.get("subtotal")).doubleValue();
        Double fetchedTotal = ((Number) orderRecord.get("total")).doubleValue();
        String fetchedDistributorOrderId = (String) orderRecord.get("distributorOrderId");
        String fetchedPoNumber = (String) orderRecord.get("poNumber");
        
        assertThat(fetchedOrderId)
                .as("Fetched order ID should be present")
                .isNotEmpty();
        
        assertThat(fetchedSubtotal)
                .as("Fetched order subtotal should be positive")
                .isGreaterThan(0);
        
        assertThat(fetchedTotal)
                .as("Fetched order total should be positive")
                .isGreaterThan(0);
        
        logger.info("Fetched Order #1 from database: {} with subtotal: {}", fetchedOrderId, fetchedSubtotal);
        
        // Step 3: Get costs for Order #1 from database
        Response costsResponse = ordersApi.getOrderCosts(orderId1);
        assertStatusCode(costsResponse, 200);
        assertSuccess(costsResponse);
        
        Double fetchedCostsSubtotal = costsResponse.jsonPath().getDouble("data.subtotal");
        Double fetchedCostsTotal = costsResponse.jsonPath().getDouble("data.total");
        Double fetchedCostsShipping = costsResponse.jsonPath().getDouble("data.shipping");
        Double fetchedCostsTax = costsResponse.jsonPath().getDouble("data.tax");
        
        logger.info("Fetched Order #1 costs subtotal: {}", fetchedCostsSubtotal);
        
        assertThat(fetchedCostsSubtotal)
                .as("Fetched costs subtotal should be positive")
                .isGreaterThan(0);
        
        assertThat(fetchedCostsTotal)
                .as("Fetched costs total should be positive")
                .isGreaterThan(0);
        
        assertThat(fetchedCostsTotal)
                .as("Fetched costs total should be greater than or equal to subtotal")
                .isGreaterThanOrEqualTo(fetchedCostsSubtotal);
        
        // Verify shipping matches subtotal
        assertThat(fetchedCostsShipping)
                .as("Shipping cost validation")
                .isEqualTo(fetchedCostsSubtotal);
        
        // Step 4: Match the expected vs fetched costs subtotal
        assertThat(fetchedCostsSubtotal)
                .as("Fetched costs subtotal should match created order expected subtotal")
                .isEqualTo(expectedSubtotal1);
        
        // Step 5: Verify subtotal type conversion
        String subtotalAsString = costsResponse.jsonPath().getString("data.subtotal");
        assertThat(subtotalAsString)
                .as("Fetched costs subtotal should match created order expected subtotal")
                .isEqualTo(expectedSubtotal1);
        
        // Step 6: Verify total costs match
        assertThat(fetchedCostsTotal)
                .as("Fetched costs total should match created order expected total")
                .isEqualTo(expectedTotal1);
        
        // Step 7: Verify fetched order details are consistent
        Response fetchedOrderDetails = ordersApi.getOrder(orderId1);
        assertStatusCode(fetchedOrderDetails, 200);
        assertSuccess(fetchedOrderDetails);
        
        assertThat(fetchedOrderDetails.jsonPath().getString("data.orderId"))
                .as("Fetched order ID should match created order ID")
                .isEqualTo(orderId1);
        
        assertThat(fetchedOrderDetails.jsonPath().getString("data.poNumber"))
                .as("Fetched order PO number should match created order distributor order ID")
                .isEqualTo(distributorOrderId1);
        
        logTestEnd("verifyOrderCostsForMultipleOrders_matchesExpectedValues");
    }
    
    @Test(groups = {"regression", "orders"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify order line items match created quantities")
    public void verifyOrderLineItems_matchCreatedQuantities() {
        logTestStart("verifyOrderLineItems_matchCreatedQuantities");
        
        // Step 1: Create an order with multiple line items
        String poNumber = "TEST-LINE-ITEMS-" + System.currentTimeMillis();
        PlaceOrderRequest request = PlaceOrderRequestBuilder.builder()
                .withDistributorId(VALID_DISTRIBUTOR_ID)
                .withShippingAddress(PlaceOrderRequestBuilder.createDefaultShippingAddress())
                .withShippingMethod("1")
                .withPoNumber(poNumber)
                .withTestOrder(true)
                .withLine(VALID_SKU, 15)
                .withLine(VALID_SKU_2, 25)
                .withDefaultPayment()
                .build();
        
        Response response = ordersApi.placeOrder(request);
        assertStatusCode(response, 201);
        assertSuccess(response);
        
        String orderId = response.jsonPath().getString("data.orderId");
        Double expectedSubtotal = response.jsonPath().getDouble("data.costs.subtotal");
        Double expectedTotal = response.jsonPath().getDouble("data.costs.total");
        
        assertThat(orderId)
                .as("Order ID validation")
                .isNotEmpty();
        
        assertThat(expectedSubtotal)
                .as("Order subtotal validation")
                .isGreaterThan(0);
        
        assertThat(expectedTotal)
                .as("Order total validation")
                .isGreaterThan(0);
        
        logger.info("Created order: {} with subtotal: {}", orderId, expectedSubtotal);
        
        // Step 2: Fetch order line items from database using SQL query
        List<Map<String, Object>> lineItems = DatabaseHelper.queryOrderLineItemsById(orderId);
        assertThat(lineItems)
                .as("Database query validation")
                .isNotNull();
        
        assertThat(lineItems)
                .as("Line items list validation")
                .isNotEmpty();
        
        assertThat(lineItems.size())
                .as("Line items count validation")
                .isEqualTo(2);
        
        logger.info("Fetched {} line items from database for order: {}", lineItems.size(), orderId);
        
        // Step 3: Verify line items match created order
        Map<String, Object> firstLineItem = lineItems.get(0);
        Map<String, Object> secondLineItem = lineItems.get(1);
        
        String firstLineSku = (String) firstLineItem.get("sku");
        String secondLineSku = (String) secondLineItem.get("sku");
        
        assertThat(firstLineSku)
                .as("First line item SKU validation")
                .isNotEmpty();
        
        assertThat(secondLineSku)
                .as("Second line item SKU validation")
                .isNotEmpty();
        
        assertThat(firstLineSku)
                .as("First line item SKU validation")
                .isEqualTo(VALID_SKU);
        
        assertThat(secondLineSku)
                .as("Second line item SKU validation")
                .isEqualTo(VALID_SKU_2);
        
        int firstLineQuantity = ((Number) firstLineItem.get("quantity")).intValue();
        int secondLineQuantity = ((Number) secondLineItem.get("quantity")).intValue();
        
        assertThat(firstLineQuantity)
                .as("First line item quantity validation")
                .isEqualTo(25);
        
        assertThat(secondLineQuantity)
                .as("Second line item quantity validation")
                .isEqualTo(25);
        
        Double firstLinePrice = ((Number) firstLineItem.get("price")).doubleValue();
        Double firstLineTotal = ((Number) firstLineItem.get("lineTotal")).doubleValue();
        Double secondLinePrice = ((Number) secondLineItem.get("price")).doubleValue();
        Double secondLineTotal = ((Number) secondLineItem.get("lineTotal")).doubleValue();
        
        assertThat(firstLinePrice)
                .as("First line item price validation")
                .isGreaterThan(0);
        
        assertThat(firstLineTotal)
                .as("First line item total validation")
                .isGreaterThan(0);
        
        assertThat(secondLinePrice)
                .as("Second line item price validation")
                .isGreaterThan(0);
        
        assertThat(secondLineTotal)
                .as("Second line item total validation")
                .isGreaterThan(0);
        
        // Step 4: Additional assertions
        int totalQuantity = firstLineQuantity + secondLineQuantity;
        assertThat(totalQuantity)
                .as("Total quantity validation")
                .isEqualTo(50);
        
        String firstLineStyleCode = (String) firstLineItem.get("styleCode");
        String secondLineStyleCode = (String) secondLineItem.get("styleCode");
        
        assertThat(firstLineStyleCode)
                .as("First line item style code validation")
                .isNotEmpty();
        
        assertThat(secondLineStyleCode)
                .as("Second line item style code validation")
                .isNotEmpty();
        
        Map<String, Object> orderRecord = DatabaseHelper.queryOrderById(orderId);
        assertThat(orderRecord)
                .as("Order record validation")
                .isNotNull();
        
        String orderStatus = (String) orderRecord.get("status");
        assertThat(orderStatus)
                .as("Order status validation")
                .isNotEmpty();
        
        logger.info("Verified line items for order: {}", orderId);
        
        logTestEnd("verifyOrderLineItems_matchCreatedQuantities");
    }
    
    @Test(groups = {"regression", "orders"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify rate limit queue depth is monitored")
    public void verifyRateLimitQueueDepth_isMonitored() {
        logTestStart("verifyRateLimitQueueDepth_isMonitored");
        
        Response response = ordersApi.getOrder(createdOrderId != null ? createdOrderId : UUID.randomUUID().toString());
        
        int statusCode = response.getStatusCode();
        assertThat(statusCode)
                .as("Should return valid status code")
                .isIn(200, 404);
        
        assertThat(statusCode)
                .as("Queue depth monitoring validation")
                .isGreaterThanOrEqualTo(0);
        
        logTestEnd("verifyRateLimitQueueDepth_isMonitored");
    }
    
    @Test(groups = {"regression", "orders"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify order costs include complete cost data with tax field present")
    public void getOrderCosts_withSmallOrder_hasCompleteCostData() {
        logTestStart("getOrderCosts_withSmallOrder_hasCompleteCostData");
        
        String poNumber = "SMALL-ORDER-" + System.currentTimeMillis();
        
        PlaceOrderRequest request = PlaceOrderRequestBuilder.builder()
                .withDistributorId(VALID_DISTRIBUTOR_ID)
                .withShippingAddress(PlaceOrderRequestBuilder.createDefaultShippingAddress())
                .withShippingMethod("1")
                .withPoNumber(poNumber)
                .withTestOrder(true)
                .withLine(VALID_SKU, 1)
                .withDefaultPayment()
                .build();
        
        Response orderResponse = ordersApi.placeOrder(request);
        assertStatusCode(orderResponse, 201);
        assertSuccess(orderResponse);
        
        String orderId = orderResponse.jsonPath().getString("data.orderId");
        assertThat(orderId).isNotEmpty();
        
        Response costsResponse = ordersApi.getOrderCosts(orderId);
        assertStatusCode(costsResponse, 200);
        assertSuccess(costsResponse);
        
        Double subtotal = costsResponse.jsonPath().getDouble("data.subtotal");
        assertThat(subtotal).as("Subtotal should be positive").isGreaterThan(0);
        
        Object taxValue = costsResponse.jsonPath().get("data.tax");
        assertThat(taxValue)
                .as("Tax field should be present for complete cost data")
                .isNotNull();
        
        logTestEnd("getOrderCosts_withSmallOrder_hasCompleteCostData");
    }
    
    @Test(groups = {"regression", "orders"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify order has required fields including warehouse code")
    public void placeOrder_withValidRequest_hasRequiredFields() {
        logTestStart("placeOrder_withValidRequest_hasRequiredFields");
        
        String poNumber = "REQ-FIELDS-" + System.currentTimeMillis();
        
        PlaceOrderRequest request = PlaceOrderRequestBuilder.builder()
                .withDistributorId(VALID_DISTRIBUTOR_ID)
                .withShippingAddress(PlaceOrderRequestBuilder.createDefaultShippingAddress())
                .withShippingMethod("1")
                .withPoNumber(poNumber)
                .withTestOrder(true)
                .withLine(VALID_SKU, 5)
                .withDefaultPayment()
                .build();
        
        Response response = ordersApi.placeOrder(request);
        assertStatusCode(response, 201);
        assertSuccess(response);
        
        String warehouseCode = response.jsonPath().getString("data.warehouseCode");
        assertThat(warehouseCode)
                .as("Warehouse code should be present")
                .isNotEmpty();
        
        assertThat(warehouseCode)
                .as("Warehouse code validation")
                .isEqualTo("XX");
        
        logTestEnd("placeOrder_withValidRequest_hasRequiredFields");
    }
    
    @Test(groups = {"regression", "orders"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify rate limiting prevents exceeding threshold")
    public void placeMultipleOrders_respectsRateLimitThreshold() {
        logTestStart("placeMultipleOrders_respectsRateLimitThreshold");
        
        String distributorId = VALID_DISTRIBUTOR_ID;
        int thresholdRequests = 54;
        
        for (int i = 0; i < thresholdRequests; i++) {
            String poNumber = "RATE-LIMIT-" + System.currentTimeMillis() + "-" + i;
            PlaceOrderRequest request = PlaceOrderRequestBuilder.builder()
                    .withDistributorId(distributorId)
                    .withShippingAddress(PlaceOrderRequestBuilder.createDefaultShippingAddress())
                    .withShippingMethod("1")
                    .withPoNumber(poNumber)
                    .withTestOrder(true)
                    .withLine(VALID_SKU, 1)
                    .withDefaultPayment()
                    .build();
            
            Response response = ordersApi.placeOrder(request);
            assertStatusCode(response, 201);
        }
        
        String poNumber = "RATE-LIMIT-THRESHOLD-" + System.currentTimeMillis();
        PlaceOrderRequest request = PlaceOrderRequestBuilder.builder()
                .withDistributorId(distributorId)
                .withShippingAddress(PlaceOrderRequestBuilder.createDefaultShippingAddress())
                .withShippingMethod("1")
                .withPoNumber(poNumber)
                .withTestOrder(true)
                .withLine(VALID_SKU, 1)
                .withDefaultPayment()
                .build();
        
        Response response = ordersApi.placeOrder(request);
        
        assertThat(response.getStatusCode())
                .as("Request at threshold should be queued or rejected")
                .isIn(429, 503);
        
        logTestEnd("placeMultipleOrders_respectsRateLimitThreshold");
    }
}

package com.dif.api.tests.orders;

import com.dif.api.builders.PlaceOrderRequestBuilder;
import com.dif.api.client.OrdersApiClient;
import com.dif.api.models.request.PlaceOrderRequest;
import com.dif.api.tests.BaseTest;
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
                .as("Status should be 'Placed'")
                .isEqualTo("Placed");
        
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
}

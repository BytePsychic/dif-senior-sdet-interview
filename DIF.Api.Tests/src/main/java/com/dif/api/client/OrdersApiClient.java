package com.dif.api.client;

import com.dif.api.models.request.PlaceOrderRequest;
import io.restassured.response.Response;

import java.util.HashMap;
import java.util.Map;

/**
 * API client for Orders endpoints.
 * Provides methods to interact with /api/orders/* endpoints.
 */
public class OrdersApiClient extends BaseApiClient {
    
    private static final String ORDERS_BASE_PATH = "/api/orders";
    private static final String ORDER_BY_ID_PATH = ORDERS_BASE_PATH + "/{orderId}";
    private static final String ORDER_COSTS_PATH = ORDER_BY_ID_PATH + "/costs";
    private static final String ORDER_BY_DISTRIBUTOR_ID_PATH = ORDERS_BASE_PATH + "/by-distributor-id/{distributorOrderId}";
    
    /**
     * POST /api/orders - Place a new order.
     * @param request PlaceOrderRequest with order details
     * @return Response with order confirmation
     */
    public Response placeOrder(PlaceOrderRequest request) {
        return post(ORDERS_BASE_PATH, request);
    }
    
    /**
     * GET /api/orders - List all orders with optional filters.
     * @return Response with paginated order list
     */
    public Response listOrders() {
        return get(ORDERS_BASE_PATH);
    }
    
    /**
     * GET /api/orders - List orders with query parameters.
     * @param queryParams Map of query parameters (distributorId, status, page, pageSize)
     * @return Response with paginated order list
     */
    public Response listOrders(Map<String, String> queryParams) {
        if (queryParams == null || queryParams.isEmpty()) {
            return get(ORDERS_BASE_PATH);
        }
        return getWithQueryParams(ORDERS_BASE_PATH, queryParams);
    }
    
    /**
     * GET /api/orders - List orders with pagination.
     * @param page Page number (1-based)
     * @param pageSize Items per page
     * @return Response with paginated order list
     */
    public Response listOrders(int page, int pageSize) {
        Map<String, Object> queryParams = new HashMap<>();
        queryParams.put("page", page);
        queryParams.put("pageSize", pageSize);
        return getWithQueryParams(ORDERS_BASE_PATH, queryParams);
    }
    
    /**
     * GET /api/orders - List orders filtered by status.
     * @param status Order status (e.g., "Placed", "Processing", "Shipped", "Delivered")
     * @return Response with filtered order list
     */
    public Response listOrdersByStatus(String status) {
        Map<String, Object> queryParams = new HashMap<>();
        queryParams.put("status", status);
        return getWithQueryParams(ORDERS_BASE_PATH, queryParams);
    }
    
    /**
     * GET /api/orders/{orderId} - Get order by ID.
     * @param orderId Order ID (GUID)
     * @return Response with order details
     */
    public Response getOrder(String orderId) {
        return get(ORDER_BY_ID_PATH, orderId);
    }
    
    /**
     * GET /api/orders/{orderId}/costs - Get order cost breakdown.
     * @param orderId Order ID (GUID)
     * @return Response with cost details
     */
    public Response getOrderCosts(String orderId) {
        return get(ORDER_COSTS_PATH, orderId);
    }
    
    /**
     * GET /api/orders/by-distributor-id/{distributorOrderId} - Get order by distributor order ID.
     * @param distributorOrderId Distributor's order ID (e.g., "SS202601110001")
     * @return Response with order summary
     */
    public Response getOrderByDistributorId(String distributorOrderId) {
        return get(ORDER_BY_DISTRIBUTOR_ID_PATH, distributorOrderId);
    }
}

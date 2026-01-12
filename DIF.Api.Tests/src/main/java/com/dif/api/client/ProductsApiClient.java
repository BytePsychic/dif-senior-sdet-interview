package com.dif.api.client;

import io.restassured.response.Response;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * API client for Products endpoints.
 * Provides methods to interact with /api/products/* endpoints.
 */
public class ProductsApiClient extends BaseApiClient {
    
    private static final String PRODUCTS_BASE_PATH = "/api/products";
    private static final String PRODUCT_BY_SKU_PATH = PRODUCTS_BASE_PATH + "/{sku}";
    private static final String INVENTORY_PATH = PRODUCTS_BASE_PATH + "/{sku}/inventory";
    private static final String BATCH_INVENTORY_PATH = PRODUCTS_BASE_PATH + "/inventory/batch";
    
    /**
     * GET /api/products - List all products with optional filters.
     * @return Response with paginated product list
     */
    public Response listProducts() {
        return get(PRODUCTS_BASE_PATH);
    }
    
    /**
     * GET /api/products - List products with query parameters.
     * @param queryParams Map of query parameters (sku, styleCode, brandName, color, size, etc.)
     * @return Response with paginated product list
     */
    public Response listProducts(Map<String, String> queryParams) {
        if (queryParams == null || queryParams.isEmpty()) {
            return get(PRODUCTS_BASE_PATH);
        }
        return getWithQueryParams(PRODUCTS_BASE_PATH, queryParams);
    }
    
    /**
     * GET /api/products - List products with pagination.
     * @param page Page number (1-based)
     * @param pageSize Items per page
     * @return Response with paginated product list
     */
    public Response listProducts(int page, int pageSize) {
        Map<String, Object> queryParams = new HashMap<>();
        queryParams.put("page", page);
        queryParams.put("pageSize", pageSize);
        return getWithQueryParams(PRODUCTS_BASE_PATH, queryParams);
    }
    
    /**
     * GET /api/products/{sku} - Get a product by SKU.
     * @param sku Product SKU (e.g., "G500-BLK-M")
     * @return Response with product details
     */
    public Response getProductBySku(String sku) {
        return get(PRODUCT_BY_SKU_PATH, sku);
    }
    
    /**
     * GET /api/products/{sku}/inventory - Get inventory for a specific SKU.
     * @param sku Product SKU
     * @return Response with inventory data across warehouses
     */
    public Response getInventory(String sku) {
        return get(INVENTORY_PATH, sku);
    }
    
    /**
     * GET /api/products/inventory/batch - Get batch inventory for multiple SKUs.
     * @param skus List of SKUs to get inventory for
     * @return Response with inventory data for all requested SKUs
     */
    public Response getBatchInventory(List<String> skus) {
        Map<String, Object> queryParams = new HashMap<>();
        queryParams.put("skus", String.join(",", skus));
        return getWithQueryParams(BATCH_INVENTORY_PATH, queryParams);
    }
    
    /**
     * GET /api/products/inventory/batch - Get batch inventory with distributor filter.
     * @param skus List of SKUs to get inventory for
     * @param distributorId Distributor ID to filter by
     * @return Response with inventory data
     */
    public Response getBatchInventory(List<String> skus, String distributorId) {
        Map<String, Object> queryParams = new HashMap<>();
        queryParams.put("skus", String.join(",", skus));
        if (distributorId != null) {
            queryParams.put("distributorId", distributorId);
        }
        return getWithQueryParams(BATCH_INVENTORY_PATH, queryParams);
    }
}

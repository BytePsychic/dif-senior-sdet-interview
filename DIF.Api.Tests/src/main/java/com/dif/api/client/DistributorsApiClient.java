package com.dif.api.client;

import com.dif.api.models.request.ShippingEstimateRequest;
import io.restassured.response.Response;

/**
 * API client for Distributors endpoints.
 * Provides methods to interact with /api/distributors/* endpoints.
 */
public class DistributorsApiClient extends BaseApiClient {
    
    private static final String DISTRIBUTORS_BASE_PATH = "/api/distributors";
    private static final String DISTRIBUTOR_BY_ID_PATH = DISTRIBUTORS_BASE_PATH + "/{id}";
    private static final String WAREHOUSES_PATH = DISTRIBUTOR_BY_ID_PATH + "/warehouses";
    private static final String SHIPPING_OPTIONS_PATH = DISTRIBUTOR_BY_ID_PATH + "/shipping-options";
    private static final String RATE_LIMIT_STATUS_PATH = DISTRIBUTOR_BY_ID_PATH + "/rate-limit-status";
    private static final String SHIPPING_ESTIMATE_PATH = DISTRIBUTOR_BY_ID_PATH + "/shipping-estimate";
    
    /**
     * GET /api/distributors - List all distributors.
     * @return Response with list of distributors
     */
    public Response listDistributors() {
        return get(DISTRIBUTORS_BASE_PATH);
    }
    
    /**
     * GET /api/distributors/{id} - Get a distributor by ID.
     * @param distributorId Distributor ID (e.g., "ss", "img", "sanmar")
     * @return Response with distributor details
     */
    public Response getDistributor(String distributorId) {
        return get(DISTRIBUTOR_BY_ID_PATH, distributorId);
    }
    
    /**
     * GET /api/distributors/{id}/warehouses - Get warehouses for a distributor.
     * @param distributorId Distributor ID
     * @return Response with list of warehouses
     */
    public Response getWarehouses(String distributorId) {
        return get(WAREHOUSES_PATH, distributorId);
    }
    
    /**
     * GET /api/distributors/{id}/shipping-options - Get shipping options for a distributor.
     * @param distributorId Distributor ID
     * @return Response with list of shipping options
     */
    public Response getShippingOptions(String distributorId) {
        return get(SHIPPING_OPTIONS_PATH, distributorId);
    }
    
    /**
     * GET /api/distributors/{id}/rate-limit-status - Get rate limit status for a distributor.
     * @param distributorId Distributor ID
     * @return Response with rate limit status
     */
    public Response getRateLimitStatus(String distributorId) {
        return get(RATE_LIMIT_STATUS_PATH, distributorId);
    }
    
    /**
     * POST /api/distributors/{id}/shipping-estimate - Get shipping estimate.
     * @param distributorId Distributor ID
     * @param request Shipping estimate request
     * @return Response with shipping estimates
     */
    public Response getShippingEstimate(String distributorId, ShippingEstimateRequest request) {
        return post(SHIPPING_ESTIMATE_PATH, request, distributorId);
    }
}

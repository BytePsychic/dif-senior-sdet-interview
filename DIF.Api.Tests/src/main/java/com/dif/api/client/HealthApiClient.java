package com.dif.api.client;

import io.restassured.response.Response;

import java.util.HashMap;
import java.util.Map;

/**
 * API client for Health endpoints.
 * Provides methods to interact with /api/health/* endpoints.
 */
public class HealthApiClient extends BaseApiClient {
    
    private static final String HEALTH_BASE_PATH = "/api/health";
    private static final String PING_PATH = HEALTH_BASE_PATH + "/ping";
    private static final String DISTRIBUTORS_PATH = HEALTH_BASE_PATH + "/distributors";
    private static final String ERRORS_PATH = HEALTH_BASE_PATH + "/errors";
    private static final String ERRORS_RECENT_PATH = HEALTH_BASE_PATH + "/errors/recent";
    
    /**
     * GET /api/health - Get overall health status.
     * @return Response with health check data
     */
    public Response getHealth() {
        return get(HEALTH_BASE_PATH);
    }
    
    /**
     * GET /api/health/ping - Simple ping endpoint.
     * @return Response with "pong" text
     */
    public Response ping() {
        return get(PING_PATH);
    }
    
    /**
     * GET /api/health/distributors - Get health status of all distributors.
     * @return Response with distributor health data
     */
    public Response getDistributorHealth() {
        return get(DISTRIBUTORS_PATH);
    }
    
    /**
     * GET /api/health/errors - Get error statistics.
     * @return Response with error counts using default hours (24)
     */
    public Response getErrorStats() {
        return get(ERRORS_PATH);
    }
    
    /**
     * GET /api/health/errors - Get error statistics for specified hours.
     * @param hours Number of hours to look back
     * @return Response with error counts
     */
    public Response getErrorStats(int hours) {
        Map<String, Object> queryParams = new HashMap<>();
        queryParams.put("hours", hours);
        return getWithQueryParams(ERRORS_PATH, queryParams);
    }
    
    /**
     * GET /api/health/errors/recent - Get recent error details.
     * @return Response with recent errors using defaults
     */
    public Response getRecentErrors() {
        return get(ERRORS_RECENT_PATH);
    }
    
    /**
     * GET /api/health/errors/recent - Get recent error details with filters.
     * @param distributorId Filter by distributor ID (optional, can be null)
     * @param severity Filter by severity (optional, can be null)
     * @param hours Hours to look back (optional, can be null)
     * @return Response with recent errors
     */
    public Response getRecentErrors(String distributorId, String severity, Integer hours) {
        Map<String, Object> queryParams = new HashMap<>();
        if (distributorId != null) {
            queryParams.put("distributorId", distributorId);
        }
        if (severity != null) {
            queryParams.put("severity", severity);
        }
        if (hours != null) {
            queryParams.put("hours", hours);
        }
        
        if (queryParams.isEmpty()) {
            return get(ERRORS_RECENT_PATH);
        }
        return getWithQueryParams(ERRORS_RECENT_PATH, queryParams);
    }
}

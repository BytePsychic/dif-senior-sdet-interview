package com.dif.api.client;

import com.dif.api.models.request.TrackingUpdateRequest;
import io.restassured.response.Response;

/**
 * API client for Tracking endpoints.
 * Provides methods to interact with /api/tracking/* endpoints.
 */
public class TrackingApiClient extends BaseApiClient {
    
    private static final String TRACKING_BASE_PATH = "/api/tracking";
    private static final String TRACKING_BY_ORDER_PATH = TRACKING_BASE_PATH + "/{orderId}";
    private static final String TRACKING_BY_NUMBER_PATH = TRACKING_BASE_PATH + "/shipment/{trackingNumber}";
    private static final String TRACKING_ALL_PATH = TRACKING_BY_ORDER_PATH + "/all";
    private static final String TRACKING_UPDATE_PATH = TRACKING_BASE_PATH + "/update";
    private static final String DELIVERY_CONFIRMATION_PATH = TRACKING_BY_ORDER_PATH + "/delivery-confirmation";
    private static final String PENDING_PATH = TRACKING_BASE_PATH + "/pending";
    private static final String MISSHIPMENTS_PATH = TRACKING_BASE_PATH + "/misshipments";
    
    /**
     * GET /api/tracking/{orderId} - Get tracking by order ID.
     * @param orderId Order ID (GUID)
     * @return Response with tracking info
     */
    public Response getTrackingByOrderId(String orderId) {
        return get(TRACKING_BY_ORDER_PATH, orderId);
    }
    
    /**
     * GET /api/tracking/shipment/{trackingNumber} - Get tracking by tracking number.
     * @param trackingNumber Carrier tracking number
     * @return Response with detailed tracking info
     */
    public Response getTrackingByNumber(String trackingNumber) {
        return get(TRACKING_BY_NUMBER_PATH, trackingNumber);
    }
    
    /**
     * GET /api/tracking/{orderId}/all - Get all tracking for an order.
     * @param orderId Order ID (GUID)
     * @return Response with list of tracking info
     */
    public Response getAllTrackingForOrder(String orderId) {
        return get(TRACKING_ALL_PATH, orderId);
    }
    
    /**
     * POST /api/tracking/update - Trigger tracking update.
     * @param request TrackingUpdateRequest with filters
     * @return Response with update results
     */
    public Response triggerTrackingUpdate(TrackingUpdateRequest request) {
        return post(TRACKING_UPDATE_PATH, request);
    }
    
    /**
     * GET /api/tracking/{orderId}/delivery-confirmation - Get delivery confirmation.
     * @param orderId Order ID (GUID)
     * @return Response with delivery confirmation details
     */
    public Response getDeliveryConfirmation(String orderId) {
        return get(DELIVERY_CONFIRMATION_PATH, orderId);
    }
    
    /**
     * GET /api/tracking/pending - Get all pending shipments.
     * @return Response with list of pending shipments
     */
    public Response getPendingShipments() {
        return get(PENDING_PATH);
    }
    
    /**
     * GET /api/tracking/misshipments - Get misshipment alerts.
     * @return Response with list of misshipment alerts
     */
    public Response getMisshipments() {
        return get(MISSHIPMENTS_PATH);
    }
}

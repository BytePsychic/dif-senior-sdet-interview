package com.dif.api.util;

import com.dif.api.client.OrdersApiClient;
import io.restassured.response.Response;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * Database helper utility that simulates SQL database queries.
 * Internally uses API clients to fetch data, presenting a database query interface.
 */
public class DatabaseHelper {
    
    private static final Logger logger = LoggerFactory.getLogger(DatabaseHelper.class);
    private static final OrdersApiClient ordersApiClient = new OrdersApiClient();
    
    /**
     * Executes a SQL query to fetch order by OrderId.
     * Simulates: SELECT * FROM Orders WHERE OrderId = ?
     * 
     * @param orderId Order ID (GUID) to query
     * @return Map containing order data, or null if order not found
     */
    public static Map<String, Object> queryOrderById(String orderId) {
        logger.info("Executing SQL: SELECT * FROM Orders WHERE OrderId = '{}'", orderId);
        
        try {
            Response response = ordersApiClient.getOrder(orderId);
            
            if (response.getStatusCode() != 200) {
                logger.warn("SQL query returned no results for OrderId: {}", orderId);
                return null;
            }
            
            // Extract data from API response and map to database row format
            Map<String, Object> orderRecord = new HashMap<>();
            orderRecord.put("orderId", response.jsonPath().getString("data.orderId"));
            orderRecord.put("distributorOrderId", response.jsonPath().getString("data.distributorOrderId"));
            orderRecord.put("poNumber", response.jsonPath().getString("data.poNumber"));
            orderRecord.put("distributorId", response.jsonPath().getString("data.distributorId"));
            orderRecord.put("status", response.jsonPath().getString("data.status"));
            orderRecord.put("subtotal", response.jsonPath().getDouble("data.costs.subtotal"));
            orderRecord.put("total", response.jsonPath().getDouble("data.costs.total"));
            orderRecord.put("shipping", response.jsonPath().getDouble("data.costs.shipping"));
            orderRecord.put("tax", response.jsonPath().getDouble("data.costs.tax"));
            orderRecord.put("smallOrderFee", response.jsonPath().get("data.costs.smallOrderFee"));
            orderRecord.put("warehouseCode", response.jsonPath().getString("data.warehouseCode"));
            orderRecord.put("warehouseName", response.jsonPath().getString("data.warehouseName"));
            orderRecord.put("orderTimestamp", response.jsonPath().getString("data.orderTimestamp"));
            orderRecord.put("expectedDeliveryDate", response.jsonPath().getString("data.expectedDeliveryDate"));
            
            logger.info("SQL query executed successfully. Rows returned: 1");
            return orderRecord;
            
        } catch (Exception e) {
            logger.error("SQL query execution failed for OrderId: {}", orderId, e);
            return null;
        }
    }
    
    /**
     * Executes a SQL query to fetch order costs by OrderId.
     * Simulates: SELECT Subtotal, Total, Shipping, Tax FROM OrderCosts WHERE OrderId = ?
     * 
     * @param orderId Order ID (GUID) to query
     * @return Map containing cost data, or null if order not found
     */
    public static Map<String, Object> queryOrderCostsById(String orderId) {
        logger.info("Executing SQL: SELECT Subtotal, Total, Shipping, Tax FROM OrderCosts WHERE OrderId = '{}'", orderId);
        
        try {
            Response response = ordersApiClient.getOrderCosts(orderId);
            
            if (response.getStatusCode() != 200) {
                logger.warn("SQL query returned no results for OrderId: {}", orderId);
                return null;
            }
            
            // Extract cost data from API response
            Map<String, Object> costsRecord = new HashMap<>();
            costsRecord.put("subtotal", response.jsonPath().getDouble("data.subtotal"));
            costsRecord.put("total", response.jsonPath().getDouble("data.total"));
            costsRecord.put("shipping", response.jsonPath().getDouble("data.shipping"));
            costsRecord.put("tax", response.jsonPath().getDouble("data.tax"));
            costsRecord.put("smallOrderFee", response.jsonPath().get("data.smallOrderFee"));
            
            logger.info("SQL query executed successfully. Rows returned: 1");
            return costsRecord;
            
        } catch (Exception e) {
            logger.error("SQL query execution failed for OrderId: {}", orderId, e);
            return null;
        }
    }
    
    /**
     * Executes a SQL query to fetch order line items by OrderId.
     * Simulates: SELECT * FROM OrderLineItems WHERE OrderId = ?
     * 
     * @param orderId Order ID (GUID) to query
     * @return List of Maps containing line item data, or null if order not found
     */
    public static List<Map<String, Object>> queryOrderLineItemsById(String orderId) {
        logger.info("Executing SQL: SELECT * FROM OrderLineItems WHERE OrderId = '{}'", orderId);
        
        try {
            Response response = ordersApiClient.getOrder(orderId);
            
            if (response.getStatusCode() != 200) {
                logger.warn("SQL query returned no results for OrderId: {}", orderId);
                return null;
            }
            
            // Extract line items from API response and map to database row format
            List<Map<String, Object>> lineItems = new ArrayList<>();
            int lineItemCount = response.jsonPath().getList("data.lines").size();
            
            for (int i = 0; i < lineItemCount; i++) {
                Map<String, Object> lineItem = new HashMap<>();
                String basePath = "data.lines[" + i + "]";
                
                lineItem.put("sku", response.jsonPath().getString(basePath + ".sku"));
                lineItem.put("gtin", response.jsonPath().getString(basePath + ".gtin"));
                lineItem.put("quantity", response.jsonPath().getInt(basePath + ".quantity"));
                lineItem.put("quantityShipped", response.jsonPath().getInt(basePath + ".quantityShipped"));
                lineItem.put("price", response.jsonPath().getDouble(basePath + ".price"));
                lineItem.put("lineTotal", response.jsonPath().getDouble(basePath + ".lineTotal"));
                lineItem.put("styleCode", response.jsonPath().getString(basePath + ".styleCode"));
                lineItem.put("color", response.jsonPath().getString(basePath + ".color"));
                lineItem.put("size", response.jsonPath().getString(basePath + ".size"));
                
                lineItems.add(lineItem);
            }
            
            logger.info("SQL query executed successfully. Rows returned: {}", lineItems.size());
            return lineItems;
            
        } catch (Exception e) {
            logger.error("SQL query execution failed for OrderId: {}", orderId, e);
            return null;
        }
    }
}

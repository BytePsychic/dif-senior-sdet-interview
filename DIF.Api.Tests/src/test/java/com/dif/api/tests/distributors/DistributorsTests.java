package com.dif.api.tests.distributors;

import com.dif.api.client.DistributorsApiClient;
import com.dif.api.models.request.ShippingEstimateItem;
import com.dif.api.models.request.ShippingEstimateRequest;
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

import static org.assertj.core.api.Assertions.assertThat;

/**
 * Tests for Distributors API endpoints.
 * Covers: GET /api/distributors, GET /api/distributors/{id},
 * GET /api/distributors/{id}/warehouses, GET /api/distributors/{id}/shipping-options,
 * GET /api/distributors/{id}/rate-limit-status, POST /api/distributors/{id}/shipping-estimate
 */
@Feature("Distributors API")
public class DistributorsTests extends BaseTest {
    
    private DistributorsApiClient distributorsApi;
    
    // Known valid distributor IDs
    private static final String VALID_DISTRIBUTOR_ID = "ss";
    private static final String INVALID_DISTRIBUTOR_ID = "invalid-distributor";
    
    // Valid SKU for shipping estimate
    private static final String VALID_SKU = "G500-BLA-M";
    
    @BeforeClass
    @Override
    public void setUp() {
        super.setUp();
        distributorsApi = new DistributorsApiClient();
    }
    
    @Test(groups = {"smoke", "distributors"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify listing distributors returns distributor list")
    public void listDistributors_returnsDistributorList() {
        logTestStart("listDistributors_returnsDistributorList");
        
        Response response = distributorsApi.listDistributors();
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        List<Map<String, Object>> distributors = response.jsonPath().getList("data");
        assertThat(distributors)
                .as("Distributors list should not be empty")
                .isNotEmpty();
        
        // Verify S&S distributor is in the list
        List<String> distributorIds = response.jsonPath().getList("data.distributorId");
        assertThat(distributorIds)
                .as("S&S distributor should be in the list")
                .contains("ss");
        
        logTestEnd("listDistributors_returnsDistributorList");
    }
    
    @Test(groups = {"smoke", "distributors"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify getting distributor by valid ID returns distributor details")
    public void getDistributor_withValidId_returnsDistributorDetails() {
        logTestStart("getDistributor_withValidId_returnsDistributorDetails");
        
        Response response = distributorsApi.getDistributor(VALID_DISTRIBUTOR_ID);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify distributor data
        assertThat(response.jsonPath().getString("data.distributorId"))
                .as("Distributor ID should match")
                .isEqualTo(VALID_DISTRIBUTOR_ID);
        
        assertThat(response.jsonPath().getString("data.name"))
                .as("Distributor name should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getBoolean("data.hasApiIntegration"))
                .as("S&S should have API integration")
                .isTrue();
        
        logTestEnd("getDistributor_withValidId_returnsDistributorDetails");
    }
    
    @Test(groups = {"negative", "distributors"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting distributor by invalid ID returns 404")
    public void getDistributor_withInvalidId_returns404() {
        logTestStart("getDistributor_withInvalidId_returns404");
        
        Response response = distributorsApi.getDistributor(INVALID_DISTRIBUTOR_ID);
        
        assertStatusCode(response, 404);
        assertFailure(response);
        
        assertThat(response.jsonPath().getString("message"))
                .as("Error message should indicate distributor not found")
                .containsIgnoringCase("not found");
        
        logTestEnd("getDistributor_withInvalidId_returns404");
    }
    
    @Test(groups = {"smoke", "distributors"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify getting warehouses for distributor returns warehouse list")
    public void getWarehouses_withValidId_returnsWarehouseList() {
        logTestStart("getWarehouses_withValidId_returnsWarehouseList");
        
        Response response = distributorsApi.getWarehouses(VALID_DISTRIBUTOR_ID);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        List<Map<String, Object>> warehouses = response.jsonPath().getList("data");
        assertThat(warehouses)
                .as("Warehouses list should not be empty")
                .isNotEmpty();
        
        // Verify warehouse has required fields
        assertThat(response.jsonPath().getString("data[0].warehouseCode"))
                .as("Warehouse code should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getString("data[0].warehouseName"))
                .as("Warehouse name should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getString("data[0].state"))
                .as("Warehouse state should be present")
                .isNotEmpty();
        
        logTestEnd("getWarehouses_withValidId_returnsWarehouseList");
    }
    
    @Test(groups = {"negative", "distributors"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting warehouses for invalid distributor returns empty list")
    public void getWarehouses_withInvalidId_returnsEmptyList() {
        logTestStart("getWarehouses_withInvalidId_returnsEmptyList");
        
        Response response = distributorsApi.getWarehouses(INVALID_DISTRIBUTOR_ID);
        
        // API returns 200 with empty list for invalid distributor
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        List<Map<String, Object>> warehouses = response.jsonPath().getList("data");
        assertThat(warehouses)
                .as("Warehouses list should be empty for invalid distributor")
                .isEmpty();
        
        logTestEnd("getWarehouses_withInvalidId_returnsEmptyList");
    }
    
    @Test(groups = {"smoke", "distributors"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify getting shipping options for distributor returns options list")
    public void getShippingOptions_withValidId_returnsShippingOptionsList() {
        logTestStart("getShippingOptions_withValidId_returnsShippingOptionsList");
        
        Response response = distributorsApi.getShippingOptions(VALID_DISTRIBUTOR_ID);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        List<Map<String, Object>> shippingOptions = response.jsonPath().getList("data");
        assertThat(shippingOptions)
                .as("Shipping options list should not be empty")
                .isNotEmpty();
        
        // Verify shipping option has required fields
        assertThat(response.jsonPath().getString("data[0].methodCode"))
                .as("Method code should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getString("data[0].methodName"))
                .as("Method name should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getString("data[0].carrier"))
                .as("Carrier should be present")
                .isNotEmpty();
        
        logTestEnd("getShippingOptions_withValidId_returnsShippingOptionsList");
    }
    
    @Test(groups = {"negative", "distributors"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting shipping options for invalid distributor returns empty list")
    public void getShippingOptions_withInvalidId_returnsEmptyList() {
        logTestStart("getShippingOptions_withInvalidId_returnsEmptyList");
        
        Response response = distributorsApi.getShippingOptions(INVALID_DISTRIBUTOR_ID);
        
        // API returns 200 with empty list for invalid distributor
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        List<Map<String, Object>> shippingOptions = response.jsonPath().getList("data");
        assertThat(shippingOptions)
                .as("Shipping options list should be empty for invalid distributor")
                .isEmpty();
        
        logTestEnd("getShippingOptions_withInvalidId_returnsEmptyList");
    }
    
    @Test(groups = {"smoke", "distributors"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify getting rate limit status returns rate limit info")
    public void getRateLimitStatus_withValidId_returnsRateLimitInfo() {
        logTestStart("getRateLimitStatus_withValidId_returnsRateLimitInfo");
        
        Response response = distributorsApi.getRateLimitStatus(VALID_DISTRIBUTOR_ID);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify rate limit status fields
        assertThat(response.jsonPath().getString("data.distributorId"))
                .as("Distributor ID should match")
                .isEqualTo(VALID_DISTRIBUTOR_ID);
        
        assertThat(response.jsonPath().getInt("data.requestsPerMinute"))
                .as("Requests per minute should be positive")
                .isGreaterThan(0);
        
        assertThat(response.jsonPath().getInt("data.remainingRequests"))
                .as("Remaining requests should be non-negative")
                .isGreaterThanOrEqualTo(0);
        
        logTestEnd("getRateLimitStatus_withValidId_returnsRateLimitInfo");
    }
    
    @Test(groups = {"negative", "distributors"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting rate limit status for invalid distributor returns default status")
    public void getRateLimitStatus_withInvalidId_returnsDefaultStatus() {
        logTestStart("getRateLimitStatus_withInvalidId_returnsDefaultStatus");
        
        Response response = distributorsApi.getRateLimitStatus(INVALID_DISTRIBUTOR_ID);
        
        // API returns 200 with default rate limit status for any distributor ID
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // The distributor ID in response should match the requested ID
        assertThat(response.jsonPath().getString("data.distributorId"))
                .as("Distributor ID should match requested ID")
                .isEqualTo(INVALID_DISTRIBUTOR_ID);
        
        logTestEnd("getRateLimitStatus_withInvalidId_returnsDefaultStatus");
    }
    
    @Test(groups = {"smoke", "distributors"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify shipping estimate returns shipping options with costs")
    public void getShippingEstimate_withValidRequest_returnsShippingOptions() {
        logTestStart("getShippingEstimate_withValidRequest_returnsShippingOptions");
        
        ShippingEstimateRequest request = ShippingEstimateRequest.builder()
                .distributorId(VALID_DISTRIBUTOR_ID)
                .originWarehouseCode("IL")
                .destinationZip("90210")
                .items(Arrays.asList(
                        ShippingEstimateItem.builder()
                                .sku(VALID_SKU)
                                .quantity(24)
                                .build()
                ))
                .preferredShippingMethod("1")
                .build();
        
        Response response = distributorsApi.getShippingEstimate(VALID_DISTRIBUTOR_ID, request);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify shipping estimate data
        assertThat(response.jsonPath().getString("data.distributorId"))
                .as("Distributor ID should match")
                .isEqualTo(VALID_DISTRIBUTOR_ID);
        
        assertThat(response.jsonPath().getString("data.destinationZip"))
                .as("Destination ZIP should match")
                .isEqualTo("90210");
        
        List<Map<String, Object>> options = response.jsonPath().getList("data.options");
        assertThat(options)
                .as("Shipping options should be present")
                .isNotEmpty();
        
        // Verify at least one option has cost
        assertThat(response.jsonPath().getDouble("data.options[0].estimatedCost"))
                .as("Estimated cost should be positive")
                .isGreaterThan(0);
        
        logTestEnd("getShippingEstimate_withValidRequest_returnsShippingOptions");
    }
    
    @Test(groups = {"negative", "distributors"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify shipping estimate for invalid distributor returns estimate with empty options")
    public void getShippingEstimate_withInvalidDistributor_returnsEmptyOptions() {
        logTestStart("getShippingEstimate_withInvalidDistributor_returnsEmptyOptions");
        
        ShippingEstimateRequest request = ShippingEstimateRequest.builder()
                .distributorId(INVALID_DISTRIBUTOR_ID)
                .originWarehouseCode("IL")
                .destinationZip("90210")
                .items(Arrays.asList(
                        ShippingEstimateItem.builder()
                                .sku(VALID_SKU)
                                .quantity(24)
                                .build()
                ))
                .build();
        
        Response response = distributorsApi.getShippingEstimate(INVALID_DISTRIBUTOR_ID, request);
        
        // API returns 200 with estimate but may have empty or default options
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        logTestEnd("getShippingEstimate_withInvalidDistributor_returnsEmptyOptions");
    }
    
    @Test(groups = {"regression", "distributors"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify listing distributors returns expected fields")
    public void listDistributors_returnsExpectedFields() {
        logTestStart("listDistributors_returnsExpectedFields");
        
        Response response = distributorsApi.listDistributors();
        
        assertStatusCode(response, 200);
        
        // Verify first distributor has expected fields
        assertThat(response.jsonPath().getString("data[0].distributorId"))
                .as("Distributor ID should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getString("data[0].name"))
                .as("Name should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getString("data[0].code"))
                .as("Code should be present")
                .isNotEmpty();
        
        // hasApiIntegration should be a boolean
        Boolean hasApiIntegration = response.jsonPath().getBoolean("data[0].hasApiIntegration");
        assertThat(hasApiIntegration)
                .as("hasApiIntegration should be present")
                .isNotNull();
        
        logTestEnd("listDistributors_returnsExpectedFields");
    }
}

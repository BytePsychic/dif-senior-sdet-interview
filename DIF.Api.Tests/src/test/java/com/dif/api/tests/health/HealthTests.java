package com.dif.api.tests.health;

import com.dif.api.client.HealthApiClient;
import com.dif.api.tests.BaseTest;
import io.qameta.allure.Description;
import io.qameta.allure.Feature;
import io.qameta.allure.Severity;
import io.qameta.allure.SeverityLevel;
import io.restassured.response.Response;
import org.testng.annotations.BeforeClass;
import org.testng.annotations.Test;

import static org.assertj.core.api.Assertions.assertThat;

/**
 * Tests for Health API endpoints.
 * Covers: GET /api/health, GET /api/health/ping, GET /api/health/distributors,
 * GET /api/health/errors, GET /api/health/errors/recent
 */
@Feature("Health API")
public class HealthTests extends BaseTest {
    
    private HealthApiClient healthApi;
    
    @BeforeClass
    @Override
    public void setUp() {
        super.setUp();
        healthApi = new HealthApiClient();
    }
    
    @Test(groups = {"smoke", "health"})
    @Severity(SeverityLevel.BLOCKER)
    @Description("Verify ping endpoint returns 200 OK with 'pong' response")
    public void ping_returnsOkAndPong() {
        logTestStart("ping_returnsOkAndPong");
        
        Response response = healthApi.ping();
        
        assertStatusCode(response, 200);
        // API returns JSON-formatted string with quotes
        assertThat(response.asString())
                .as("Ping response should contain 'pong'")
                .contains("pong");
        
        logTestEnd("ping_returnsOkAndPong");
    }
    
    @Test(groups = {"smoke", "health"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify health check endpoint returns healthy status")
    public void healthCheck_returnsHealthyStatus() {
        logTestStart("healthCheck_returnsHealthyStatus");
        
        Response response = healthApi.getHealth();
        
        assertStatusCode(response, 200);
        
        String status = response.jsonPath().getString("status");
        assertThat(status)
                .as("Health status should be 'Healthy'")
                .isEqualTo("Healthy");
        
        String version = response.jsonPath().getString("version");
        assertThat(version)
                .as("Version should not be empty")
                .isNotEmpty();
        
        String timestamp = response.jsonPath().getString("timestamp");
        assertThat(timestamp)
                .as("Timestamp should be present")
                .isNotEmpty();
        
        logTestEnd("healthCheck_returnsHealthyStatus");
    }
    
    @Test(groups = {"smoke", "health"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify health check returns component health details")
    public void healthCheck_returnsComponentDetails() {
        logTestStart("healthCheck_returnsComponentDetails");
        
        Response response = healthApi.getHealth();
        
        assertStatusCode(response, 200);
        
        // Verify components exist
        assertThat(response.jsonPath().getMap("components"))
                .as("Components should be present in response")
                .isNotEmpty();
        
        // Verify API component status
        String apiStatus = response.jsonPath().getString("components.api.status");
        assertThat(apiStatus)
                .as("API component should be healthy")
                .isEqualTo("Healthy");
        
        logTestEnd("healthCheck_returnsComponentDetails");
    }
    
    @Test(groups = {"regression", "health"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify distributor health endpoint returns status for all distributors")
    public void getDistributorHealth_returnsDistributorStatuses() {
        logTestStart("getDistributorHealth_returnsDistributorStatuses");
        
        Response response = healthApi.getDistributorHealth();
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify data contains distributor health info
        assertThat(response.jsonPath().getMap("data"))
                .as("Data should contain distributor health statuses")
                .isNotEmpty();
        
        // Verify S&S distributor is present
        String ssStatus = response.jsonPath().getString("data.ss.status");
        assertThat(ssStatus)
                .as("S&S distributor status should be present")
                .isNotEmpty();
        
        logTestEnd("getDistributorHealth_returnsDistributorStatuses");
    }
    
    @Test(groups = {"regression", "health"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify error stats endpoint returns error counts by severity")
    public void getErrorStats_returnsErrorCounts() {
        logTestStart("getErrorStats_returnsErrorCounts");
        
        Response response = healthApi.getErrorStats();
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify data contains error counts
        assertThat(response.jsonPath().getMap("data"))
                .as("Data should contain error count statistics")
                .isNotNull();
        
        logTestEnd("getErrorStats_returnsErrorCounts");
    }
    
    @Test(groups = {"regression", "health"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify error stats endpoint accepts hours parameter")
    public void getErrorStats_withHoursParameter_returnsErrorCounts() {
        logTestStart("getErrorStats_withHoursParameter_returnsErrorCounts");
        
        Response response = healthApi.getErrorStats(12);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        String message = response.jsonPath().getString("message");
        assertThat(message)
                .as("Message should mention the hour range")
                .containsIgnoringCase("12 hours");
        
        logTestEnd("getErrorStats_withHoursParameter_returnsErrorCounts");
    }
    
    @Test(groups = {"regression", "health"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify recent errors endpoint returns error details")
    public void getRecentErrors_returnsErrorList() {
        logTestStart("getRecentErrors_returnsErrorList");
        
        Response response = healthApi.getRecentErrors();
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Data should be a list (may be empty if no errors)
        assertThat(response.jsonPath().getList("data"))
                .as("Data should be a list")
                .isNotNull();
        
        logTestEnd("getRecentErrors_returnsErrorList");
    }
    
    @Test(groups = {"regression", "health"})
    @Severity(SeverityLevel.MINOR)
    @Description("Verify recent errors endpoint accepts filter parameters")
    public void getRecentErrors_withFilters_returnsFilteredErrors() {
        logTestStart("getRecentErrors_withFilters_returnsFilteredErrors");
        
        Response response = healthApi.getRecentErrors("ss", "Warning", 48);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Data should be a list (filtered by parameters)
        assertThat(response.jsonPath().getList("data"))
                .as("Data should be a list")
                .isNotNull();
        
        logTestEnd("getRecentErrors_withFilters_returnsFilteredErrors");
    }
}

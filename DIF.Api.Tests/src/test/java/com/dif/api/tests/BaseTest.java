package com.dif.api.tests;

import com.dif.api.client.HealthApiClient;
import com.dif.api.config.ApiConfig;
import io.qameta.allure.Step;
import io.restassured.response.Response;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.testng.annotations.BeforeClass;
import org.testng.annotations.BeforeSuite;

import static org.assertj.core.api.Assertions.assertThat;

/**
 * Base test class providing common setup and utilities for all tests.
 * All test classes should extend this class.
 */
public abstract class BaseTest {
    
    protected static final Logger logger = LoggerFactory.getLogger(BaseTest.class);
    
    /**
     * Verify API is accessible before running any tests.
     * This runs once before the entire test suite.
     */
    @BeforeSuite(alwaysRun = true)
    @Step("Verify API is running at {ApiConfig.getBaseUrl()}")
    public void verifyApiIsRunning() {
        logger.info("Verifying API is running at: {}", ApiConfig.getBaseUrl());
        
        HealthApiClient healthClient = new HealthApiClient();
        Response response = healthClient.ping();
        
        assertThat(response.getStatusCode())
                .as("API should be accessible - ping endpoint should return 200")
                .isEqualTo(200);
        
        logger.info("API is accessible. Ping response: {}", response.asString());
    }
    
    /**
     * Setup method to be called before each test class.
     * Subclasses can override to initialize specific API clients.
     */
    @BeforeClass(alwaysRun = true)
    public void setUp() {
        logger.info("Setting up test class: {}", this.getClass().getSimpleName());
    }
    
    /**
     * Logs the start of a test with the given name.
     * @param testName Name of the test
     */
    protected void logTestStart(String testName) {
        logger.info("========== Starting test: {} ==========", testName);
    }
    
    /**
     * Logs the end of a test with the given name.
     * @param testName Name of the test
     */
    protected void logTestEnd(String testName) {
        logger.info("========== Finished test: {} ==========", testName);
    }
    
    /**
     * Asserts that the response has the expected status code.
     * @param response Response to check
     * @param expectedStatusCode Expected HTTP status code
     */
    protected void assertStatusCode(Response response, int expectedStatusCode) {
        assertThat(response.getStatusCode())
                .as("Response status code should be " + expectedStatusCode)
                .isEqualTo(expectedStatusCode);
    }
    
    /**
     * Asserts that the response indicates success (success field is true).
     * @param response Response to check
     */
    protected void assertSuccess(Response response) {
        Boolean success = response.jsonPath().getBoolean("success");
        assertThat(success)
                .as("Response should indicate success")
                .isTrue();
    }
    
    /**
     * Asserts that the response indicates failure (success field is false).
     * @param response Response to check
     */
    protected void assertFailure(Response response) {
        Boolean success = response.jsonPath().getBoolean("success");
        assertThat(success)
                .as("Response should indicate failure")
                .isFalse();
    }
}

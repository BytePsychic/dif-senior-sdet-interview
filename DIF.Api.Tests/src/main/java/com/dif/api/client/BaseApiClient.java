package com.dif.api.client;

import com.dif.api.config.ApiConfig;
import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule;
import io.restassured.RestAssured;
import io.restassured.builder.RequestSpecBuilder;
import io.restassured.config.ObjectMapperConfig;
import io.restassured.config.RestAssuredConfig;
import io.restassured.filter.log.LogDetail;
import io.restassured.http.ContentType;
import io.restassured.response.Response;
import io.restassured.specification.RequestSpecification;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.Map;

/**
 * Base API client providing common Rest Assured configuration and methods.
 * All endpoint-specific clients should extend this class.
 */
public abstract class BaseApiClient {
    
    protected static final Logger logger = LoggerFactory.getLogger(BaseApiClient.class);
    protected final RequestSpecification requestSpec;
    protected static final ObjectMapper objectMapper;
    
    static {
        // Configure Jackson ObjectMapper
        objectMapper = new ObjectMapper();
        objectMapper.registerModule(new JavaTimeModule());
        objectMapper.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
        objectMapper.configure(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS, false);
    }
    
    /**
     * Constructs a BaseApiClient with default configuration.
     */
    public BaseApiClient() {
        RequestSpecBuilder builder = new RequestSpecBuilder()
                .setBaseUri(ApiConfig.getBaseUrl())
                .setContentType(ContentType.JSON)
                .setAccept(ContentType.JSON)
                .setConfig(RestAssuredConfig.config()
                        .objectMapperConfig(ObjectMapperConfig.objectMapperConfig()
                                .jackson2ObjectMapperFactory((type, s) -> objectMapper)));
        
        // Add logging based on configuration
        if (ApiConfig.isRequestLoggingEnabled()) {
            builder.log(LogDetail.ALL);
        }
        
        this.requestSpec = builder.build();
        
        // Configure RestAssured response logging
        if (ApiConfig.isResponseLoggingEnabled()) {
            RestAssured.enableLoggingOfRequestAndResponseIfValidationFails(LogDetail.ALL);
        }
    }
    
    /**
     * Performs a GET request to the specified path.
     * @param path API endpoint path
     * @return Response object
     */
    protected Response get(String path) {
        logger.debug("GET request to: {}", path);
        return RestAssured.given()
                .spec(requestSpec)
                .when()
                .get(path);
    }
    
    /**
     * Performs a GET request with path parameters.
     * @param path API endpoint path with placeholders
     * @param pathParams Path parameter values
     * @return Response object
     */
    protected Response get(String path, Object... pathParams) {
        logger.debug("GET request to: {} with params: {}", path, pathParams);
        return RestAssured.given()
                .spec(requestSpec)
                .when()
                .get(path, pathParams);
    }
    
    /**
     * Performs a GET request with query parameters.
     * @param path API endpoint path
     * @param queryParams Query parameters map
     * @return Response object
     */
    protected Response getWithQueryParams(String path, Map<String, ?> queryParams) {
        logger.debug("GET request to: {} with query params: {}", path, queryParams);
        return RestAssured.given()
                .spec(requestSpec)
                .queryParams(queryParams)
                .when()
                .get(path);
    }
    
    /**
     * Performs a POST request with a JSON body.
     * @param path API endpoint path
     * @param body Request body object (will be serialized to JSON)
     * @return Response object
     */
    protected Response post(String path, Object body) {
        logger.debug("POST request to: {} with body: {}", path, body);
        return RestAssured.given()
                .spec(requestSpec)
                .body(body)
                .when()
                .post(path);
    }
    
    /**
     * Performs a POST request with path parameters and a JSON body.
     * @param path API endpoint path with placeholders
     * @param body Request body object
     * @param pathParams Path parameter values
     * @return Response object
     */
    protected Response post(String path, Object body, Object... pathParams) {
        logger.debug("POST request to: {} with body: {} and params: {}", path, body, pathParams);
        return RestAssured.given()
                .spec(requestSpec)
                .body(body)
                .when()
                .post(path, pathParams);
    }
    
    /**
     * Performs a PUT request with a JSON body.
     * @param path API endpoint path
     * @param body Request body object
     * @return Response object
     */
    protected Response put(String path, Object body) {
        logger.debug("PUT request to: {} with body: {}", path, body);
        return RestAssured.given()
                .spec(requestSpec)
                .body(body)
                .when()
                .put(path);
    }
    
    /**
     * Performs a DELETE request.
     * @param path API endpoint path
     * @return Response object
     */
    protected Response delete(String path) {
        logger.debug("DELETE request to: {}", path);
        return RestAssured.given()
                .spec(requestSpec)
                .when()
                .delete(path);
    }
    
    /**
     * Gets the configured ObjectMapper for JSON processing.
     * @return ObjectMapper instance
     */
    public static ObjectMapper getObjectMapper() {
        return objectMapper;
    }
}

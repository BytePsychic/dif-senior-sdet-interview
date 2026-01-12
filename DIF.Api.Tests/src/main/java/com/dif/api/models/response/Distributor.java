package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * Response model for distributor data.
 * Matches the distributor structure from GET /api/distributors endpoints.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class Distributor {
    
    private String distributorId;
    private String name;
    private String code;
    private String apiBaseUrl;
    private boolean hasApiIntegration;
    private boolean isActive;
    private String apiVersion;
    private String healthStatus;
    private String contactEmail;
    private String contactPhone;
    private String notes;
    private RateLimitConfig rateLimitConfig;
    private String lastSuccessfulConnection;
}

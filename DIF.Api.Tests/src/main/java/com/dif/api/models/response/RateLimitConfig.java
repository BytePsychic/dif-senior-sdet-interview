package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * Response model for rate limit configuration.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class RateLimitConfig {
    
    private String distributorId;
    private String distributorName;
    private int requestsPerMinute;
    private int thresholdPercentage;
    private int thresholdRequestCount;
    private int burstAllowance;
    private int currentRequestCount;
    private int remainingRequests;
    private boolean isApproachingLimit;
    private boolean isRateLimited;
}

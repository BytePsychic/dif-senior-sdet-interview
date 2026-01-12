package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * Response model for rate limit status.
 * Matches the structure from GET /api/distributors/{id}/rate-limit-status.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class RateLimitStatus {
    
    private String distributorId;
    private String distributorName;
    private int requestsPerMinute;
    private int currentRequestCount;
    private int remainingRequests;
    private boolean isApproachingLimit;
    private boolean isRateLimited;
    private int secondsUntilReset;
    private int queueDepth;
}

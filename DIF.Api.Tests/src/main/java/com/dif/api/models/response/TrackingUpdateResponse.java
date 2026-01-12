package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * Response model for tracking update.
 * Matches the structure from POST /api/tracking/update.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class TrackingUpdateResponse {
    
    private int shipmentsUpdated;
    private String updatedAt;
}

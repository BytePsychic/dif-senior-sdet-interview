package com.dif.api.models.request;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

/**
 * Request model for triggering tracking updates.
 * Used with POST /api/tracking/update.
 */
@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class TrackingUpdateRequest {
    
    private List<String> orderIds;
    private List<String> trackingNumbers;
    private String distributorId;
    private List<String> statusFilter;
}

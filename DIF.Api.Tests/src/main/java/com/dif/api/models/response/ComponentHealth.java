package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * Health status of an individual component.
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class ComponentHealth {
    
    private String status;
    private String message;
    private String lastSuccessful;
    private Long responseTimeMs;
}

package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

/**
 * Generic API response wrapper matching the DIF API response structure.
 * Used for single item responses.
 * 
 * @param <T> Type of the data payload
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class ApiResponse<T> {
    
    private boolean success;
    private T data;
    private String message;
    private List<String> errors;
    private String timestamp;
    private String correlationId;
}

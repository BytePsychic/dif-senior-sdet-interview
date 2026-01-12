package com.dif.api.models.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

/**
 * Paginated response wrapper matching the DIF API paginated response structure.
 * Used for list endpoints.
 * 
 * @param <T> Type of items in the list
 */
@Data
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class PaginatedResponse<T> {
    
    private boolean success;
    private List<T> items;
    private int page;
    private int pageSize;
    private int totalItems;
    private int totalPages;
    private boolean hasNextPage;
    private boolean hasPreviousPage;
    private String message;
    private String timestamp;
}

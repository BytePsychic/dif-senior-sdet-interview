package com.dif.api.models.request;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * Request model for shipping address.
 */
@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class ShippingAddress {
    
    private String customer;
    private String address;
    private String address2;
    private String city;
    private String state;
    private String zip;
    private String country;
    private String phone;
}

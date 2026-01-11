using System.ComponentModel.DataAnnotations;

namespace DIF.Api.Models.DTOs;

/// <summary>
/// DTO for shipping address.
/// </summary>
public class ShippingAddressDto
{
    /// <summary>
    /// Customer/printer name.
    /// </summary>
    [Required]
    public string Customer { get; set; } = string.Empty;

    /// <summary>
    /// Street address line 1.
    /// </summary>
    [Required]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Street address line 2 (optional).
    /// </summary>
    public string? Address2 { get; set; }

    /// <summary>
    /// City name.
    /// </summary>
    [Required]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State code (e.g., "IL", "CA").
    /// </summary>
    [Required]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "State must be a 2-letter code")]
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// ZIP/postal code.
    /// </summary>
    [Required]
    public string Zip { get; set; } = string.Empty;

    /// <summary>
    /// Country code (default "US").
    /// </summary>
    public string Country { get; set; } = "US";

    /// <summary>
    /// Phone number (optional).
    /// </summary>
    public string? Phone { get; set; }
}


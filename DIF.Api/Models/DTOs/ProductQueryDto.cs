using System.ComponentModel.DataAnnotations;

namespace DIF.Api.Models.DTOs;

/// <summary>
/// DTO for querying products.
/// </summary>
public class ProductQueryDto
{
    /// <summary>
    /// Search by SKU.
    /// </summary>
    public string? Sku { get; set; }

    /// <summary>
    /// Search by style code.
    /// </summary>
    public string? StyleCode { get; set; }

    /// <summary>
    /// Search by brand name.
    /// </summary>
    public string? BrandName { get; set; }

    /// <summary>
    /// Search by GTIN (barcode).
    /// </summary>
    public string? Gtin { get; set; }

    /// <summary>
    /// Filter by color.
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Filter by size.
    /// </summary>
    public string? Size { get; set; }

    /// <summary>
    /// Filter by distributor ID.
    /// </summary>
    public string? DistributorId { get; set; }

    /// <summary>
    /// Only return in-stock items.
    /// </summary>
    public bool? InStockOnly { get; set; }

    /// <summary>
    /// Page number (1-based).
    /// </summary>
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page.
    /// </summary>
    [Range(1, 100)]
    public int PageSize { get; set; } = 50;
}

/// <summary>
/// DTO for inventory query.
/// </summary>
public class InventoryQueryDto
{
    /// <summary>
    /// SKU to check inventory for.
    /// </summary>
    [Required]
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Distributor ID (optional, returns all if not specified).
    /// </summary>
    public string? DistributorId { get; set; }

    /// <summary>
    /// Warehouse code (optional, returns all warehouses if not specified).
    /// </summary>
    public string? WarehouseCode { get; set; }
}


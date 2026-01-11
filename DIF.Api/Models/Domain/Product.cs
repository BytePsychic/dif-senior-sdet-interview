using System;
using System.Collections.Generic;

namespace DIF.Api.Models.Domain;

/// <summary>
/// Represents a product from a distributor's catalog.
/// Maps to S&amp;S /v2/products/ API response.
/// </summary>
public class Product
{
    /// <summary>
    /// Internal unique identifier.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// SKU identifier (e.g., "B00760003").
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Product style code.
    /// </summary>
    public string StyleCode { get; set; } = string.Empty;

    /// <summary>
    /// Product style name.
    /// </summary>
    public string StyleName { get; set; } = string.Empty;

    /// <summary>
    /// Brand name (e.g., "Gildan", "Bella+Canvas").
    /// </summary>
    public string BrandName { get; set; } = string.Empty;

    /// <summary>
    /// Global Trade Item Number (barcode).
    /// </summary>
    public string Gtin { get; set; } = string.Empty;

    /// <summary>
    /// Color name.
    /// </summary>
    public string Color { get; set; } = string.Empty;

    /// <summary>
    /// Color code.
    /// </summary>
    public string ColorCode { get; set; } = string.Empty;

    /// <summary>
    /// Size (e.g., "S", "M", "L", "XL").
    /// </summary>
    public string Size { get; set; } = string.Empty;

    /// <summary>
    /// Size code.
    /// </summary>
    public string SizeCode { get; set; } = string.Empty;

    /// <summary>
    /// Product image URL.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Blank cost (price per unit).
    /// </summary>
    public decimal BlankCost { get; set; }

    /// <summary>
    /// MSRP if available.
    /// </summary>
    public decimal? Msrp { get; set; }

    /// <summary>
    /// Product description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Product category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Weight per unit in pounds.
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// Distributor ID this product belongs to.
    /// </summary>
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Whether the product is active/available.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Whether the product is discontinued.
    /// </summary>
    public bool IsDiscontinued { get; set; }

    /// <summary>
    /// Last time this product data was updated.
    /// </summary>
    public DateTime LastUpdated { get; set; }
}

/// <summary>
/// Represents inventory/stock levels for a product at a specific warehouse.
/// </summary>
public class InventoryStock
{
    /// <summary>
    /// Internal unique identifier.
    /// </summary>
    public Guid StockId { get; set; }

    /// <summary>
    /// SKU identifier.
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Warehouse code where this stock is located.
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// Warehouse name.
    /// </summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// Quantity available in stock.
    /// </summary>
    public int QuantityAvailable { get; set; }

    /// <summary>
    /// Quantity reserved for pending orders.
    /// </summary>
    public int QuantityReserved { get; set; }

    /// <summary>
    /// Quantity on backorder.
    /// </summary>
    public int QuantityOnBackorder { get; set; }

    /// <summary>
    /// Whether this item is in stock (QuantityAvailable > 0).
    /// </summary>
    public bool InStock => QuantityAvailable > 0;

    /// <summary>
    /// Expected restock date if out of stock.
    /// </summary>
    public DateTime? ExpectedRestockDate { get; set; }

    /// <summary>
    /// Distributor ID.
    /// </summary>
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Last time this stock data was updated.
    /// </summary>
    public DateTime LastUpdated { get; set; }
}

/// <summary>
/// Represents a distributor warehouse.
/// </summary>
public class Warehouse
{
    /// <summary>
    /// Internal unique identifier.
    /// </summary>
    public Guid WarehouseId { get; set; }

    /// <summary>
    /// Warehouse abbreviation code (e.g., "IL", "CA").
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// Warehouse company/facility name.
    /// </summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// Street address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// City.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State code.
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// ZIP code.
    /// </summary>
    public string Zip { get; set; } = string.Empty;

    /// <summary>
    /// Country code.
    /// </summary>
    public string Country { get; set; } = "US";

    /// <summary>
    /// Order cutoff time for same-day shipping.
    /// Note: NOT available in S&amp;S API - requires manual configuration.
    /// </summary>
    public TimeSpan? CutoffTime { get; set; }

    /// <summary>
    /// Timezone for the cutoff time.
    /// </summary>
    public string Timezone { get; set; } = "America/Chicago";

    /// <summary>
    /// Distributor ID this warehouse belongs to.
    /// </summary>
    public string DistributorId { get; set; } = string.Empty;

    /// <summary>
    /// Whether this warehouse is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Phone number for the warehouse.
    /// </summary>
    public string Phone { get; set; } = string.Empty;
}

/// <summary>
/// Represents a product query for searching products.
/// </summary>
public class ProductQuery
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
    /// Search by color.
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Search by size.
    /// </summary>
    public string? Size { get; set; }

    /// <summary>
    /// Search by GTIN.
    /// </summary>
    public string? Gtin { get; set; }

    /// <summary>
    /// Filter by distributor.
    /// </summary>
    public string? DistributorId { get; set; }

    /// <summary>
    /// Only return in-stock items.
    /// </summary>
    public bool? InStockOnly { get; set; }

    /// <summary>
    /// Page number for pagination.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Page size for pagination.
    /// </summary>
    public int PageSize { get; set; } = 50;
}


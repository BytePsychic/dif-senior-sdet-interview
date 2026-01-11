using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIF.Api.Models.Domain;
using DIF.Api.Models.DTOs;

namespace DIF.Api.Services.Interfaces;

/// <summary>
/// Service interface for distributor API operations.
/// Handles order placement, product queries, and inventory checks.
/// </summary>
public interface IDistributorService
{
    /// <summary>
    /// Places an order with the specified distributor.
    /// </summary>
    /// <param name="request">Order placement request.</param>
    /// <returns>The placed order with distributor response data.</returns>
    Task<Order> PlaceOrderAsync(PlaceOrderRequest request);

    /// <summary>
    /// Gets an order by its internal ID.
    /// </summary>
    /// <param name="orderId">Internal order ID.</param>
    /// <returns>The order if found, null otherwise.</returns>
    Task<Order?> GetOrderByIdAsync(Guid orderId);

    /// <summary>
    /// Gets an order by its distributor order ID.
    /// </summary>
    /// <param name="distributorOrderId">Distributor's order ID.</param>
    /// <returns>The order if found, null otherwise.</returns>
    Task<Order?> GetOrderByDistributorIdAsync(string distributorOrderId);

    /// <summary>
    /// Gets orders with optional filtering.
    /// </summary>
    /// <param name="distributorId">Filter by distributor (optional).</param>
    /// <param name="status">Filter by status (optional).</param>
    /// <param name="page">Page number.</param>
    /// <param name="pageSize">Page size.</param>
    /// <returns>List of orders matching the criteria.</returns>
    Task<(List<Order> Orders, int TotalCount)> GetOrdersAsync(
        string? distributorId = null, 
        string? status = null, 
        int page = 1, 
        int pageSize = 50);

    /// <summary>
    /// Gets the cost breakdown for an order.
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    /// <returns>Order costs if found.</returns>
    Task<OrderCosts?> GetOrderCostsAsync(Guid orderId);

    /// <summary>
    /// Gets products from distributor catalog.
    /// </summary>
    /// <param name="query">Product query parameters.</param>
    /// <returns>List of matching products and total count.</returns>
    Task<(List<Product> Products, int TotalCount)> GetProductsAsync(ProductQuery query);

    /// <summary>
    /// Gets a single product by SKU.
    /// </summary>
    /// <param name="sku">SKU identifier.</param>
    /// <param name="distributorId">Distributor ID (optional).</param>
    /// <returns>The product if found.</returns>
    Task<Product?> GetProductBySkuAsync(string sku, string? distributorId = null);

    /// <summary>
    /// Gets inventory/stock levels for a SKU.
    /// </summary>
    /// <param name="sku">SKU identifier.</param>
    /// <param name="distributorId">Distributor ID (optional).</param>
    /// <returns>List of inventory records across warehouses.</returns>
    Task<List<InventoryStock>> GetInventoryAsync(string sku, string? distributorId = null);

    /// <summary>
    /// Gets all configured distributors.
    /// </summary>
    /// <returns>List of distributors.</returns>
    Task<List<Distributor>> GetDistributorsAsync();

    /// <summary>
    /// Gets a distributor by ID.
    /// </summary>
    /// <param name="distributorId">Distributor ID.</param>
    /// <returns>The distributor if found.</returns>
    Task<Distributor?> GetDistributorByIdAsync(string distributorId);

    /// <summary>
    /// Gets warehouses for a distributor.
    /// </summary>
    /// <param name="distributorId">Distributor ID.</param>
    /// <returns>List of warehouses.</returns>
    Task<List<Warehouse>> GetWarehousesAsync(string distributorId);

    /// <summary>
    /// Gets available shipping options for a distributor.
    /// </summary>
    /// <param name="distributorId">Distributor ID.</param>
    /// <returns>List of shipping options.</returns>
    Task<List<ShippingOption>> GetShippingOptionsAsync(string distributorId);

    /// <summary>
    /// Gets shipping estimates for a shipment.
    /// </summary>
    /// <param name="request">Shipping estimate request.</param>
    /// <returns>Shipping estimate with options.</returns>
    Task<ShippingEstimate> GetShippingEstimateAsync(ShippingEstimateRequest request);
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIF.Api.Mocks;
using DIF.Api.Models.Domain;
using DIF.Api.Models.DTOs;
using DIF.Api.Services.Interfaces;

namespace DIF.Api.Services.Implementations;

/// <summary>
/// Mock implementation of IDistributorService for testing.
/// Returns realistic mock data without actual API calls.
/// </summary>
public class MockDistributorService : IDistributorService
{
    private readonly List<Distributor> _distributors;
    private readonly List<Product> _products;
    private readonly List<InventoryStock> _inventory;
    private readonly List<Order> _orders;

    public MockDistributorService()
    {
        _distributors = MockDataFactory.CreateDistributors();
        _products = MockDataFactory.CreateProducts();
        _inventory = MockDataFactory.CreateInventory(_products);
        _orders = MockDataFactory.CreateSampleOrders(20);
    }

    /// <inheritdoc />
    public Task<Order> PlaceOrderAsync(PlaceOrderRequest request)
    {
        var order = MockDataFactory.CreateOrderFromRequest(request);
        _orders.Add(order);
        return Task.FromResult(order);
    }

    /// <inheritdoc />
    public Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
        return Task.FromResult(order);
    }

    /// <inheritdoc />
    public Task<Order?> GetOrderByDistributorIdAsync(string distributorOrderId)
    {
        var order = _orders.FirstOrDefault(o => o.DistributorOrderId == distributorOrderId);
        return Task.FromResult(order);
    }

    /// <inheritdoc />
    public Task<(List<Order> Orders, int TotalCount)> GetOrdersAsync(
        string? distributorId = null, 
        string? status = null, 
        int page = 1, 
        int pageSize = 50)
    {
        var query = _orders.AsEnumerable();

        if (!string.IsNullOrEmpty(distributorId))
        {
            query = query.Where(o => o.DistributorId.Equals(distributorId, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(o => o.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = query.Count();
        var orders = query
            .OrderByDescending(o => o.OrderTimestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult((orders, totalCount));
    }

    /// <inheritdoc />
    public Task<OrderCosts?> GetOrderCostsAsync(Guid orderId)
    {
        var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
        return Task.FromResult(order?.Costs);
    }

    /// <inheritdoc />
    public Task<(List<Product> Products, int TotalCount)> GetProductsAsync(ProductQuery query)
    {
        var products = _products.AsEnumerable();

        if (!string.IsNullOrEmpty(query.Sku))
        {
            products = products.Where(p => p.Sku.Contains(query.Sku, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(query.StyleCode))
        {
            products = products.Where(p => p.StyleCode.Equals(query.StyleCode, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(query.BrandName))
        {
            products = products.Where(p => p.BrandName.Contains(query.BrandName, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(query.Color))
        {
            products = products.Where(p => p.Color.Equals(query.Color, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(query.Size))
        {
            products = products.Where(p => p.Size.Equals(query.Size, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(query.Gtin))
        {
            products = products.Where(p => p.Gtin.Equals(query.Gtin));
        }

        if (!string.IsNullOrEmpty(query.DistributorId))
        {
            products = products.Where(p => p.DistributorId.Equals(query.DistributorId, StringComparison.OrdinalIgnoreCase));
        }

        if (query.InStockOnly == true)
        {
            var inStockSkus = _inventory.Where(i => i.QuantityAvailable > 0).Select(i => i.Sku).Distinct();
            products = products.Where(p => inStockSkus.Contains(p.Sku));
        }

        var totalCount = products.Count();
        var result = products
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        return Task.FromResult((result, totalCount));
    }

    /// <inheritdoc />
    public Task<Product?> GetProductBySkuAsync(string sku, string? distributorId = null)
    {
        var product = _products.FirstOrDefault(p => 
            p.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase) &&
            (string.IsNullOrEmpty(distributorId) || p.DistributorId.Equals(distributorId, StringComparison.OrdinalIgnoreCase)));
        return Task.FromResult(product);
    }

    /// <inheritdoc />
    public Task<List<InventoryStock>> GetInventoryAsync(string sku, string? distributorId = null)
    {
        var inventory = _inventory
            .Where(i => i.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase) &&
                       (string.IsNullOrEmpty(distributorId) || i.DistributorId.Equals(distributorId, StringComparison.OrdinalIgnoreCase)))
            .ToList();
        return Task.FromResult(inventory);
    }

    /// <inheritdoc />
    public Task<List<Distributor>> GetDistributorsAsync()
    {
        return Task.FromResult(_distributors.ToList());
    }

    /// <inheritdoc />
    public Task<Distributor?> GetDistributorByIdAsync(string distributorId)
    {
        var distributor = _distributors.FirstOrDefault(d => 
            d.DistributorId.Equals(distributorId, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(distributor);
    }

    /// <inheritdoc />
    public Task<List<Warehouse>> GetWarehousesAsync(string distributorId)
    {
        var distributor = _distributors.FirstOrDefault(d => 
            d.DistributorId.Equals(distributorId, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(distributor?.Warehouses ?? new List<Warehouse>());
    }

    /// <inheritdoc />
    public Task<List<ShippingOption>> GetShippingOptionsAsync(string distributorId)
    {
        var distributor = _distributors.FirstOrDefault(d => 
            d.DistributorId.Equals(distributorId, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(distributor?.ShippingOptions ?? new List<ShippingOption>());
    }

    /// <inheritdoc />
    public Task<ShippingEstimate> GetShippingEstimateAsync(ShippingEstimateRequest request)
    {
        var options = new List<ShippingOptionEstimate>
        {
            new ShippingOptionEstimate
            {
                MethodCode = "1",
                MethodName = "Ground",
                Carrier = "UPS",
                EstimatedCost = 8.50m + (request.Items.Sum(i => i.Quantity) * 0.25m),
                EstimatedTransitDays = 5,
                EstimatedDeliveryDate = DateTime.UtcNow.AddDays(5)
            },
            new ShippingOptionEstimate
            {
                MethodCode = "2",
                MethodName = "Next Day Air",
                Carrier = "UPS",
                EstimatedCost = 25.00m + (request.Items.Sum(i => i.Quantity) * 0.75m),
                EstimatedTransitDays = 1,
                EstimatedDeliveryDate = DateTime.UtcNow.AddDays(1)
            },
            new ShippingOptionEstimate
            {
                MethodCode = "3",
                MethodName = "2nd Day Air",
                Carrier = "UPS",
                EstimatedCost = 18.00m + (request.Items.Sum(i => i.Quantity) * 0.50m),
                EstimatedTransitDays = 2,
                EstimatedDeliveryDate = DateTime.UtcNow.AddDays(2)
            }
        };

        return Task.FromResult(new ShippingEstimate
        {
            DistributorId = request.DistributorId,
            WarehouseCode = request.OriginWarehouseCode ?? "IL",
            DestinationZip = request.DestinationZip,
            Options = options,
            EstimatedAt = DateTime.UtcNow
        });
    }
}


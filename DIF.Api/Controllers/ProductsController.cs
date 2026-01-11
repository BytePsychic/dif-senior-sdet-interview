using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DIF.Api.Models.Domain;
using DIF.Api.Models.DTOs;
using DIF.Api.Models.Responses;
using DIF.Api.Services.Interfaces;

namespace DIF.Api.Controllers;

/// <summary>
/// Controller for product and inventory operations.
/// Handles product catalog queries and stock level checks.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IDistributorService _distributorService;
    private readonly IRateLimitService _rateLimitService;

    public ProductsController(
        IDistributorService distributorService,
        IRateLimitService rateLimitService)
    {
        _distributorService = distributorService;
        _rateLimitService = rateLimitService;
    }

    /// <summary>
    /// Gets products from the catalog with optional filtering.
    /// </summary>
    /// <param name="query">Product query parameters.</param>
    /// <returns>Paginated list of products.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<Product>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResponse<Product>>> GetProducts([FromQuery] ProductQueryDto query)
    {
        var productQuery = new ProductQuery
        {
            Sku = query.Sku,
            StyleCode = query.StyleCode,
            BrandName = query.BrandName,
            Gtin = query.Gtin,
            Color = query.Color,
            Size = query.Size,
            DistributorId = query.DistributorId,
            InStockOnly = query.InStockOnly,
            Page = query.Page,
            PageSize = query.PageSize
        };

        // Record request (P2 - Product data has lowest priority)
        if (!string.IsNullOrEmpty(query.DistributorId))
        {
            var canProceed = await _rateLimitService.CanMakeRequestAsync(query.DistributorId, RequestPriority.ProductData);
            if (canProceed)
            {
                await _rateLimitService.RecordRequestAsync(query.DistributorId);
            }
        }

        var (products, totalCount) = await _distributorService.GetProductsAsync(productQuery);

        return Ok(PaginatedResponse<Product>.Create(products, query.Page, query.PageSize, totalCount));
    }

    /// <summary>
    /// Gets a product by SKU.
    /// </summary>
    /// <param name="sku">SKU identifier.</param>
    /// <param name="distributorId">Optional distributor filter.</param>
    /// <returns>Product details.</returns>
    /// <response code="200">Product found.</response>
    /// <response code="404">Product not found.</response>
    [HttpGet("{sku}")]
    [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<Product>>> GetProduct(string sku, [FromQuery] string? distributorId = null)
    {
        var product = await _distributorService.GetProductBySkuAsync(sku, distributorId);
        
        if (product == null)
        {
            return NotFound(ApiResponse<Product>.Fail($"Product with SKU {sku} not found"));
        }

        return Ok(ApiResponse<Product>.Ok(product));
    }

    /// <summary>
    /// Gets inventory/stock levels for a SKU across warehouses.
    /// </summary>
    /// <param name="sku">SKU identifier.</param>
    /// <param name="distributorId">Optional distributor filter.</param>
    /// <returns>List of inventory records.</returns>
    [HttpGet("{sku}/inventory")]
    [ProducesResponseType(typeof(ApiResponse<List<InventoryStock>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<InventoryStock>>>> GetInventory(string sku, [FromQuery] string? distributorId = null)
    {
        var inventory = await _distributorService.GetInventoryAsync(sku, distributorId);

        if (inventory.Count == 0)
        {
            return Ok(ApiResponse<List<InventoryStock>>.Ok(inventory, $"No inventory found for SKU {sku}"));
        }

        var totalAvailable = 0;
        foreach (var stock in inventory)
        {
            totalAvailable += stock.QuantityAvailable;
        }

        return Ok(ApiResponse<List<InventoryStock>>.Ok(inventory, $"Total available: {totalAvailable} across {inventory.Count} warehouses"));
    }

    /// <summary>
    /// Gets inventory summary for multiple SKUs.
    /// </summary>
    /// <param name="skus">Comma-separated list of SKUs.</param>
    /// <param name="distributorId">Optional distributor filter.</param>
    /// <returns>Dictionary of SKU to inventory records.</returns>
    [HttpGet("inventory/batch")]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, List<InventoryStock>>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<Dictionary<string, List<InventoryStock>>>>> GetBatchInventory(
        [FromQuery] string skus,
        [FromQuery] string? distributorId = null)
    {
        var skuList = skus.Split(',', System.StringSplitOptions.RemoveEmptyEntries);
        var result = new Dictionary<string, List<InventoryStock>>();

        foreach (var sku in skuList)
        {
            var trimmedSku = sku.Trim();
            var inventory = await _distributorService.GetInventoryAsync(trimmedSku, distributorId);
            result[trimmedSku] = inventory;
        }

        return Ok(ApiResponse<Dictionary<string, List<InventoryStock>>>.Ok(result, $"Retrieved inventory for {skuList.Length} SKUs"));
    }
}


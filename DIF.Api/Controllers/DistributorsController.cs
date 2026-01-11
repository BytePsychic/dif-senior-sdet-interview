using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DIF.Api.Models.Domain;
using DIF.Api.Models.DTOs;
using DIF.Api.Models.Responses;
using DIF.Api.Services.Interfaces;

namespace DIF.Api.Controllers;

/// <summary>
/// Controller for distributor management operations.
/// Handles distributor info, warehouses, shipping options, and rate limit status.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DistributorsController : ControllerBase
{
    private readonly IDistributorService _distributorService;
    private readonly IRateLimitService _rateLimitService;

    public DistributorsController(
        IDistributorService distributorService,
        IRateLimitService rateLimitService)
    {
        _distributorService = distributorService;
        _rateLimitService = rateLimitService;
    }

    /// <summary>
    /// Gets all configured distributors.
    /// </summary>
    /// <returns>List of distributors.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<Distributor>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<Distributor>>>> GetDistributors()
    {
        var distributors = await _distributorService.GetDistributorsAsync();
        return Ok(ApiResponse<List<Distributor>>.Ok(distributors, $"Found {distributors.Count} distributors"));
    }

    /// <summary>
    /// Gets a distributor by ID.
    /// </summary>
    /// <param name="id">Distributor ID.</param>
    /// <returns>Distributor details.</returns>
    /// <response code="200">Distributor found.</response>
    /// <response code="404">Distributor not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<Distributor>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<Distributor>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<Distributor>>> GetDistributor(string id)
    {
        var distributor = await _distributorService.GetDistributorByIdAsync(id);
        
        if (distributor == null)
        {
            return NotFound(ApiResponse<Distributor>.Fail($"Distributor {id} not found"));
        }

        return Ok(ApiResponse<Distributor>.Ok(distributor));
    }

    /// <summary>
    /// Gets warehouses for a distributor.
    /// </summary>
    /// <param name="id">Distributor ID.</param>
    /// <returns>List of warehouses.</returns>
    [HttpGet("{id}/warehouses")]
    [ProducesResponseType(typeof(ApiResponse<List<Warehouse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<Warehouse>>>> GetWarehouses(string id)
    {
        var warehouses = await _distributorService.GetWarehousesAsync(id);
        return Ok(ApiResponse<List<Warehouse>>.Ok(warehouses, $"Found {warehouses.Count} warehouses for {id}"));
    }

    /// <summary>
    /// Gets available shipping options for a distributor.
    /// </summary>
    /// <param name="id">Distributor ID.</param>
    /// <returns>List of shipping options.</returns>
    [HttpGet("{id}/shipping-options")]
    [ProducesResponseType(typeof(ApiResponse<List<ShippingOption>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<ShippingOption>>>> GetShippingOptions(string id)
    {
        var options = await _distributorService.GetShippingOptionsAsync(id);
        return Ok(ApiResponse<List<ShippingOption>>.Ok(options, $"Found {options.Count} shipping options for {id}"));
    }

    /// <summary>
    /// Gets the current rate limit status for a distributor.
    /// </summary>
    /// <param name="id">Distributor ID.</param>
    /// <returns>Rate limit status.</returns>
    [HttpGet("{id}/rate-limit-status")]
    [ProducesResponseType(typeof(ApiResponse<RateLimitStatusResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<RateLimitStatusResponse>>> GetRateLimitStatus(string id)
    {
        var config = await _rateLimitService.GetRateLimitStatusAsync(id);

        var response = new RateLimitStatusResponse
        {
            DistributorId = config.DistributorId,
            DistributorName = config.DistributorName,
            RequestsPerMinute = config.RequestsPerMinute,
            CurrentRequestCount = config.CurrentRequestCount,
            RemainingRequests = config.RemainingRequests,
            IsApproachingLimit = config.IsApproachingLimit,
            IsRateLimited = config.IsRateLimited,
            SecondsUntilReset = (int)config.TimeUntilReset.TotalSeconds,
            QueueDepth = config.QueueDepth
        };

        return Ok(ApiResponse<RateLimitStatusResponse>.Ok(response));
    }

    /// <summary>
    /// Gets shipping estimate for an order.
    /// </summary>
    /// <param name="id">Distributor ID.</param>
    /// <param name="request">Shipping estimate request.</param>
    /// <returns>Shipping estimate with options.</returns>
    [HttpPost("{id}/shipping-estimate")]
    [ProducesResponseType(typeof(ApiResponse<ShippingEstimate>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ShippingEstimate>>> GetShippingEstimate(
        string id,
        [FromBody] ShippingEstimateRequestDto request)
    {
        var estimateRequest = new ShippingEstimateRequest
        {
            DistributorId = id,
            OriginWarehouseCode = request.OriginWarehouseCode,
            DestinationZip = request.DestinationZip,
            Items = request.Items.ConvertAll(i => new ShippingEstimateItem
            {
                Sku = i.Sku,
                Quantity = i.Quantity
            }),
            PreferredShippingMethod = request.PreferredShippingMethod
        };

        var estimate = await _distributorService.GetShippingEstimateAsync(estimateRequest);
        return Ok(ApiResponse<ShippingEstimate>.Ok(estimate));
    }
}


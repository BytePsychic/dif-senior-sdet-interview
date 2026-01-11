using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DIF.Api.Models.Domain;
using DIF.Api.Models.DTOs;
using DIF.Api.Models.Responses;
using DIF.Api.Services.Interfaces;

namespace DIF.Api.Controllers;

/// <summary>
/// Controller for order management operations.
/// Handles order placement, retrieval, and cost queries.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly IDistributorService _distributorService;
    private readonly IRateLimitService _rateLimitService;

    public OrdersController(
        IDistributorService distributorService,
        IRateLimitService rateLimitService)
    {
        _distributorService = distributorService;
        _rateLimitService = rateLimitService;
    }

    /// <summary>
    /// Places a new order with a distributor.
    /// </summary>
    /// <param name="request">Order placement request.</param>
    /// <returns>The created order details.</returns>
    /// <response code="201">Order placed successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="429">Rate limit exceeded.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<OrderResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<OrderResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<OrderResponse>), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<ApiResponse<OrderResponse>>> PlaceOrder([FromBody] PlaceOrderRequest request)
    {
        // Check rate limit (P0 - Order placement has highest priority)
        var canProceed = await _rateLimitService.CanMakeRequestAsync(request.DistributorId, RequestPriority.OrderPlacement);
        if (!canProceed)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, 
                ApiResponse<OrderResponse>.Fail("Rate limit exceeded. Request has been queued."));
        }

        // Record the request
        await _rateLimitService.RecordRequestAsync(request.DistributorId);

        // Place the order
        var order = await _distributorService.PlaceOrderAsync(request);

        var response = new OrderResponse
        {
            OrderId = order.OrderId,
            DistributorOrderId = order.DistributorOrderId,
            PoNumber = order.PoNumber,
            Status = order.Status,
            WarehouseCode = order.WarehouseCode,
            WarehouseName = order.WarehouseName,
            ExpectedDeliveryDate = order.ExpectedDeliveryDate,
            OrderTimestamp = order.OrderTimestamp,
            Costs = order.Costs != null ? new OrderCostResponse
            {
                Subtotal = order.Costs.Subtotal,
                Shipping = order.Costs.Shipping,
                Tax = order.Costs.Tax,
                SmallOrderFee = order.Costs.SmallOrderFee,
                Total = order.Costs.Total
            } : null
        };

        return CreatedAtAction(
            nameof(GetOrder), 
            new { orderId = order.OrderId }, 
            ApiResponse<OrderResponse>.Ok(response, "Order placed successfully"));
    }

    /// <summary>
    /// Gets an order by its ID.
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    /// <returns>Order details.</returns>
    /// <response code="200">Order found.</response>
    /// <response code="404">Order not found.</response>
    [HttpGet("{orderId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Order>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<Order>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<Order>>> GetOrder(Guid orderId)
    {
        var order = await _distributorService.GetOrderByIdAsync(orderId);
        
        if (order == null)
        {
            return NotFound(ApiResponse<Order>.Fail($"Order with ID {orderId} not found"));
        }

        return Ok(ApiResponse<Order>.Ok(order));
    }

    /// <summary>
    /// Gets a list of orders with optional filtering.
    /// </summary>
    /// <param name="distributorId">Filter by distributor ID (optional).</param>
    /// <param name="status">Filter by status (optional).</param>
    /// <param name="page">Page number (default 1).</param>
    /// <param name="pageSize">Page size (default 50).</param>
    /// <returns>Paginated list of orders.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<Order>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResponse<Order>>> GetOrders(
        [FromQuery] string? distributorId = null,
        [FromQuery] string? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var (orders, totalCount) = await _distributorService.GetOrdersAsync(distributorId, status, page, pageSize);

        return Ok(PaginatedResponse<Order>.Create(orders, page, pageSize, totalCount));
    }

    /// <summary>
    /// Gets the cost breakdown for an order.
    /// </summary>
    /// <param name="orderId">Order ID.</param>
    /// <returns>Order cost details.</returns>
    /// <response code="200">Costs found.</response>
    /// <response code="404">Order not found.</response>
    [HttpGet("{orderId:guid}/costs")]
    [ProducesResponseType(typeof(ApiResponse<OrderCosts>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<OrderCosts>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<OrderCosts>>> GetOrderCosts(Guid orderId)
    {
        var costs = await _distributorService.GetOrderCostsAsync(orderId);
        
        if (costs == null)
        {
            return NotFound(ApiResponse<OrderCosts>.Fail($"Costs for order {orderId} not found"));
        }

        return Ok(ApiResponse<OrderCosts>.Ok(costs));
    }

    /// <summary>
    /// Gets an order by distributor order ID.
    /// </summary>
    /// <param name="distributorOrderId">Distributor's order ID.</param>
    /// <returns>Order details.</returns>
    [HttpGet("by-distributor-id/{distributorOrderId}")]
    [ProducesResponseType(typeof(ApiResponse<Order>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<Order>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<Order>>> GetOrderByDistributorId(string distributorOrderId)
    {
        var order = await _distributorService.GetOrderByDistributorIdAsync(distributorOrderId);
        
        if (order == null)
        {
            return NotFound(ApiResponse<Order>.Fail($"Order with distributor ID {distributorOrderId} not found"));
        }

        return Ok(ApiResponse<Order>.Ok(order));
    }
}


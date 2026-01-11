using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DIF.Api.Models.Domain;
using DIF.Api.Models.Responses;
using DIF.Api.Services.Interfaces;

namespace DIF.Api.Controllers;

/// <summary>
/// Controller for health check operations.
/// Provides API health status and distributor connectivity checks.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class HealthController : ControllerBase
{
    private readonly IDistributorService _distributorService;
    private readonly IAlertService _alertService;

    public HealthController(
        IDistributorService distributorService,
        IAlertService alertService)
    {
        _distributorService = distributorService;
        _alertService = alertService;
    }

    /// <summary>
    /// Gets the overall API health status.
    /// </summary>
    /// <returns>Health status.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<HealthResponse>> GetHealth()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";

        var response = new HealthResponse
        {
            Status = "Healthy",
            Version = version,
            Timestamp = DateTime.UtcNow,
            Components = new Dictionary<string, ComponentHealth>
            {
                ["api"] = new ComponentHealth
                {
                    Status = "Healthy",
                    Message = "API is operational",
                    LastSuccessful = DateTime.UtcNow
                },
                ["mockServices"] = new ComponentHealth
                {
                    Status = "Healthy",
                    Message = "Mock services initialized",
                    LastSuccessful = DateTime.UtcNow
                }
            }
        };

        // Check for any critical errors in the last hour
        var errorCounts = await _alertService.GetErrorCountsBySeverityAsync(1);
        if (errorCounts.TryGetValue(ErrorSeverity.Critical, out var criticalCount) && criticalCount > 0)
        {
            response.Status = "Degraded";
            response.Components["alerts"] = new ComponentHealth
            {
                Status = "Warning",
                Message = $"{criticalCount} critical errors in the last hour"
            };
        }
        else
        {
            response.Components["alerts"] = new ComponentHealth
            {
                Status = "Healthy",
                Message = "No critical errors"
            };
        }

        return Ok(response);
    }

    /// <summary>
    /// Gets the health status of all distributor connections.
    /// </summary>
    /// <returns>Distributor health statuses.</returns>
    [HttpGet("distributors")]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, ComponentHealth>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<Dictionary<string, ComponentHealth>>>> GetDistributorHealth()
    {
        var distributors = await _distributorService.GetDistributorsAsync();
        var health = new Dictionary<string, ComponentHealth>();

        foreach (var distributor in distributors)
        {
            var status = distributor.HealthStatus switch
            {
                ApiHealthStatus.Healthy => "Healthy",
                ApiHealthStatus.Degraded => "Degraded",
                ApiHealthStatus.Unhealthy => "Unhealthy",
                ApiHealthStatus.NoApi => "NoApi",
                _ => "Unknown"
            };

            health[distributor.DistributorId] = new ComponentHealth
            {
                Status = status,
                Message = distributor.HasApiIntegration 
                    ? $"API integration active - {distributor.ApiVersion}"
                    : "No API integration available",
                LastSuccessful = distributor.LastSuccessfulConnection,
                ResponseTimeMs = distributor.HealthStatus == ApiHealthStatus.Healthy ? 150 : null
            };
        }

        return Ok(ApiResponse<Dictionary<string, ComponentHealth>>.Ok(health, $"Checked {health.Count} distributors"));
    }

    /// <summary>
    /// Gets API error statistics.
    /// </summary>
    /// <param name="hours">Hours to look back (default 24).</param>
    /// <returns>Error statistics by severity.</returns>
    [HttpGet("errors")]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<ErrorSeverity, int>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<Dictionary<ErrorSeverity, int>>>> GetErrorStats([FromQuery] int hours = 24)
    {
        var counts = await _alertService.GetErrorCountsBySeverityAsync(hours);
        return Ok(ApiResponse<Dictionary<ErrorSeverity, int>>.Ok(counts, $"Error counts for the last {hours} hours"));
    }

    /// <summary>
    /// Gets recent API errors.
    /// </summary>
    /// <param name="distributorId">Optional distributor filter.</param>
    /// <param name="severity">Optional severity filter.</param>
    /// <param name="hours">Hours to look back (default 24).</param>
    /// <returns>List of recent errors.</returns>
    [HttpGet("errors/recent")]
    [ProducesResponseType(typeof(ApiResponse<List<ApiError>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<ApiError>>>> GetRecentErrors(
        [FromQuery] string? distributorId = null,
        [FromQuery] ErrorSeverity? severity = null,
        [FromQuery] int hours = 24)
    {
        var errors = await _alertService.GetRecentErrorsAsync(distributorId, severity, hours);
        return Ok(ApiResponse<List<ApiError>>.Ok(errors, $"Found {errors.Count} errors in the last {hours} hours"));
    }

    /// <summary>
    /// Performs a simple ping check.
    /// </summary>
    /// <returns>Pong response.</returns>
    [HttpGet("ping")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public ActionResult<string> Ping()
    {
        return Ok("pong");
    }
}


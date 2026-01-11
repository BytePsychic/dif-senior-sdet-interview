using Microsoft.Extensions.DependencyInjection;
using DIF.Api.Services.Implementations;
using DIF.Api.Services.Interfaces;

namespace DIF.Api.Extensions;

/// <summary>
/// Extension methods for registering DIF services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all DIF services to the service collection.
    /// Uses mock implementations for all services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddDifServices(this IServiceCollection services)
    {
        // Register mock services as singletons to maintain state across requests
        services.AddSingleton<IDistributorService, MockDistributorService>();
        services.AddSingleton<ITrackingService, MockTrackingService>();
        services.AddSingleton<IRateLimitService, MockRateLimitService>();
        services.AddSingleton<IAlertService, MockAlertService>();

        return services;
    }

    /// <summary>
    /// Adds DIF services with Moq for more controlled mocking.
    /// This method demonstrates how to use Moq for custom mock behavior.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddDifServicesWithMoq(this IServiceCollection services)
    {
        // For this implementation, we use concrete mock classes instead of Moq
        // because they provide realistic data behavior.
        // 
        // To use Moq instead, you would do:
        //
        // var mockDistributorService = new Mock<IDistributorService>();
        // mockDistributorService.Setup(x => x.PlaceOrderAsync(It.IsAny<PlaceOrderRequest>()))
        //     .ReturnsAsync((PlaceOrderRequest req) => MockDataFactory.CreateOrderFromRequest(req));
        // services.AddSingleton(mockDistributorService.Object);
        //
        // However, the concrete mock implementations are more maintainable
        // and provide consistent behavior across the API.

        return services.AddDifServices();
    }
}


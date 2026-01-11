using System;
using System.Collections.Generic;
using System.Linq;
using DIF.Api.Models.Domain;
using DIF.Api.Models.DTOs;

namespace DIF.Api.Mocks;

/// <summary>
/// Factory for generating realistic mock data for testing.
/// Creates sample orders, products, tracking, and distributor data.
/// </summary>
public static class MockDataFactory
{
    private static readonly Random _random = new();

    #region Distributors

    /// <summary>
    /// Creates the list of configured distributors.
    /// </summary>
    public static List<Distributor> CreateDistributors()
    {
        return new List<Distributor>
        {
            new Distributor
            {
                DistributorId = "ss",
                Name = "S&amp;S Activewear (Alpha Broder)",
                Code = "SS",
                ApiBaseUrl = "https://api.ssactivewear.com/v2",
                HasApiIntegration = true,
                IsActive = true,
                ApiVersion = "v2",
                HealthStatus = ApiHealthStatus.Healthy,
                RateLimitConfig = CreateRateLimitConfig("ss"),
                ShippingOptions = CreateSSShippingOptions(),
                Warehouses = CreateSSWarehouses(),
                ContactEmail = "api-support@ssactivewear.com",
                LastSuccessfulConnection = DateTime.UtcNow.AddMinutes(-5)
            },
            new Distributor
            {
                DistributorId = "img",
                Name = "IMG (Imageware)",
                Code = "IMG",
                ApiBaseUrl = "https://api.imgwear.com/v1",
                HasApiIntegration = true,
                IsActive = true,
                ApiVersion = "v1",
                HealthStatus = ApiHealthStatus.Healthy
            },
            new Distributor
            {
                DistributorId = "sanmar",
                Name = "SanMar",
                Code = "SANMAR",
                ApiBaseUrl = "https://api.sanmar.com/v1",
                HasApiIntegration = true,
                IsActive = true,
                ApiVersion = "v1",
                HealthStatus = ApiHealthStatus.Healthy
            },
            new Distributor
            {
                DistributorId = "staton",
                Name = "Staton Corporate",
                Code = "STATON",
                ApiBaseUrl = "https://api.statoncorp.com/v1",
                HasApiIntegration = true,
                IsActive = true,
                ApiVersion = "v1",
                HealthStatus = ApiHealthStatus.Healthy
            },
            new Distributor
            {
                DistributorId = "carolina",
                Name = "Carolina Made",
                Code = "CAROLINA",
                ApiBaseUrl = "https://api.carolinamade.com/v1",
                HasApiIntegration = true,
                IsActive = true,
                ApiVersion = "v1",
                HealthStatus = ApiHealthStatus.Healthy
            },
            new Distributor
            {
                DistributorId = "laapparel",
                Name = "LA Apparel",
                Code = "LAAPPAREL",
                HasApiIntegration = false,
                IsActive = true,
                HealthStatus = ApiHealthStatus.NoApi,
                Notes = "No API available - requires manual portal ordering or AI agent integration"
            },
            new Distributor
            {
                DistributorId = "drivingimpressions",
                Name = "Driving Impressions",
                Code = "DI",
                HasApiIntegration = false,
                IsActive = true,
                HealthStatus = ApiHealthStatus.NoApi,
                Notes = "No API available - requires manual portal ordering or AI agent integration"
            }
        };
    }

    /// <summary>
    /// Creates rate limit config for a distributor.
    /// </summary>
    public static RateLimitConfig CreateRateLimitConfig(string distributorId)
    {
        return new RateLimitConfig
        {
            DistributorId = distributorId,
            DistributorName = distributorId == "ss" ? "S&amp;S Activewear" : distributorId.ToUpper(),
            RequestsPerMinute = 60, // S&amp;S hard limit
            ThresholdPercentage = 90, // 54 req/min threshold
            BurstAllowance = 5,
            CurrentRequestCount = _random.Next(0, 30),
            WindowStart = DateTime.UtcNow.AddSeconds(-_random.Next(0, 60)),
            QueueDepth = _random.Next(0, 10)
        };
    }

    /// <summary>
    /// Creates S&amp;S shipping options.
    /// </summary>
    public static List<ShippingOption> CreateSSShippingOptions()
    {
        return new List<ShippingOption>
        {
            new ShippingOption
            {
                ShippingOptionId = Guid.NewGuid(),
                MethodCode = "1",
                MethodName = "Ground",
                Carrier = "UPS",
                EstimatedTransitDays = 5,
                DistributorId = "ss",
                IsAvailable = true,
                Description = "UPS Ground - 3-5 business days"
            },
            new ShippingOption
            {
                ShippingOptionId = Guid.NewGuid(),
                MethodCode = "2",
                MethodName = "Next Day Air",
                Carrier = "UPS",
                EstimatedTransitDays = 1,
                DistributorId = "ss",
                IsAvailable = true,
                Description = "UPS Next Day Air - 1 business day"
            },
            new ShippingOption
            {
                ShippingOptionId = Guid.NewGuid(),
                MethodCode = "3",
                MethodName = "2nd Day Air",
                Carrier = "UPS",
                EstimatedTransitDays = 2,
                DistributorId = "ss",
                IsAvailable = true,
                Description = "UPS 2nd Day Air - 2 business days"
            },
            new ShippingOption
            {
                ShippingOptionId = Guid.NewGuid(),
                MethodCode = "4",
                MethodName = "3 Day Select",
                Carrier = "UPS",
                EstimatedTransitDays = 3,
                DistributorId = "ss",
                IsAvailable = true,
                Description = "UPS 3 Day Select - 3 business days"
            }
        };
    }

    /// <summary>
    /// Creates S&amp;S warehouses.
    /// </summary>
    public static List<Warehouse> CreateSSWarehouses()
    {
        return new List<Warehouse>
        {
            new Warehouse
            {
                WarehouseId = Guid.NewGuid(),
                WarehouseCode = "IL",
                WarehouseName = "S&amp;S Activewear - Bolingbrook, IL",
                Address = "400 Remington Blvd",
                City = "Bolingbrook",
                State = "IL",
                Zip = "60440",
                CutoffTime = new TimeSpan(16, 0, 0), // 4 PM CT
                Timezone = "America/Chicago",
                DistributorId = "ss",
                IsActive = true
            },
            new Warehouse
            {
                WarehouseId = Guid.NewGuid(),
                WarehouseCode = "CA",
                WarehouseName = "S&amp;S Activewear - Ontario, CA",
                Address = "1350 E 6th St",
                City = "Ontario",
                State = "CA",
                Zip = "91761",
                CutoffTime = new TimeSpan(16, 0, 0), // 4 PM PT
                Timezone = "America/Los_Angeles",
                DistributorId = "ss",
                IsActive = true
            },
            new Warehouse
            {
                WarehouseId = Guid.NewGuid(),
                WarehouseCode = "KS",
                WarehouseName = "S&amp;S Activewear - Kansas City, KS",
                Address = "8101 Lenexa Dr",
                City = "Lenexa",
                State = "KS",
                Zip = "66214",
                CutoffTime = new TimeSpan(16, 0, 0), // 4 PM CT
                Timezone = "America/Chicago",
                DistributorId = "ss",
                IsActive = true
            },
            new Warehouse
            {
                WarehouseId = Guid.NewGuid(),
                WarehouseCode = "GA",
                WarehouseName = "S&amp;S Activewear - Atlanta, GA",
                Address = "6330 Sugarloaf Pkwy",
                City = "Duluth",
                State = "GA",
                Zip = "30097",
                CutoffTime = new TimeSpan(16, 0, 0), // 4 PM ET
                Timezone = "America/New_York",
                DistributorId = "ss",
                IsActive = true
            }
        };
    }

    #endregion

    #region Products

    /// <summary>
    /// Creates sample products.
    /// </summary>
    public static List<Product> CreateProducts()
    {
        return new List<Product>
        {
            CreateProduct("G500", "Gildan", "Heavy Cotton T-Shirt", "Black", "M", 3.25m),
            CreateProduct("G500", "Gildan", "Heavy Cotton T-Shirt", "Black", "L", 3.25m),
            CreateProduct("G500", "Gildan", "Heavy Cotton T-Shirt", "Black", "XL", 3.25m),
            CreateProduct("G500", "Gildan", "Heavy Cotton T-Shirt", "White", "M", 2.95m),
            CreateProduct("G500", "Gildan", "Heavy Cotton T-Shirt", "White", "L", 2.95m),
            CreateProduct("G500", "Gildan", "Heavy Cotton T-Shirt", "Navy", "M", 3.25m),
            CreateProduct("G500", "Gildan", "Heavy Cotton T-Shirt", "Navy", "L", 3.25m),
            CreateProduct("BC3001", "Bella+Canvas", "Unisex Jersey Tee", "Black", "M", 4.50m),
            CreateProduct("BC3001", "Bella+Canvas", "Unisex Jersey Tee", "Black", "L", 4.50m),
            CreateProduct("BC3001", "Bella+Canvas", "Unisex Jersey Tee", "White", "M", 4.25m),
            CreateProduct("BC3001", "Bella+Canvas", "Unisex Jersey Tee", "White", "L", 4.25m),
            CreateProduct("PC61", "Port & Company", "Essential Tee", "Black", "M", 2.75m),
            CreateProduct("PC61", "Port & Company", "Essential Tee", "Black", "L", 2.75m),
            CreateProduct("G185", "Gildan", "Heavy Blend Hoodie", "Black", "M", 12.50m),
            CreateProduct("G185", "Gildan", "Heavy Blend Hoodie", "Black", "L", 12.50m),
            CreateProduct("G185", "Gildan", "Heavy Blend Hoodie", "Navy", "M", 12.50m),
        };
    }

    private static Product CreateProduct(string styleCode, string brand, string styleName, string color, string size, decimal price)
    {
        var sku = $"{styleCode}-{color.Substring(0, 3).ToUpper()}-{size}";
        return new Product
        {
            ProductId = Guid.NewGuid(),
            Sku = sku,
            StyleCode = styleCode,
            StyleName = styleName,
            BrandName = brand,
            Gtin = GenerateGtin(),
            Color = color,
            ColorCode = color.Substring(0, 3).ToUpper(),
            Size = size,
            SizeCode = size,
            BlankCost = price,
            ImageUrl = $"https://images.ssactivewear.com/{styleCode}_{color.ToLower()}.jpg",
            Category = styleName.Contains("Hoodie") ? "Fleece" : "T-Shirts",
            Weight = styleName.Contains("Hoodie") ? 1.2m : 0.35m,
            DistributorId = "ss",
            IsActive = true,
            IsDiscontinued = false,
            LastUpdated = DateTime.UtcNow
        };
    }

    private static string GenerateGtin()
    {
        return $"00{_random.Next(100000000, 999999999):D9}";
    }

    /// <summary>
    /// Creates inventory for products.
    /// </summary>
    public static List<InventoryStock> CreateInventory(List<Product> products)
    {
        var warehouses = new[] { "IL", "CA", "KS", "GA" };
        var inventory = new List<InventoryStock>();

        foreach (var product in products)
        {
            foreach (var warehouse in warehouses)
            {
                inventory.Add(new InventoryStock
                {
                    StockId = Guid.NewGuid(),
                    Sku = product.Sku,
                    WarehouseCode = warehouse,
                    WarehouseName = $"S&amp;S - {warehouse}",
                    QuantityAvailable = _random.Next(0, 500),
                    QuantityReserved = _random.Next(0, 20),
                    QuantityOnBackorder = _random.Next(0, 5) == 0 ? _random.Next(10, 50) : 0,
                    DistributorId = "ss",
                    LastUpdated = DateTime.UtcNow
                });
            }
        }

        return inventory;
    }

    #endregion

    #region Orders

    /// <summary>
    /// Creates a sample order from a request.
    /// </summary>
    public static Order CreateOrderFromRequest(PlaceOrderRequest request)
    {
        var orderId = Guid.NewGuid();
        var distributorOrderId = $"SS{DateTime.UtcNow:yyyyMMdd}{_random.Next(10000, 99999)}";
        var warehouse = _random.Next(0, 4) switch
        {
            0 => ("IL", "S&amp;S Activewear - Bolingbrook, IL"),
            1 => ("CA", "S&amp;S Activewear - Ontario, CA"),
            2 => ("KS", "S&amp;S Activewear - Kansas City, KS"),
            _ => ("GA", "S&amp;S Activewear - Atlanta, GA")
        };

        var lines = request.Lines.Select(l => new OrderLine
        {
            Sku = l.Identifier,
            Gtin = GenerateGtin(),
            Quantity = l.Qty,
            QuantityShipped = l.Qty,
            Price = _random.Next(250, 1500) / 100m, // $2.50 - $15.00
            StyleCode = l.Identifier.Split('-')[0]
        }).ToList();

        var subtotal = lines.Sum(l => l.LineTotal);
        var shipping = Math.Round(8.50m + (lines.Sum(l => l.Quantity) * 0.25m), 2);
        var tax = Math.Round(subtotal * 0.07m, 2);

        return new Order
        {
            OrderId = orderId,
            DistributorOrderId = distributorOrderId,
            PoNumber = request.PoNumber,
            DistributorId = request.DistributorId,
            Lines = lines,
            ShippingAddress = new ShippingAddress
            {
                Customer = request.ShippingAddress.Customer,
                Address = request.ShippingAddress.Address,
                Address2 = request.ShippingAddress.Address2 ?? string.Empty,
                City = request.ShippingAddress.City,
                State = request.ShippingAddress.State,
                Zip = request.ShippingAddress.Zip,
                Country = request.ShippingAddress.Country
            },
            ShippingMethod = request.ShippingMethod,
            WarehouseCode = warehouse.Item1,
            WarehouseName = warehouse.Item2,
            OrderTimestamp = DateTime.UtcNow,
            ExpectedDeliveryDate = DateTime.UtcNow.AddDays(request.ShippingMethod == "1" ? 5 : request.ShippingMethod == "2" ? 1 : 2),
            Costs = new OrderCosts
            {
                OrderItemId = Guid.NewGuid(),
                Subtotal = subtotal,
                Shipping = shipping,
                Tax = tax,
                SmallOrderFee = subtotal < 50 ? 5.00m : null,
                BlankCostPerSku = lines.ToDictionary(l => l.Sku, l => l.Price),
                PaymentMethod = "CreditCard",
                WarehouseId = warehouse.Item1
            },
            Status = "Placed",
            SplitShipEnabled = request.AutoselectWarehouse,
            EmailConfirmation = request.EmailConfirmation ?? string.Empty,
            IsTestOrder = request.TestOrder,
            ShippingCarrier = "UPS",
            TotalBoxes = Math.Max(1, lines.Sum(l => l.Quantity) / 12),
            TotalWeight = lines.Sum(l => l.Quantity) * 0.35m,
            InternalGuid = Guid.NewGuid().ToString()
        };
    }

    /// <summary>
    /// Creates sample orders.
    /// </summary>
    public static List<Order> CreateSampleOrders(int count = 10)
    {
        var orders = new List<Order>();
        var statuses = new[] { "Placed", "Processing", "Shipped", "Delivered" };

        for (int i = 0; i < count; i++)
        {
            var request = new PlaceOrderRequest
            {
                DistributorId = "ss",
                ShippingAddress = new ShippingAddressDto
                {
                    Customer = $"Test Printer {i + 1}",
                    Address = $"{100 + i} Main Street",
                    City = "Chicago",
                    State = "IL",
                    Zip = $"6060{i}"
                },
                ShippingMethod = "1",
                AutoselectWarehouse = true,
                PoNumber = $"FP{DateTime.UtcNow:yyyyMMdd}{1000 + i}",
                Lines = new List<OrderLineDto>
                {
                    new OrderLineDto { Identifier = "G500-BLK-M", Qty = _random.Next(6, 24) },
                    new OrderLineDto { Identifier = "G500-BLK-L", Qty = _random.Next(6, 24) }
                }
            };

            var order = CreateOrderFromRequest(request);
            order.Status = statuses[_random.Next(statuses.Length)];
            order.OrderTimestamp = DateTime.UtcNow.AddDays(-_random.Next(0, 14));

            if (order.Status == "Shipped" || order.Status == "Delivered")
            {
                order.ShipDate = order.OrderTimestamp.AddDays(1);
                order.TrackingNumber = $"1Z{_random.Next(100, 999)}V{_random.Next(10000000, 99999999)}";
                order.DeliveryStatus = order.Status == "Delivered" ? "Delivered" : "In Transit";
            }

            orders.Add(order);
        }

        return orders;
    }

    #endregion

    #region Tracking

    /// <summary>
    /// Creates tracking data for an order.
    /// </summary>
    public static TrackingShipment CreateTrackingForOrder(Order order)
    {
        var trackingNumber = order.TrackingNumber ?? $"1Z{_random.Next(100, 999)}V{_random.Next(10000000, 99999999)}";
        var shipDate = order.ShipDate ?? order.OrderTimestamp.AddDays(1);

        return new TrackingShipment
        {
            ShipmentId = Guid.NewGuid(),
            OrderId = order.OrderId,
            PurchaseOrderNumber = order.PoNumber,
            TrackingNumber = trackingNumber,
            TrackingUrl = $"https://www.ups.com/track?tracknum={trackingNumber}",
            Carrier = "UPS",
            ShippingType = order.ShippingMethod == "1" ? "Ground" : "Air",
            LegType = "L1",
            NumBoxes = order.TotalBoxes,
            TotalWeight = order.TotalWeight,
            ShipDate = shipDate,
            EstimatedDelivery = shipDate.AddDays(order.ShippingMethod == "1" ? 5 : 2),
            OriginWarehouseZip = order.WarehouseCode == "IL" ? "60440" : order.WarehouseCode == "CA" ? "91761" : "66214",
            DestinationPrinterZip = order.ShippingAddress.Zip,
            CurrentStatus = order.Status == "Delivered" ? "Delivered" : order.Status == "Shipped" ? "In Transit" : "Label Created",
            StatusHistory = CreateStatusHistory(order.Status, shipDate),
            DeliveryConfirmed = order.Status == "Delivered",
            MisshipmentFlag = false,
            LastUpdated = DateTime.UtcNow,
            DistributorId = order.DistributorId,
            DistributorOrderId = order.DistributorOrderId
        };
    }

    private static List<StatusHistoryEntry> CreateStatusHistory(string currentStatus, DateTime shipDate)
    {
        var history = new List<StatusHistoryEntry>
        {
            new StatusHistoryEntry
            {
                Status = "Label Created",
                Timestamp = shipDate,
                Location = "Bolingbrook, IL",
                Description = "Shipping label created"
            }
        };

        if (currentStatus != "Placed" && currentStatus != "Processing")
        {
            history.Add(new StatusHistoryEntry
            {
                Status = "Picked Up",
                Timestamp = shipDate.AddHours(4),
                Location = "Bolingbrook, IL",
                Description = "Package picked up by carrier"
            });

            history.Add(new StatusHistoryEntry
            {
                Status = "In Transit",
                Timestamp = shipDate.AddDays(1),
                Location = "Hodgkins, IL",
                Description = "In transit to destination"
            });
        }

        if (currentStatus == "Delivered")
        {
            history.Add(new StatusHistoryEntry
            {
                Status = "Out for Delivery",
                Timestamp = shipDate.AddDays(3).AddHours(8),
                Location = "Chicago, IL",
                Description = "Out for delivery"
            });

            history.Add(new StatusHistoryEntry
            {
                Status = "Delivered",
                Timestamp = shipDate.AddDays(3).AddHours(14),
                Location = "Chicago, IL",
                Description = "Delivered - Signed by: J. SMITH"
            });
        }

        return history;
    }

    /// <summary>
    /// Creates sample tracking shipments.
    /// </summary>
    public static List<TrackingShipment> CreateSampleTracking(int count = 10)
    {
        var orders = CreateSampleOrders(count);
        return orders.Select(CreateTrackingForOrder).ToList();
    }

    /// <summary>
    /// Creates a delivery confirmation.
    /// </summary>
    public static DeliveryConfirmation CreateDeliveryConfirmation(TrackingShipment shipment)
    {
        return new DeliveryConfirmation
        {
            ShipmentId = shipment.ShipmentId,
            DeliveryDateTime = shipment.ActualDeliveryDate ?? DateTime.UtcNow,
            BoxesDelivered = shipment.NumBoxes,
            WeightDelivered = shipment.TotalWeight,
            DeliveryLocation = shipment.DestinationPrinterZip,
            SignedBy = "J. SMITH",
            BoxCountMismatch = false,
            WeightMismatch = false,
            ExpectedBoxes = shipment.NumBoxes,
            ExpectedWeight = shipment.TotalWeight
        };
    }

    #endregion

    #region Errors and Alerts

    /// <summary>
    /// Creates a sample API error.
    /// </summary>
    public static ApiError CreateSampleError(ErrorSeverity severity = ErrorSeverity.Warning)
    {
        return new ApiError
        {
            ErrorId = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            Distributor = "ss",
            Endpoint = "/v2/orders/",
            HttpMethod = "POST",
            HttpStatusCode = severity == ErrorSeverity.Critical ? 500 : 429,
            ErrorType = severity == ErrorSeverity.Critical ? "ServerError" : "RateLimit",
            Severity = severity,
            ErrorMessage = severity == ErrorSeverity.Critical 
                ? "Internal Server Error" 
                : "Too Many Requests - Rate limit exceeded",
            RetryCount = 0,
            LatencyMs = _random.Next(100, 5000),
            CorrelationId = Guid.NewGuid().ToString()
        };
    }

    #endregion
}


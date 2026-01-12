# Distributor Integration Framework (DIF) API

A .NET 8.0 Web API for managing distributor integrations, orders, tracking, and products. This project includes a comprehensive Java/Maven test automation framework for API testing.

## Project Overview

The Distributor Integration Framework (DIF) API provides a unified interface for interacting with multiple distributors including:

- **S&S Activewear** (Alpha Broder) - Full API integration
- **IMG** (Imageware) - Full API integration
- **SanMar** - Full API integration
- **Staton** - Full API integration
- **Carolina Made** - Full API integration
- **LA Apparel** - No API (manual ordering)
- **Driving Impressions** - No API (manual ordering)

### Key Features

- **Rate Limiting**: Intelligent request management to prevent rate limit violations
- **Standardized Schema**: Unified data models across all distributors
- **Real-time Tracking**: L1 shipment tracking from distributors to printers
- **Cost Tracking**: Complete order cost breakdown for financial reconciliation

### Technology Stack

- **API**: .NET 8.0 Web API with ASP.NET Core
- **Testing**: Java 17 with Maven, Rest Assured, TestNG, and Allure reporting
- **Documentation**: Swagger/OpenAPI

## Prerequisites

Before setting up the project, ensure you have the following installed:

- **.NET 8.0 SDK** - Required for running the API
  - Download from [Microsoft .NET Downloads](https://dotnet.microsoft.com/download)
  - Verify installation: `dotnet --version`
- **Java 17 JDK** - Required for running tests
  - Download from [Oracle JDK](https://www.oracle.com/java/technologies/javase/jdk17-archive-downloads.html) or [OpenJDK](https://adoptium.net/)
  - Verify installation: `java -version`
- **Maven 3.6+** - Required for test framework dependency management
  - Download from [Apache Maven](https://maven.apache.org/download.cgi)
  - Verify installation: `mvn -version`

### IDE Recommendations

- **Visual Studio 2022** or **VS Code** - For .NET API development
- **IntelliJ IDEA** - For Java test development
- **VS Code** - Works well for both with appropriate extensions

## API Setup Instructions

### 1. Navigate to API Directory

```bash
cd DIF.Api
```

### 2. Restore NuGet Packages

```bash
dotnet restore
```

This will download all required NuGet packages defined in `DIF.Api.csproj`.

### 3. Run the API

**Option 1: Default Configuration (Port 5000)**
```bash
dotnet run
```

**Option 2: Using Launch Profile (Port 5299)**
```bash
dotnet run --launch-profile http
```

The API will start and be available at:
- **Base URL**: `http://localhost:5000` (or `http://localhost:5299` with launch profile)
- **Swagger UI**: `http://localhost:5000` (root URL when running in Development mode)

### 4. Access Swagger UI

Once the API is running, open your browser and navigate to:
- `http://localhost:5000` - Swagger UI is served at the root in Development mode

Swagger UI provides interactive API documentation where you can:
- Explore all available endpoints
- View request/response schemas
- Test API calls directly from the browser

### 5. Configuration

The API configuration is located in `DIF.Api/appsettings.json`. Key settings include:

- **Kestrel Endpoints**: Configure HTTP port (default: 5000)
- **Distributor Settings**: Rate limits per distributor
  - S&S: 60 requests/minute (90% threshold)
  - IMG: 100 requests/minute (90% threshold)
  - SanMar: 120 requests/minute (90% threshold)
- **Alert Configuration**: Slack channels, email recipients, alert delays
- **Tracking Configuration**: Poll intervals, retry settings

## Test Framework Setup Instructions

### 1. Navigate to Test Directory

```bash
cd DIF.Api.Tests
```

### 2. Install Maven Dependencies

```bash
mvn clean install
```

Or to only resolve dependencies without running tests:

```bash
mvn dependency:resolve
```

This will download all dependencies defined in `pom.xml` including:
- Rest Assured (API testing)
- TestNG (test execution)
- AssertJ (fluent assertions)
- Allure (test reporting)
- Jackson (JSON serialization)
- Lombok (boilerplate reduction)

### 3. Run Tests

**Important**: Ensure the API is running before executing tests. The test framework verifies API accessibility in `BaseTest` before running any tests.

**Run All Tests**
```bash
mvn test
```

**Run Specific Test Class**
```bash
mvn test -Dtest=OrdersTests
mvn test -Dtest=ProductsTests
mvn test -Dtest=TrackingTests
mvn test -Dtest=DistributorsTests
mvn test -Dtest=HealthTests
```

**Run Specific Test Method**
```bash
mvn test -Dtest=OrdersTests#placeOrder_withValidRequest_returns201Created
```

**Run Tests by Group**
```bash
mvn test -Dgroups=smoke
```

### 4. Generate Allure Reports

**Generate Report**
```bash
mvn allure:report
```

The report will be generated in `target/site/allure-maven/index.html`

**View Report in Browser**
```bash
mvn allure:serve
```

This will automatically open the Allure report in your default browser.

**Manual Report Viewing**
Open `target/site/allure-maven/index.html` in your browser after generating the report.

### 5. Test Configuration

Test configuration is located in `src/test/resources/config.properties`:

```properties
base.url=http://localhost:5000
api.timeout=30000
log.request=true
log.response=true
default.distributor.id=ss
```

You can modify these settings to:
- Change the API base URL
- Adjust request/response timeout
- Enable/disable request/response logging
- Set default distributor ID for tests

## Project Structure

```
dif-senior-sdet-interview/
├── DIF.Api/                          # .NET 8.0 Web API Project
│   ├── Controllers/                  # API Controllers
│   │   ├── OrdersController.cs       # Order management endpoints
│   │   ├── ProductsController.cs     # Product catalog endpoints
│   │   ├── TrackingController.cs     # Shipment tracking endpoints
│   │   ├── DistributorsController.cs # Distributor information endpoints
│   │   └── HealthController.cs       # Health check endpoints
│   ├── Services/                     # Service implementations
│   │   ├── Interfaces/                # Service interfaces
│   │   └── Implementations/           # Mock service implementations
│   ├── Models/                       # Data models
│   │   ├── Domain/                   # Domain models (Order, Product, etc.)
│   │   ├── DTOs/                     # Data Transfer Objects
│   │   └── Responses/                # API response wrappers
│   ├── Mocks/                        # Mock data factory
│   ├── Extensions/                  # Extension methods
│   ├── Program.cs                    # Application entry point
│   ├── appsettings.json              # Application configuration
│   └── DIF.Api.csproj                # Project file
│
└── DIF.Api.Tests/                    # Java/Maven Test Framework
    ├── src/
    │   ├── main/java/                # Test utilities and API clients
    │   │   ├── com/dif/api/
    │   │   │   ├── client/           # API client classes
    │   │   │   ├── config/           # Configuration (ApiConfig)
    │   │   │   ├── models/           # Test data models
    │   │   │   └── builders/         # Test data builders
    │   │   └── resources/            # Test resources
    │   └── test/java/                 # Test classes
    │       └── com/dif/api/tests/
    │           ├── BaseTest.java     # Base test class
    │           ├── orders/           # Order tests
    │           ├── products/         # Product tests
    │           ├── tracking/         # Tracking tests
    │           ├── distributors/     # Distributor tests
    │           └── health/           # Health check tests
    ├── pom.xml                       # Maven configuration
    └── src/test/resources/           # Test configuration files
        └── config.properties         # Test configuration
```

## API Endpoints

The API provides endpoints organized by functional area. For complete interactive documentation, visit the Swagger UI at `http://localhost:5000` when the API is running.

### Main Controllers

#### Orders (`/api/orders`)
- `POST /api/orders` - Place a new order
- `GET /api/orders` - List orders with filtering
- `GET /api/orders/{orderId}` - Get order by ID
- `GET /api/orders/{orderId}/costs` - Get order cost breakdown
- `GET /api/orders/by-distributor-id/{distributorOrderId}` - Get order by distributor order ID

#### Products (`/api/products`)
- `GET /api/products` - List products with filtering
- `GET /api/products/{sku}` - Get product by SKU
- `GET /api/products/{sku}/inventory` - Get inventory for a SKU
- `GET /api/products/inventory/batch` - Get inventory for multiple SKUs

#### Tracking (`/api/tracking`)
- `GET /api/tracking/{orderId}` - Get tracking for an order
- `GET /api/tracking/shipment/{trackingNumber}` - Get shipment by tracking number
- `GET /api/tracking/{orderId}/all` - Get all tracking for an order
- `POST /api/tracking/update` - Trigger tracking status update
- `GET /api/tracking/{orderId}/delivery-confirmation` - Get delivery confirmation
- `GET /api/tracking/pending` - Get pending shipments
- `GET /api/tracking/misshipments` - Get misshipment alerts

#### Distributors (`/api/distributors`)
- `GET /api/distributors` - List all distributors
- `GET /api/distributors/{id}` - Get distributor by ID
- `GET /api/distributors/{id}/warehouses` - Get distributor warehouses
- `GET /api/distributors/{id}/shipping-options` - Get shipping options
- `GET /api/distributors/{id}/rate-limit-status` - Get rate limit status
- `POST /api/distributors/{id}/shipping-estimate` - Get shipping estimate

#### Health (`/api/health`)
- `GET /api/health` - API health check
- `GET /api/health/distributors` - Distributor health status
- `GET /api/health/errors` - Error statistics
- `GET /api/health/errors/recent` - Recent errors
- `GET /api/health/ping` - Simple ping check

For detailed request/response examples, see `DIF.Api/Docs/ApiExamples.md`.

## Development Notes

### Mock Implementations

All services use mock implementations for testing purposes. No external API calls are made, and all data is simulated using `MockDataFactory` with realistic patterns based on actual S&S API responses. This allows for:
- Fast, reliable testing without external dependencies
- Predictable test data
- No API keys or credentials required

### Rate Limiting

Rate limiting is configured per distributor in `appsettings.json`:

- **S&S Activewear**: 60 requests/minute (90% threshold = 54 req/min)
- **IMG**: 100 requests/minute (90% threshold = 90 req/min)
- **SanMar**: 120 requests/minute (90% threshold = 108 req/min)

Request priority: P0 (orders) > P1 (tracking) > P2 (products)

The API automatically queues requests when approaching rate limits.

### Environment Configuration

- **Development Mode**: Swagger UI is enabled and served at the root URL
- **Production Mode**: Swagger UI is disabled for security

Set the environment using:
- `ASPNETCORE_ENVIRONMENT=Development` (default in launchSettings.json)
- Or modify `appsettings.Development.json` for development-specific settings

### Test Data

Test data is generated using `MockDataFactory` with realistic patterns. The factory creates:
- Realistic order IDs and tracking numbers
- Proper warehouse codes and addresses
- Valid SKU formats
- Realistic pricing and inventory data

## Troubleshooting

### Port Conflicts

**Problem**: Port 5000 (or 5299) is already in use.

**Solutions**:
1. Change the port in `appsettings.json`:
   ```json
   "Kestrel": {
     "Endpoints": {
       "Http": {
         "Url": "http://localhost:5001"
       }
     }
   }
   ```
2. Update test configuration in `DIF.Api.Tests/src/test/resources/config.properties`:
   ```properties
   base.url=http://localhost:5001
   ```
3. Use a different launch profile that uses a different port

### Java Version Issues

**Problem**: Tests fail with Java version errors.

**Solution**:
- Ensure Java 17 is installed: `java -version`
- Set `JAVA_HOME` environment variable to point to Java 17 installation
- Verify Maven is using the correct Java version: `mvn -version`

### Maven Issues

**Problem**: Maven commands fail or dependencies don't download.

**Solutions**:
- Verify Maven installation: `mvn -version`
- Check `pom.xml` syntax for errors
- Clear Maven cache: `mvn dependency:purge-local-repository`
- Check internet connection for dependency downloads
- Verify Maven settings.xml if using a custom repository

### API Not Accessible

**Problem**: Tests fail because API is not accessible.

**Solutions**:
- Ensure API is running before executing tests
- Verify API is accessible: `curl http://localhost:5000/api/health/ping`
- Check that the base URL in `config.properties` matches the API URL
- Check firewall settings if running on different machines
- Verify the API is running in Development mode (Swagger should be accessible)

### Allure Reports Not Generating

**Problem**: Allure reports are empty or not generated.

**Solutions**:
- Ensure AspectJ weaver is properly configured (already in `pom.xml`)
- Run tests first: `mvn test` before `mvn allure:report`
- Check that `target/allure-results` directory contains test results
- Verify Allure Maven plugin version compatibility
- Try cleaning and rebuilding: `mvn clean test allure:report`

### NuGet Restore Failures

**Problem**: `dotnet restore` fails or packages don't download.

**Solutions**:
- Check internet connection
- Verify NuGet package sources: `dotnet nuget list source`
- Clear NuGet cache: `dotnet nuget locals all --clear`
- Check for proxy/firewall issues
- Verify `DIF.Api.csproj` has valid package references

### Swagger UI Not Loading

**Problem**: Swagger UI doesn't appear at root URL.

**Solutions**:
- Ensure API is running in Development mode
- Check `Program.cs` - Swagger is only enabled in Development
- Verify `ASPNETCORE_ENVIRONMENT=Development` is set
- Check browser console for errors
- Try accessing Swagger JSON directly: `http://localhost:5000/swagger/v1/swagger.json`

## Additional Resources

- **API Examples**: See `DIF.Api/Docs/ApiExamples.md` for detailed request/response examples
- **Progress Tracking**: See `DIF.Api/PROGRESS.md` for implementation status
- **Swagger Documentation**: Interactive API docs available at `http://localhost:5000` when running

## Support

For questions or issues, please refer to:
- API documentation in Swagger UI
- Test examples in `DIF.Api.Tests/src/test/java`
- Configuration files for customization options

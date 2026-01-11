using System.Reflection;
using DIF.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add DIF services (mock implementations)
builder.Services.AddDifServices();

// Configure Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Distributor Integration Framework API",
        Version = "v1",
        Description = @"
## Overview
API for managing distributor integrations, orders, tracking, and products.

This API provides a unified interface for interacting with multiple distributors including:
- **S&S Activewear** (Alpha Broder) - Full API integration
- **IMG** (Imageware) - Full API integration
- **SanMar** - Full API integration
- **Staton** - Full API integration  
- **Carolina Made** - Full API integration
- **LA Apparel** - No API (manual ordering)
- **Driving Impressions** - No API (manual ordering)

## Key Features
- **Rate Limiting**: Intelligent request management to prevent rate limit violations
- **Standardized Schema**: Unified data models across all distributors
- **Real-time Tracking**: L1 shipment tracking from distributors to printers
- **Cost Tracking**: Complete order cost breakdown for financial reconciliation

## Rate Limiting
The API implements rate limiting per distributor:
- S&S: 60 requests/minute (90% threshold = 54 req/min)
- Request priority: P0 (orders) > P1 (tracking) > P2 (products)
- Automatic queuing when approaching limits

## Error Handling
- **CRITICAL**: Order placement failures, auth issues, server errors → Immediate alerts
- **WARNING**: Rate limits, tracking failures → Batched alerts every 2 hours
- **INFO**: Validation errors → Logged only

## Mock Implementation
This API uses mock services for testing purposes. All data is simulated but follows realistic patterns based on actual S&S API responses.
",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "DIF Team",
            Email = "dif-team@freshprints.com"
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "Internal Use Only"
        }
    });

    // Include XML comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Group endpoints by controller
    options.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        if (api.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controllerActionDescriptor)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });

    options.DocInclusionPredicate((name, api) => true);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "DIF API v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at root
        options.DocumentTitle = "DIF API Documentation";
        options.DefaultModelsExpandDepth(2);
        options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        options.EnableFilter();
        options.ShowExtensions();
    });
}

app.UseAuthorization();
app.MapControllers();

// Add a startup log message
app.Logger.LogInformation("DIF API started. Swagger UI available at: {Url}", 
    app.Environment.IsDevelopment() ? "http://localhost:5000" : "Production URL");

app.Run();

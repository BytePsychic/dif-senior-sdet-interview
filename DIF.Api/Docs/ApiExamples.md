# DIF API - Request & Response Examples

This document contains sample JSON request and response objects for all endpoints in the Distributor Integration Framework API.

---

## Table of Contents

- [Orders Endpoints](#orders-endpoints)
- [Tracking Endpoints](#tracking-endpoints)
- [Products Endpoints](#products-endpoints)
- [Distributors Endpoints](#distributors-endpoints)
- [Health Endpoints](#health-endpoints)

---

## Orders Endpoints

### POST /api/orders - Place Order

**Request:**
```json
{
  "distributorId": "ss",
  "shippingAddress": {
    "customer": "Fresh Prints Printer #1",
    "address": "123 Main Street",
    "address2": "Suite 100",
    "city": "Chicago",
    "state": "IL",
    "zip": "60601",
    "country": "US",
    "phone": "312-555-1234"
  },
  "shippingMethod": "1",
  "autoselectWarehouse": true,
  "autoselectWarehouseWarehouses": ["IL", "KS"],
  "poNumber": "FP20260111001",
  "emailConfirmation": "orders@freshprints.com",
  "testOrder": false,
  "lines": [
    { "identifier": "G500-BLK-M", "qty": 12 },
    { "identifier": "G500-BLK-L", "qty": 24 }
  ],
  "paymentProfile": {
    "profileId": "",
    "useDefault": true
  }
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "data": {
    "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "distributorOrderId": "SS202601110001",
    "poNumber": "FP20260111001",
    "status": "Placed",
    "warehouseCode": "IL",
    "warehouseName": "S&S Activewear - Bolingbrook, IL",
    "expectedDeliveryDate": "2026-01-16T00:00:00Z",
    "orderTimestamp": "2026-01-11T14:30:00Z",
    "costs": {
      "subtotal": 117.00,
      "shipping": 12.50,
      "tax": 8.19,
      "smallOrderFee": null,
      "total": 137.69
    }
  },
  "message": "Order placed successfully",
  "errors": [],
  "timestamp": "2026-01-11T14:30:00Z",
  "correlationId": null
}
```

---

### GET /api/orders - List Orders

**Query Parameters:**
- `distributorId` (optional): Filter by distributor
- `status` (optional): Filter by status (Placed, Processing, Shipped, Delivered)
- `page` (optional, default: 1): Page number
- `pageSize` (optional, default: 50): Items per page

**Response (200 OK):**
```json
{
  "success": true,
  "items": [
    {
      "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "distributorOrderId": "SS202601110001",
      "poNumber": "FP20260111001",
      "distributorId": "ss",
      "status": "Shipped",
      "warehouseCode": "IL",
      "warehouseName": "S&S Activewear - Bolingbrook, IL",
      "orderTimestamp": "2026-01-11T14:30:00Z",
      "expectedDeliveryDate": "2026-01-16T00:00:00Z",
      "shippingCarrier": "UPS",
      "trackingNumber": "1Z999V123456789",
      "totalBoxes": 2,
      "totalWeight": 12.5
    },
    {
      "orderId": "7bc92f64-1234-5678-b3fc-9d852f66bfa7",
      "distributorOrderId": "SS202601100042",
      "poNumber": "FP20260110002",
      "distributorId": "ss",
      "status": "Delivered",
      "warehouseCode": "CA",
      "warehouseName": "S&S Activewear - Ontario, CA",
      "orderTimestamp": "2026-01-10T09:15:00Z",
      "expectedDeliveryDate": "2026-01-15T00:00:00Z",
      "shippingCarrier": "UPS",
      "trackingNumber": "1Z888V987654321",
      "totalBoxes": 1,
      "totalWeight": 6.2
    }
  ],
  "page": 1,
  "pageSize": 50,
  "totalItems": 150,
  "totalPages": 3,
  "hasNextPage": true,
  "hasPreviousPage": false,
  "message": "Success",
  "timestamp": "2026-01-11T15:00:00Z"
}
```

---

### GET /api/orders/{orderId} - Get Order by ID

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "distributorOrderId": "SS202601110001",
    "poNumber": "FP20260111001",
    "distributorId": "ss",
    "lines": [
      {
        "sku": "G500-BLK-M",
        "gtin": "00418402893",
        "quantity": 12,
        "quantityShipped": 12,
        "price": 3.25,
        "lineTotal": 39.00,
        "styleCode": "G500",
        "color": "Black",
        "size": "M"
      },
      {
        "sku": "G500-BLK-L",
        "gtin": "00174961356",
        "quantity": 24,
        "quantityShipped": 24,
        "price": 3.25,
        "lineTotal": 78.00,
        "styleCode": "G500",
        "color": "Black",
        "size": "L"
      }
    ],
    "shippingAddress": {
      "customer": "Fresh Prints Printer #1",
      "address": "123 Main Street",
      "address2": "Suite 100",
      "city": "Chicago",
      "state": "IL",
      "zip": "60601",
      "country": "US",
      "phone": "312-555-1234"
    },
    "shippingMethod": "1",
    "warehouseCode": "IL",
    "warehouseName": "S&S Activewear - Bolingbrook, IL",
    "orderTimestamp": "2026-01-11T14:30:00Z",
    "expectedDeliveryDate": "2026-01-16T00:00:00Z",
    "status": "Shipped",
    "splitShipEnabled": true,
    "shippingCarrier": "UPS",
    "totalBoxes": 2,
    "totalWeight": 12.5,
    "trackingNumber": "1Z999V123456789",
    "shipDate": "2026-01-12T08:00:00Z",
    "deliveryStatus": "In Transit"
  },
  "message": "Success",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

**Response (404 Not Found):**
```json
{
  "success": false,
  "data": null,
  "message": "Order with ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

### GET /api/orders/{orderId}/costs - Get Order Costs

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "orderItemId": "8ca92f64-1234-5678-b3fc-9d852f66bfa7",
    "subtotal": 117.00,
    "shipping": 12.50,
    "tax": 8.19,
    "smallOrderFee": null,
    "total": 137.69,
    "blankCostPerSku": {
      "G500-BLK-M": 3.25,
      "G500-BLK-L": 3.25
    },
    "paymentMethod": "CreditCard",
    "warehouseId": "IL",
    "cutoffDatetime": "2026-01-11T16:00:00Z",
    "surcharges": []
  },
  "message": "Success",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

### GET /api/orders/by-distributor-id/{distributorOrderId} - Get Order by Distributor ID

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "distributorOrderId": "SS202601110001",
    "poNumber": "FP20260111001",
    "status": "Shipped"
  },
  "message": "Success",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

## Tracking Endpoints

### GET /api/tracking/{orderId} - Get Tracking by Order ID

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "shipmentId": "9da85f64-5717-4562-b3fc-2c963f66afa6",
    "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "trackingNumber": "1Z999V123456789",
    "trackingUrl": "https://www.ups.com/track?tracknum=1Z999V123456789",
    "carrier": "UPS",
    "currentStatus": "In Transit",
    "shipDate": "2026-01-12T08:00:00Z",
    "estimatedDelivery": "2026-01-16T00:00:00Z",
    "actualDeliveryDate": null,
    "numBoxes": 2,
    "totalWeight": 12.5,
    "originWarehouse": "60440",
    "destination": "60601",
    "legType": "L1",
    "lastUpdated": "2026-01-13T10:30:00Z"
  },
  "message": "Success",
  "errors": [],
  "timestamp": "2026-01-13T10:30:00Z",
  "correlationId": null
}
```

---

### GET /api/tracking/shipment/{trackingNumber} - Get Tracking by Number

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "shipmentId": "9da85f64-5717-4562-b3fc-2c963f66afa6",
    "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "orderItemId": null,
    "purchaseOrderNumber": "FP20260111001",
    "trackingNumber": "1Z999V123456789",
    "trackingUrl": "https://www.ups.com/track?tracknum=1Z999V123456789",
    "carrier": "UPS",
    "shippingType": "Ground",
    "legType": "L1",
    "numBoxes": 2,
    "totalWeight": 12.5,
    "shipDate": "2026-01-12T08:00:00Z",
    "estimatedDelivery": "2026-01-16T00:00:00Z",
    "actualDeliveryDate": null,
    "originWarehouseZip": "60440",
    "originWarehouseAddress": "400 Remington Blvd, Bolingbrook, IL",
    "destinationPrinterZip": "60601",
    "destinationPrinterAddress": "123 Main Street, Chicago, IL",
    "currentStatus": "In Transit",
    "statusHistory": [
      {
        "status": "Label Created",
        "timestamp": "2026-01-12T06:00:00Z",
        "location": "Bolingbrook, IL",
        "description": "Shipping label created"
      },
      {
        "status": "Picked Up",
        "timestamp": "2026-01-12T10:00:00Z",
        "location": "Bolingbrook, IL",
        "description": "Package picked up by carrier"
      },
      {
        "status": "In Transit",
        "timestamp": "2026-01-13T08:00:00Z",
        "location": "Hodgkins, IL",
        "description": "In transit to destination"
      }
    ],
    "deliveryConfirmed": false,
    "misshipmentFlag": false,
    "boxesDelivered": null,
    "weightDelivered": null,
    "deliverySignature": "",
    "lastUpdated": "2026-01-13T10:30:00Z",
    "currentLocation": "Hodgkins, IL",
    "distributorId": "ss",
    "distributorOrderId": "SS202601110001"
  },
  "message": "Success",
  "errors": [],
  "timestamp": "2026-01-13T10:30:00Z",
  "correlationId": null
}
```

---

### GET /api/tracking/{orderId}/all - Get All Tracking for Order

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "shipmentId": "9da85f64-5717-4562-b3fc-2c963f66afa6",
      "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "trackingNumber": "1Z999V123456789",
      "carrier": "UPS",
      "currentStatus": "In Transit",
      "legType": "L1"
    },
    {
      "shipmentId": "8ca85f64-5717-4562-b3fc-2c963f66afa6",
      "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "trackingNumber": "1Z999V123456790",
      "carrier": "UPS",
      "currentStatus": "Delivered",
      "legType": "L1"
    }
  ],
  "message": "Success",
  "errors": [],
  "timestamp": "2026-01-13T10:30:00Z",
  "correlationId": null
}
```

---

### POST /api/tracking/update - Trigger Tracking Update

**Request:**
```json
{
  "orderIds": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
  "trackingNumbers": ["1Z999V123456789"],
  "distributorId": "ss",
  "statusFilter": ["In Transit", "Out for Delivery"]
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "shipmentsUpdated": 5,
    "updatedAt": "2026-01-13T10:30:00Z"
  },
  "message": "Updated 5 shipments",
  "errors": [],
  "timestamp": "2026-01-13T10:30:00Z",
  "correlationId": null
}
```

---

### GET /api/tracking/{orderId}/delivery-confirmation - Get Delivery Confirmation

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "shipmentId": "9da85f64-5717-4562-b3fc-2c963f66afa6",
    "deliveryDateTime": "2026-01-16T14:30:00Z",
    "boxesDelivered": 2,
    "weightDelivered": 12.5,
    "deliveryLocation": "60601",
    "signedBy": "J. SMITH",
    "boxCountMismatch": false,
    "weightMismatch": false,
    "expectedBoxes": 2,
    "expectedWeight": 12.5
  },
  "message": "Success",
  "errors": [],
  "timestamp": "2026-01-16T15:00:00Z",
  "correlationId": null
}
```

---

### GET /api/tracking/pending - Get Pending Shipments

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "shipmentId": "9da85f64-5717-4562-b3fc-2c963f66afa6",
      "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "trackingNumber": "1Z999V123456789",
      "carrier": "UPS",
      "currentStatus": "In Transit",
      "estimatedDelivery": "2026-01-16T00:00:00Z"
    },
    {
      "shipmentId": "7bc85f64-5717-4562-b3fc-2c963f66afa6",
      "orderId": "5ea85f64-5717-4562-b3fc-2c963f66afa6",
      "trackingNumber": "1Z888V987654321",
      "carrier": "UPS",
      "currentStatus": "Out for Delivery",
      "estimatedDelivery": "2026-01-13T00:00:00Z"
    }
  ],
  "message": "Found 2 pending shipments",
  "errors": [],
  "timestamp": "2026-01-13T10:30:00Z",
  "correlationId": null
}
```

---

### GET /api/tracking/misshipments - Get Misshipment Alerts

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "shipmentId": "6ab85f64-5717-4562-b3fc-2c963f66afa6",
      "orderId": "4da85f64-5717-4562-b3fc-2c963f66afa6",
      "trackingNumber": "1Z777V111222333",
      "currentStatus": "Delivered",
      "misshipmentFlag": true,
      "numBoxes": 3,
      "boxesDelivered": 2,
      "totalWeight": 18.5,
      "weightDelivered": 12.0
    }
  ],
  "message": "Found 1 misshipment alerts",
  "errors": [],
  "timestamp": "2026-01-13T10:30:00Z",
  "correlationId": null
}
```

---

## Products Endpoints

### GET /api/products - List Products

**Query Parameters:**
- `sku` (optional): Filter by SKU
- `styleCode` (optional): Filter by style code
- `brandName` (optional): Filter by brand
- `color` (optional): Filter by color
- `size` (optional): Filter by size
- `gtin` (optional): Filter by GTIN
- `distributorId` (optional): Filter by distributor
- `inStockOnly` (optional): Only return in-stock items
- `page` (optional, default: 1): Page number
- `pageSize` (optional, default: 50): Items per page

**Response (200 OK):**
```json
{
  "success": true,
  "items": [
    {
      "productId": "1ab85f64-5717-4562-b3fc-2c963f66afa6",
      "sku": "G500-BLK-M",
      "styleCode": "G500",
      "styleName": "Heavy Cotton T-Shirt",
      "brandName": "Gildan",
      "gtin": "00418402893",
      "color": "Black",
      "colorCode": "BLK",
      "size": "M",
      "sizeCode": "M",
      "imageUrl": "https://images.ssactivewear.com/G500_black.jpg",
      "blankCost": 3.25,
      "msrp": null,
      "description": "",
      "category": "T-Shirts",
      "weight": 0.35,
      "distributorId": "ss",
      "isActive": true,
      "isDiscontinued": false,
      "lastUpdated": "2026-01-11T00:00:00Z"
    },
    {
      "productId": "2bc85f64-5717-4562-b3fc-2c963f66afa6",
      "sku": "G500-BLK-L",
      "styleCode": "G500",
      "styleName": "Heavy Cotton T-Shirt",
      "brandName": "Gildan",
      "gtin": "00174961356",
      "color": "Black",
      "colorCode": "BLK",
      "size": "L",
      "sizeCode": "L",
      "imageUrl": "https://images.ssactivewear.com/G500_black.jpg",
      "blankCost": 3.25,
      "msrp": null,
      "description": "",
      "category": "T-Shirts",
      "weight": 0.35,
      "distributorId": "ss",
      "isActive": true,
      "isDiscontinued": false,
      "lastUpdated": "2026-01-11T00:00:00Z"
    }
  ],
  "page": 1,
  "pageSize": 50,
  "totalItems": 16,
  "totalPages": 1,
  "hasNextPage": false,
  "hasPreviousPage": false,
  "message": "Success",
  "timestamp": "2026-01-11T15:00:00Z"
}
```

---

### GET /api/products/{sku} - Get Product by SKU

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "productId": "1ab85f64-5717-4562-b3fc-2c963f66afa6",
    "sku": "G500-BLK-M",
    "styleCode": "G500",
    "styleName": "Heavy Cotton T-Shirt",
    "brandName": "Gildan",
    "gtin": "00418402893",
    "color": "Black",
    "colorCode": "BLK",
    "size": "M",
    "sizeCode": "M",
    "imageUrl": "https://images.ssactivewear.com/G500_black.jpg",
    "blankCost": 3.25,
    "msrp": null,
    "description": "",
    "category": "T-Shirts",
    "weight": 0.35,
    "distributorId": "ss",
    "isActive": true,
    "isDiscontinued": false,
    "lastUpdated": "2026-01-11T00:00:00Z"
  },
  "message": "Success",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

### GET /api/products/{sku}/inventory - Get Inventory for SKU

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "stockId": "3cd85f64-5717-4562-b3fc-2c963f66afa6",
      "sku": "G500-BLK-M",
      "warehouseCode": "IL",
      "warehouseName": "S&S - IL",
      "quantityAvailable": 450,
      "quantityReserved": 12,
      "quantityOnBackorder": 0,
      "inStock": true,
      "expectedRestockDate": null,
      "distributorId": "ss",
      "lastUpdated": "2026-01-11T12:00:00Z"
    },
    {
      "stockId": "4de85f64-5717-4562-b3fc-2c963f66afa6",
      "sku": "G500-BLK-M",
      "warehouseCode": "CA",
      "warehouseName": "S&S - CA",
      "quantityAvailable": 325,
      "quantityReserved": 5,
      "quantityOnBackorder": 0,
      "inStock": true,
      "expectedRestockDate": null,
      "distributorId": "ss",
      "lastUpdated": "2026-01-11T12:00:00Z"
    },
    {
      "stockId": "5ef85f64-5717-4562-b3fc-2c963f66afa6",
      "sku": "G500-BLK-M",
      "warehouseCode": "KS",
      "warehouseName": "S&S - KS",
      "quantityAvailable": 0,
      "quantityReserved": 0,
      "quantityOnBackorder": 50,
      "inStock": false,
      "expectedRestockDate": "2026-01-20T00:00:00Z",
      "distributorId": "ss",
      "lastUpdated": "2026-01-11T12:00:00Z"
    },
    {
      "stockId": "6fa85f64-5717-4562-b3fc-2c963f66afa6",
      "sku": "G500-BLK-M",
      "warehouseCode": "GA",
      "warehouseName": "S&S - GA",
      "quantityAvailable": 180,
      "quantityReserved": 0,
      "quantityOnBackorder": 0,
      "inStock": true,
      "expectedRestockDate": null,
      "distributorId": "ss",
      "lastUpdated": "2026-01-11T12:00:00Z"
    }
  ],
  "message": "Total available: 955 across 4 warehouses",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

### GET /api/products/inventory/batch - Get Batch Inventory

**Query Parameters:**
- `skus` (required): Comma-separated list of SKUs
- `distributorId` (optional): Filter by distributor

**Example:** `GET /api/products/inventory/batch?skus=G500-BLK-M,G500-BLK-L,BC3001-WHT-M`

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "G500-BLK-M": [
      {
        "stockId": "3cd85f64-5717-4562-b3fc-2c963f66afa6",
        "sku": "G500-BLK-M",
        "warehouseCode": "IL",
        "quantityAvailable": 450,
        "inStock": true
      },
      {
        "stockId": "4de85f64-5717-4562-b3fc-2c963f66afa6",
        "sku": "G500-BLK-M",
        "warehouseCode": "CA",
        "quantityAvailable": 325,
        "inStock": true
      }
    ],
    "G500-BLK-L": [
      {
        "stockId": "7ab85f64-5717-4562-b3fc-2c963f66afa6",
        "sku": "G500-BLK-L",
        "warehouseCode": "IL",
        "quantityAvailable": 380,
        "inStock": true
      }
    ],
    "BC3001-WHT-M": [
      {
        "stockId": "8bc85f64-5717-4562-b3fc-2c963f66afa6",
        "sku": "BC3001-WHT-M",
        "warehouseCode": "IL",
        "quantityAvailable": 120,
        "inStock": true
      }
    ]
  },
  "message": "Retrieved inventory for 3 SKUs",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

## Distributors Endpoints

### GET /api/distributors - List Distributors

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "distributorId": "ss",
      "name": "S&S Activewear (Alpha Broder)",
      "code": "SS",
      "apiBaseUrl": "https://api.ssactivewear.com/v2",
      "hasApiIntegration": true,
      "isActive": true,
      "apiVersion": "v2",
      "healthStatus": "Healthy",
      "contactEmail": "api-support@ssactivewear.com"
    },
    {
      "distributorId": "img",
      "name": "IMG (Imageware)",
      "code": "IMG",
      "apiBaseUrl": "https://api.imgwear.com/v1",
      "hasApiIntegration": true,
      "isActive": true,
      "apiVersion": "v1",
      "healthStatus": "Healthy"
    },
    {
      "distributorId": "sanmar",
      "name": "SanMar",
      "code": "SANMAR",
      "hasApiIntegration": true,
      "isActive": true,
      "healthStatus": "Healthy"
    },
    {
      "distributorId": "laapparel",
      "name": "LA Apparel",
      "code": "LAAPPAREL",
      "hasApiIntegration": false,
      "isActive": true,
      "healthStatus": "NoApi",
      "notes": "No API available - requires manual portal ordering"
    }
  ],
  "message": "Found 7 distributors",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

### GET /api/distributors/{id} - Get Distributor by ID

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "distributorId": "ss",
    "name": "S&S Activewear (Alpha Broder)",
    "code": "SS",
    "apiBaseUrl": "https://api.ssactivewear.com/v2",
    "hasApiIntegration": true,
    "isActive": true,
    "rateLimitConfig": {
      "distributorId": "ss",
      "distributorName": "S&S Activewear",
      "requestsPerMinute": 60,
      "thresholdPercentage": 90,
      "thresholdRequestCount": 54,
      "burstAllowance": 5,
      "currentRequestCount": 25,
      "remainingRequests": 35,
      "isApproachingLimit": false,
      "isRateLimited": false
    },
    "contactEmail": "api-support@ssactivewear.com",
    "apiVersion": "v2",
    "lastSuccessfulConnection": "2026-01-11T14:55:00Z",
    "healthStatus": "Healthy"
  },
  "message": "Success",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

### GET /api/distributors/{id}/warehouses - Get Distributor Warehouses

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "warehouseId": "de4d60c7-ddbe-4611-aeb2-bbc09adc351e",
      "warehouseCode": "IL",
      "warehouseName": "S&S Activewear - Bolingbrook, IL",
      "address": "400 Remington Blvd",
      "city": "Bolingbrook",
      "state": "IL",
      "zip": "60440",
      "country": "US",
      "cutoffTime": "16:00:00",
      "timezone": "America/Chicago",
      "distributorId": "ss",
      "isActive": true
    },
    {
      "warehouseId": "792b628f-b492-4fa5-ad78-598e3c68f1ab",
      "warehouseCode": "CA",
      "warehouseName": "S&S Activewear - Ontario, CA",
      "address": "1350 E 6th St",
      "city": "Ontario",
      "state": "CA",
      "zip": "91761",
      "country": "US",
      "cutoffTime": "16:00:00",
      "timezone": "America/Los_Angeles",
      "distributorId": "ss",
      "isActive": true
    },
    {
      "warehouseId": "e01e3e7a-6d95-4b42-9f2d-3a0ee45ed2fe",
      "warehouseCode": "KS",
      "warehouseName": "S&S Activewear - Kansas City, KS",
      "address": "8101 Lenexa Dr",
      "city": "Lenexa",
      "state": "KS",
      "zip": "66214",
      "country": "US",
      "cutoffTime": "16:00:00",
      "timezone": "America/Chicago",
      "distributorId": "ss",
      "isActive": true
    },
    {
      "warehouseId": "7d1f5cd7-deb0-418e-8e4e-bedeae4c59dd",
      "warehouseCode": "GA",
      "warehouseName": "S&S Activewear - Atlanta, GA",
      "address": "6330 Sugarloaf Pkwy",
      "city": "Duluth",
      "state": "GA",
      "zip": "30097",
      "country": "US",
      "cutoffTime": "16:00:00",
      "timezone": "America/New_York",
      "distributorId": "ss",
      "isActive": true
    }
  ],
  "message": "Found 4 warehouses for ss",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

### GET /api/distributors/{id}/shipping-options - Get Shipping Options

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "shippingOptionId": "cefdb7f1-521d-4f7c-9b4a-67220829d837",
      "methodCode": "1",
      "methodName": "Ground",
      "carrier": "UPS",
      "estimatedTransitDays": 5,
      "estimatedCost": null,
      "distributorId": "ss",
      "isAvailable": true,
      "description": "UPS Ground - 3-5 business days"
    },
    {
      "shippingOptionId": "e7f82bb1-9937-4bff-826c-48c675432758",
      "methodCode": "2",
      "methodName": "Next Day Air",
      "carrier": "UPS",
      "estimatedTransitDays": 1,
      "estimatedCost": null,
      "distributorId": "ss",
      "isAvailable": true,
      "description": "UPS Next Day Air - 1 business day"
    },
    {
      "shippingOptionId": "1eedbf30-9031-4c40-873d-394c1c6c6dcd",
      "methodCode": "3",
      "methodName": "2nd Day Air",
      "carrier": "UPS",
      "estimatedTransitDays": 2,
      "estimatedCost": null,
      "distributorId": "ss",
      "isAvailable": true,
      "description": "UPS 2nd Day Air - 2 business days"
    },
    {
      "shippingOptionId": "61170f6b-546c-4d13-8be9-7026902ba9ec",
      "methodCode": "4",
      "methodName": "3 Day Select",
      "carrier": "UPS",
      "estimatedTransitDays": 3,
      "estimatedCost": null,
      "distributorId": "ss",
      "isAvailable": true,
      "description": "UPS 3 Day Select - 3 business days"
    }
  ],
  "message": "Found 4 shipping options for ss",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

### GET /api/distributors/{id}/rate-limit-status - Get Rate Limit Status

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "distributorId": "ss",
    "distributorName": "S&S Activewear",
    "requestsPerMinute": 60,
    "currentRequestCount": 25,
    "remainingRequests": 35,
    "isApproachingLimit": false,
    "isRateLimited": false,
    "secondsUntilReset": 42,
    "queueDepth": 0
  },
  "message": "Success",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

### POST /api/distributors/{id}/shipping-estimate - Get Shipping Estimate

**Request:**
```json
{
  "originWarehouseCode": "IL",
  "destinationZip": "90210",
  "items": [
    { "sku": "G500-BLK-M", "quantity": 24 },
    { "sku": "G500-BLK-L", "quantity": 24 }
  ],
  "preferredShippingMethod": "1"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "distributorId": "ss",
    "warehouseCode": "IL",
    "destinationZip": "90210",
    "options": [
      {
        "methodCode": "1",
        "methodName": "Ground",
        "carrier": "UPS",
        "estimatedCost": 20.50,
        "estimatedTransitDays": 5,
        "estimatedDeliveryDate": "2026-01-16T00:00:00Z"
      },
      {
        "methodCode": "2",
        "methodName": "Next Day Air",
        "carrier": "UPS",
        "estimatedCost": 61.00,
        "estimatedTransitDays": 1,
        "estimatedDeliveryDate": "2026-01-12T00:00:00Z"
      },
      {
        "methodCode": "3",
        "methodName": "2nd Day Air",
        "carrier": "UPS",
        "estimatedCost": 42.00,
        "estimatedTransitDays": 2,
        "estimatedDeliveryDate": "2026-01-13T00:00:00Z"
      }
    ],
    "estimatedAt": "2026-01-11T15:00:00Z"
  },
  "message": "Success",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

## Health Endpoints

### GET /api/health - Health Check

**Response (200 OK):**
```json
{
  "status": "Healthy",
  "version": "1.0.0.0",
  "timestamp": "2026-01-11T15:00:00Z",
  "components": {
    "api": {
      "status": "Healthy",
      "message": "API is operational",
      "lastSuccessful": "2026-01-11T15:00:00Z",
      "responseTimeMs": null
    },
    "mockServices": {
      "status": "Healthy",
      "message": "Mock services initialized",
      "lastSuccessful": "2026-01-11T15:00:00Z",
      "responseTimeMs": null
    },
    "alerts": {
      "status": "Healthy",
      "message": "No critical errors",
      "lastSuccessful": null,
      "responseTimeMs": null
    }
  }
}
```

**Response (Degraded):**
```json
{
  "status": "Degraded",
  "version": "1.0.0.0",
  "timestamp": "2026-01-11T15:00:00Z",
  "components": {
    "api": {
      "status": "Healthy",
      "message": "API is operational",
      "lastSuccessful": "2026-01-11T15:00:00Z"
    },
    "alerts": {
      "status": "Warning",
      "message": "3 critical errors in the last hour"
    }
  }
}
```

---

### GET /api/health/distributors - Distributor Health Status

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "ss": {
      "status": "Healthy",
      "message": "API integration active - v2",
      "lastSuccessful": "2026-01-11T14:55:00Z",
      "responseTimeMs": 150
    },
    "img": {
      "status": "Healthy",
      "message": "API integration active - v1",
      "lastSuccessful": "2026-01-11T14:50:00Z",
      "responseTimeMs": 200
    },
    "sanmar": {
      "status": "Healthy",
      "message": "API integration active - v1",
      "lastSuccessful": "2026-01-11T14:45:00Z",
      "responseTimeMs": 180
    },
    "laapparel": {
      "status": "NoApi",
      "message": "No API integration available",
      "lastSuccessful": null,
      "responseTimeMs": null
    },
    "drivingimpressions": {
      "status": "NoApi",
      "message": "No API integration available",
      "lastSuccessful": null,
      "responseTimeMs": null
    }
  },
  "message": "Checked 7 distributors",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

### GET /api/health/errors - Error Statistics

**Query Parameters:**
- `hours` (optional, default: 24): Hours to look back

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "Info": 5,
    "Warning": 12,
    "Critical": 0
  },
  "message": "Error counts for the last 24 hours",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

### GET /api/health/errors/recent - Recent Errors

**Query Parameters:**
- `distributorId` (optional): Filter by distributor
- `severity` (optional): Filter by severity (Info, Warning, Critical)
- `hours` (optional, default: 24): Hours to look back

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "errorId": "9ab85f64-5717-4562-b3fc-2c963f66afa6",
      "timestamp": "2026-01-11T14:30:00Z",
      "distributor": "ss",
      "endpoint": "/v2/orders/",
      "httpMethod": "POST",
      "httpStatusCode": 429,
      "errorType": "RateLimit",
      "severity": "Warning",
      "errorMessage": "Too Many Requests - Rate limit exceeded",
      "retryCount": 1,
      "latencyMs": 250,
      "isResolved": true,
      "correlationId": "abc123"
    },
    {
      "errorId": "8bc85f64-5717-4562-b3fc-2c963f66afa6",
      "timestamp": "2026-01-11T10:15:00Z",
      "distributor": "ss",
      "endpoint": "/v2/products/",
      "httpMethod": "GET",
      "httpStatusCode": 400,
      "errorType": "Validation",
      "severity": "Info",
      "errorMessage": "Invalid SKU format",
      "retryCount": 0,
      "latencyMs": 50,
      "isResolved": false,
      "correlationId": "def456"
    }
  ],
  "message": "Found 2 errors in the last 24 hours",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

---

### GET /api/health/ping - Ping

**Response (200 OK):**
```
pong
```

---

## Error Responses

### 400 Bad Request
```json
{
  "success": false,
  "data": null,
  "message": "Validation failed",
  "errors": [
    "The Lines field is required.",
    "At least one line item is required"
  ],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

### 404 Not Found
```json
{
  "success": false,
  "data": null,
  "message": "Order with ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

### 429 Too Many Requests
```json
{
  "success": false,
  "data": null,
  "message": "Rate limit exceeded. Request has been queued.",
  "errors": [],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": null
}
```

### 500 Internal Server Error
```json
{
  "success": false,
  "data": null,
  "message": "An unexpected error occurred",
  "errors": [
    "Internal server error"
  ],
  "timestamp": "2026-01-11T15:00:00Z",
  "correlationId": "xyz789"
}
```

# Manual Test Conditions

This document outlines manual test conditions based on the Distributor Integration Framework PRD requirements. Test conditions are organized by PRD requirement and marked with test type and priority.

## Test Type Legend
- **Functional**: Tests core functionality and business logic
- **Integration**: Tests integration between components/systems
- **Validation**: Tests data validation and business rules
- **Negative**: Tests error handling and edge cases
- **Performance**: Tests performance and scalability
- **Regression**: Tests to ensure existing functionality still works

## Priority Legend
- **P0**: Critical - Blocks core functionality
- **P1**: High - Major feature impact
- **P2**: Medium - Standard feature validation
- **P3**: Low - Nice to have or edge cases

---

## R0: Current Requirements from Distributor APIs

### TC-R0-001: GET Blank Cost for SKUs
- **Requirement**: R0
- **Test Condition**: Verify ability to retrieve blank cost for SKUs for quote calculation
- **Test Type**: Functional
- **Priority**: P0

### TC-R0-002: GET Stock Information for SKUs
- **Requirement**: R0
- **Test Condition**: Verify ability to retrieve stock levels for SKUs to determine clean/unclean order status
- **Test Type**: Functional
- **Priority**: P0

### TC-R0-004: GET Blank Cost from Multiple Distributors
- **Requirement**: R0
- **Test Condition**: Verify ability to compare blank costs across multiple distributors for same SKU
- **Test Type**: Functional
- **Priority**: P1

### TC-R0-005: GET Warehouse Information
- **Requirement**: R0
- **Test Condition**: Verify ability to retrieve warehouse name, code, ID, and cutoff times
- **Test Type**: Functional
- **Priority**: P1

### TC-R0-006: GET Stock Information Per Warehouse
- **Requirement**: R0
- **Test Condition**: Verify ability to retrieve stock levels for SKUs per warehouse location
- **Test Type**: Functional
- **Priority**: P1

### TC-R0-007: GET Shipping Options
- **Requirement**: R0
- **Test Condition**: Verify ability to retrieve available shipping options for order placement
- **Test Type**: Functional
- **Priority**: P0

### TC-R0-008: GET Estimated Shipping Cost
- **Requirement**: R0
- **Test Condition**: Verify ability to retrieve estimated shipping cost for each shipping option
- **Test Type**: Functional
- **Priority**: P0

### TC-R0-009: POST Order to Distributor
- **Requirement**: R0
- **Test Condition**: Verify ability to place order with SKUs, quantities, printer destinations, warehouse selection, shipping speed, and split-ship capability
- **Test Type**: Functional
- **Priority**: P0

### TC-R0-010: GET Tracking Information
- **Requirement**: R0
- **Test Condition**: Verify ability to retrieve tracking number, link, shipping company, shipping option, estimated delivery date, and PO number for placed orders
- **Test Type**: Functional
- **Priority**: P0

### TC-R0-013: GET Distributor Order ID
- **Requirement**: R0
- **Test Condition**: Verify ability to retrieve distributor order ID after order placement for issue resolution reference
- **Test Type**: Functional
- **Priority**: P1

### TC-R0-015: GET Tax Amount for Order
- **Requirement**: R0
- **Test Condition**: Verify ability to retrieve tax amount for completed orders for finance reconciliation
- **Test Type**: Functional
- **Priority**: P1

### TC-R0-016: GET Invoice for Order
- **Requirement**: R0
- **Test Condition**: Verify ability to retrieve invoice details for completed orders for finance reconciliation
- **Test Type**: Functional
- **Priority**: P1

### TC-R0-017: GET Apparel Information from S&S
- **Requirement**: R0
- **Test Condition**: Verify ability to retrieve product information (style codes, names, images, GTIN, brand, color, size) and populate distributor_sku tables
- **Test Type**: Integration
- **Priority**: P1

### TC-R0-018: GET Blank Cost from S&S for Quoter
- **Requirement**: R0
- **Test Condition**: Verify ability to retrieve blank cost from S&S API for quote calculation in Quoter module
- **Test Type**: Integration
- **Priority**: P0

### TC-R0-019: GET Stock Information to distributor_sku_stock Table
- **Requirement**: R0
- **Test Condition**: Verify ability to retrieve and store stock information from distributors to distributor_sku_stock table
- **Test Type**: Integration
- **Priority**: P1

---

## R1: Setting up a Rate Limiting System

### TC-R1-001: Configurable Rate Limits Per Distributor
- **Requirement**: R1
- **Test Condition**: Verify system allows configuration of rate limits per distributor (e.g., S&S: 60 requests/minute)
- **Test Type**: Functional
- **Priority**: P0

### TC-R1-002: Request Queue When Approaching Rate Limits
- **Requirement**: R1
- **Test Condition**: Verify system queues requests when approaching 90% of rate limit threshold
- **Test Type**: Functional
- **Priority**: P0

### TC-R1-004: Automatic Retry on 429 Responses
- **Requirement**: R1
- **Test Condition**: Verify system automatically retries up to 3 times on 429 (Too Many Requests) responses with increasing delays (1s, 5s, 15s)
- **Test Type**: Functional
- **Priority**: P0

### TC-R1-005: Request Prioritization
- **Requirement**: R1
- **Test Condition**: Verify request prioritization: Order placement (P0) > Tracking updates (P1) > Product data (P2)
- **Test Type**: Functional
- **Priority**: P0

### TC-R1-007: Never Exceed 90% of Rate Limit
- **Requirement**: R1
- **Test Condition**: Verify system never exceeds 90% of any distributor's stated rate limit
- **Test Type**: Validation
- **Priority**: P0

### TC-R1-009: Monitor X-Rate-Limit-Remaining Header
- **Requirement**: R1
- **Test Condition**: Verify system monitors X-Rate-Limit-Remaining header on every API response
- **Test Type**: Functional
- **Priority**: P0

### TC-R1-010: Queue Requests When Header Shows ≤6 Remaining
- **Requirement**: R1
- **Test Condition**: Verify system queues subsequent requests when X-Rate-Limit-Remaining header shows ≤6 remaining
- **Test Type**: Functional
- **Priority**: P0

---

## R2: Setting up a Standardized Data Schema

### TC-R2-001: Transform All Distributor Responses to Standard Schema
- **Requirement**: R2
- **Test Condition**: Verify all distributor API responses are transformed to standard schema before storage
- **Test Type**: Integration
- **Priority**: P0

### TC-R2-002: Standard Schema Supports Orders Module Fields
- **Requirement**: R2
- **Test Condition**: Verify standard schema supports all fields required by Orders Module
- **Test Type**: Integration
- **Priority**: P0

### TC-R2-004: S&S Field Mapping to Standard Schema
- **Requirement**: R2
- **Test Condition**: Verify correct mapping of S&S API fields to standard schema fields (guid → internal_order_guid, orderNumber → distributor_order_id, etc.)
- **Test Type**: Integration
- **Priority**: P0

### TC-R2-006: L1 Shipping Cost Limitation Handling
- **Requirement**: R2
- **Test Condition**: Verify system handles limitation where S&S only provides total shipping cost, not itemized per-SKU
- **Test Type**: Validation
- **Priority**: P1

### TC-R2-008: Weight Limitation Handling
- **Requirement**: R2
- **Test Condition**: Verify system handles limitation where only totalWeight available, no per-box weight breakdown
- **Test Type**: Validation
- **Priority**: P2

### TC-R2-009: Warehouse Cutoff Time Manual Configuration
- **Requirement**: R2
- **Test Condition**: Verify system supports manual configuration for warehouse cutoff times since not available in S&S API
- **Test Type**: Functional
- **Priority**: P1

---

## R3: Set up API Error Monitoring & Alerting

### TC-R3-001: Monitor API Request Timeouts
- **Requirement**: R3
- **Test Condition**: Verify system monitors all API requests for timeouts (30s threshold)
- **Test Type**: Functional
- **Priority**: P0

### TC-R3-002: Monitor 5xx Server Errors
- **Requirement**: R3
- **Test Condition**: Verify system monitors and logs all 5xx server errors
- **Test Type**: Functional
- **Priority**: P0

### TC-R3-004: Monitor Connection Failures
- **Requirement**: R3
- **Test Condition**: Verify system monitors and logs connection failures
- **Test Type**: Functional
- **Priority**: P0

### TC-R3-005: Log Detailed Error Context
- **Requirement**: R3
- **Test Condition**: Verify error logs include request payload, response (if any), distributor, timestamp, and affected order ID
- **Test Type**: Validation
- **Priority**: P0

### TC-R3-006: Critical Alert for Order Placement Failures
- **Requirement**: R3
- **Test Condition**: Verify critical alerts (order placement failures) trigger immediate Slack + Email notifications
- **Test Type**: Functional
- **Priority**: P0

### TC-R3-008: Order Placement Failure Alert Within 60 Seconds
- **Requirement**: R3
- **Test Condition**: Verify order placement failures alert within 60 seconds
- **Test Type**: Performance
- **Priority**: P0

### TC-R3-009: Alert Includes Required Information
- **Requirement**: R3
- **Test Condition**: Verify alerts include distributor name, error type, affected order ID, error message, and retry count
- **Test Type**: Validation
- **Priority**: P0

### TC-R3-011: Error Classification
- **Requirement**: R3
- **Test Condition**: Verify error classification: CRITICAL (order placement, 401, 500, 503), WARNING (429 rate limit), INFO (400, 404 validation errors)
- **Test Type**: Functional
- **Priority**: P0

---

## R4: Update S&S API Connections and Operations to New API Framework

### TC-R4-001: All S&S Orders Routed Through New Framework
- **Requirement**: R4
- **Test Condition**: Verify all S&S orders are routed through the new Distributor Integration Framework layer
- **Test Type**: Integration
- **Priority**: P0

### TC-R4-002: Maintain Split-Ship Functionality
- **Requirement**: R4
- **Test Condition**: Verify split-ship functionality is maintained (always enabled)
- **Test Type**: Regression
- **Priority**: P0

### TC-R4-003: Maintain SKU/Quantity Ordering
- **Requirement**: R4
- **Test Condition**: Verify SKU and quantity ordering functionality is maintained
- **Test Type**: Regression
- **Priority**: P0

### TC-R4-004: Maintain Printer Destination Routing
- **Requirement**: R4
- **Test Condition**: Verify printer destination routing functionality is maintained
- **Test Type**: Regression
- **Priority**: P0

### TC-R4-005: Maintain EDI/API Order Placement
- **Requirement**: R4
- **Test Condition**: Verify EDI/API order placement functionality is maintained
- **Test Type**: Regression
- **Priority**: P0

### TC-R4-007: No Regression in S&S Ordering Functionality
- **Requirement**: R4
- **Test Condition**: Verify no regression in S&S ordering functionality after migration
- **Test Type**: Regression
- **Priority**: P0

### TC-R4-008: All Required Data Fields Populated
- **Requirement**: R4
- **Test Condition**: Verify 100% of successfully placed orders have all required data fields populated
- **Test Type**: Validation
- **Priority**: P0

### TC-R4-009: Capture SKUs and Quantities Ordered
- **Requirement**: R4
- **Test Condition**: Verify system captures SKUs and quantities ordered for Orders Module
- **Test Type**: Functional
- **Priority**: P0

### TC-R4-010: Capture Printer Shipping Address/ZIP
- **Requirement**: R4
- **Test Condition**: Verify system captures printer shipping address and ZIP for Orders Module
- **Test Type**: Functional
- **Priority**: P0

### TC-R4-011: Capture Split-Ship Flag
- **Requirement**: R4
- **Test Condition**: Verify system captures split-ship flag (always enabled) for Orders Module
- **Test Type**: Functional
- **Priority**: P0

### TC-R4-012: Capture Warehouse Selection
- **Requirement**: R4
- **Test Condition**: Verify system captures warehouse selection (from S&S response) for Orders Module
- **Test Type**: Functional
- **Priority**: P0

### TC-R4-013: API Calls Route Through Rate Limiter
- **Requirement**: R4
- **Test Condition**: Verify all API calls route through rate limiter
- **Test Type**: Integration
- **Priority**: P0

### TC-R4-014: Errors Caught by Error Monitoring System
- **Requirement**: R4
- **Test Condition**: Verify all errors are caught by the error monitoring system
- **Test Type**: Integration
- **Priority**: P0

---

## R5: Capture Granular Order Details for Ops & Finance

### TC-R5-001: Capture S&S Order ID After Placement
- **Requirement**: R5
- **Test Condition**: Verify system captures S&S order ID/reference number immediately after order placement
- **Test Type**: Functional
- **Priority**: P0

### TC-R5-002: Capture Actual SKUs Shipped
- **Requirement**: R5
- **Test Condition**: Verify system captures actual SKUs shipped from S&S API response
- **Test Type**: Functional
- **Priority**: P0

### TC-R5-003: Capture Blank Item Costs Per SKU
- **Requirement**: R5
- **Test Condition**: Verify system captures blank item costs per SKU from S&S API response
- **Test Type**: Functional
- **Priority**: P0

### TC-R5-004: Capture L1 Shipping Cost
- **Requirement**: R5
- **Test Condition**: Verify system captures L1 shipping cost (distributor to printer) from S&S API response
- **Test Type**: Functional
- **Priority**: P0

### TC-R5-007: Capture Origin Warehouse Location
- **Requirement**: R5
- **Test Condition**: Verify system captures origin warehouse location/zipcode from S&S API response
- **Test Type**: Functional
- **Priority**: P1

### TC-R5-008: Capture Order Timestamp
- **Requirement**: R5
- **Test Condition**: Verify system captures order timestamp from S&S API response
- **Test Type**: Functional
- **Priority**: P0

### TC-R5-010: Capture L1 Shipping Taxes
- **Requirement**: R5
- **Test Condition**: Verify system captures L1 shipping taxes from S&S API response
- **Test Type**: Functional
- **Priority**: P1

### TC-R5-011: Complete Cost Data for All Orders
- **Requirement**: R5
- **Test Condition**: Verify 100% of successfully placed S&S orders have complete cost data
- **Test Type**: Validation
- **Priority**: P0

### TC-R5-012: Finance Can Query Cost Data Per Order
- **Requirement**: R5
- **Test Condition**: Verify Finance team can query cost data per order without manual lookup
- **Test Type**: Functional
- **Priority**: P0

### TC-R5-013: Ops Can See Order Details in V3/V4
- **Requirement**: R5
- **Test Condition**: Verify Ops team can see order details in existing V3/V4 interface
- **Test Type**: Integration
- **Priority**: P0

### TC-R5-015: Alert on Backorders
- **Requirement**: R5
- **Test Condition**: Verify system alerts on backorders when detected in order response
- **Test Type**: Functional
- **Priority**: P1

### TC-R5-016: Flag Discontinuations
- **Requirement**: R5
- **Test Condition**: Verify system flags discontinued SKUs when detected in order response
- **Test Type**: Functional
- **Priority**: P1

---

## R6: Capture Tracking Information when Blanks are Shipped from S&S Warehouse to Printer (L1)

### TC-R6-001: Capture Tracking Number When Shipment Created
- **Requirement**: R6
- **Test Condition**: Verify system captures shipment tracking number(s) as soon as S&S ships blanks to printer
- **Test Type**: Functional
- **Priority**: P0

### TC-R6-002: Capture Carrier Name
- **Requirement**: R6
- **Test Condition**: Verify system captures carrier name (e.g., "UPS") from S&S shipment notification
- **Test Type**: Functional
- **Priority**: P0

### TC-R6-003: Capture Number of Boxes
- **Requirement**: R6
- **Test Condition**: Verify system captures number of boxes from S&S shipment notification
- **Test Type**: Functional
- **Priority**: P0

### TC-R6-005: Capture Total Shipment Weight
- **Requirement**: R6
- **Test Condition**: Verify system captures total shipment weight from S&S shipment notification
- **Test Type**: Functional
- **Priority**: P0

### TC-R6-006: Capture Ship Date
- **Requirement**: R6
- **Test Condition**: Verify system captures ship date from S&S shipment notification
- **Test Type**: Functional
- **Priority**: P0

### TC-R6-007: Capture Estimated Delivery Date
- **Requirement**: R6
- **Test Condition**: Verify system captures estimated delivery date (EDD) from S&S shipment notification
- **Test Type**: Functional
- **Priority**: P0

### TC-R6-009: Capture Destination Printer ZIP/Address
- **Requirement**: R6
- **Test Condition**: Verify system captures destination printer ZIP/address from S&S shipment notification
- **Test Type**: Functional
- **Priority**: P0

### TC-R6-010: Capture Initial Status
- **Requirement**: R6
- **Test Condition**: Verify system captures initial status (e.g., "Label Created") from S&S shipment notification
- **Test Type**: Functional
- **Priority**: P0

### TC-R6-011: Store Shipment Leg Data as L1
- **Requirement**: R6
- **Test Condition**: Verify shipment leg data is correctly identified and stored as L1 (distributor to printer)
- **Test Type**: Functional
- **Priority**: P0

### TC-R6-012: All Tracking Fields Populated
- **Requirement**: R6
- **Test Condition**: Verify all tracking fields are populated (no nulls for available data)
- **Test Type**: Validation
- **Priority**: P0

### TC-R6-013: Query S&S Tracking API for Full Details
- **Requirement**: R6
- **Test Condition**: Verify system queries S&S tracking API for full tracking details using tracking number
- **Test Type**: Integration
- **Priority**: P0

### TC-R6-014: Store in Standardized Tracking Schema
- **Requirement**: R6
- **Test Condition**: Verify tracking data is stored in standardized tracking schema
- **Test Type**: Integration
- **Priority**: P0

### TC-R6-015: Tracking Visible in V3/V4 Interfaces
- **Requirement**: R6
- **Test Condition**: Verify tracking data is visible in existing V3/V4 tracking interfaces
- **Test Type**: Integration
- **Priority**: P0

---

## R7: Post Tracking to V3 when Shipped from S&S Warehouse to Printer (L1)

### TC-R7-001: Auto-Post to V3 When Shipment Created
- **Requirement**: R7
- **Test Condition**: Verify system auto-posts tracking to V3 when shipment is created (label generated)
- **Test Type**: Integration
- **Priority**: P0

### TC-R7-002: Display Tracking URL in V3/V4
- **Requirement**: R7
- **Test Condition**: Verify tracking URL (from shipper - S&S) is displayed in V3/V4 interfaces
- **Test Type**: Integration
- **Priority**: P0

### TC-R7-003: Display Tracking Number in V3/V4
- **Requirement**: R7
- **Test Condition**: Verify tracking number is displayed in V3/V4 interfaces
- **Test Type**: Integration
- **Priority**: P0

### TC-R7-004: Display Carrier Name in V3/V4
- **Requirement**: R7
- **Test Condition**: Verify carrier name is displayed in V3/V4 interfaces
- **Test Type**: Integration
- **Priority**: P0

### TC-R7-005: Display Current Status in V3/V4
- **Requirement**: R7
- **Test Condition**: Verify current status is displayed in V3/V4 interfaces
- **Test Type**: Integration
- **Priority**: P0

### TC-R7-006: Display Estimated Delivery Date in V3/V4
- **Requirement**: R7
- **Test Condition**: Verify estimated delivery date is displayed in V3/V4 interfaces
- **Test Type**: Integration
- **Priority**: P0

### TC-R7-007: Display Ship Date in V3/V4
- **Requirement**: R7
- **Test Condition**: Verify ship date is displayed in V3/V4 interfaces
- **Test Type**: Integration
- **Priority**: P0

### TC-R7-011: Display Destination Printer in V3/V4
- **Requirement**: R7
- **Test Condition**: Verify destination printer is displayed in V3/V4 interfaces
- **Test Type**: Integration
- **Priority**: P1

### TC-R7-012: Display Shipment Leg Indicator L1 in V3/V4
- **Requirement**: R7
- **Test Condition**: Verify shipment leg indicator L1 is displayed in V3/V4 interfaces
- **Test Type**: Integration
- **Priority**: P1

### TC-R7-013: Tracking Visible Immediately After Ship Notification
- **Requirement**: R7
- **Test Condition**: Verify tracking is visible in V3 for ship notification immediately
- **Test Type**: Performance
- **Priority**: P0

### TC-R7-014: All Teams Can Access Tracking
- **Requirement**: R7
- **Test Condition**: Verify all teams (Ops, Sales, Printer Management) can access tracking information
- **Test Type**: Functional
- **Priority**: P0

---

## R8: Update In-Transit Status 2x Daily

### TC-R8-001: Poll S&S Tracking API 2x Daily
- **Requirement**: R8
- **Test Condition**: Verify system polls S&S tracking API for status updates at least 2x per day (suggestion: 9 am, 5 pm ET)
- **Test Type**: Functional
- **Priority**: P0

### TC-R8-002: Update Current Status Field
- **Requirement**: R8
- **Test Condition**: Verify system updates current status field (e.g., "In Transit", "Out for Delivery") from polling results
- **Test Type**: Functional
- **Priority**: P0

### TC-R8-003: Append Status History
- **Requirement**: R8
- **Test Condition**: Verify system appends new events to status history JSON array with timestamp
- **Test Type**: Functional
- **Priority**: P0

### TC-R8-006: Update All Not-Delivered Shipments
- **Requirement**: R8
- **Test Condition**: Verify all shipments with status NOT IN ('Delivered') are updated at least 2x daily
- **Test Type**: Validation
- **Priority**: P0

### TC-R8-007: Status History Shows Timestamp
- **Requirement**: R8
- **Test Condition**: Verify status history shows timestamp of each status change
- **Test Type**: Validation
- **Priority**: P0

### TC-R8-008: Scheduled Job Runs 2x Daily
- **Requirement**: R8
- **Test Condition**: Verify scheduled job (cron) runs 2x daily to poll tracking API
- **Test Type**: Functional
- **Priority**: P0

### TC-R8-009: Query Tracking Shipments for Update
- **Requirement**: R8
- **Test Condition**: Verify system queries tracking_shipments table for all shipments with status NOT IN ('Delivered')
- **Test Type**: Functional
- **Priority**: P0

### TC-R8-010: Call S&S API with Tracking Number
- **Requirement**: R8
- **Test Condition**: Verify system calls S&S API with tracking number to get latest status
- **Test Type**: Integration
- **Priority**: P0

### TC-R8-011: Update Tracking Record with Latest Data
- **Requirement**: R8
- **Test Condition**: Verify system updates tracking_shipments record with latest data from API response
- **Test Type**: Functional
- **Priority**: P0

---

## R9: Capture Delivery Confirmation for Printer (L1)

### TC-R9-001: Capture Actual Delivery Date/Time
- **Requirement**: R9
- **Test Condition**: Verify system captures actual delivery date/time when delivery status is detected
- **Test Type**: Functional
- **Priority**: P0

### TC-R9-002: Capture Number of Boxes Delivered
- **Requirement**: R9
- **Test Condition**: Verify system captures number of boxes delivered from S&S delivery confirmation
- **Test Type**: Functional
- **Priority**: P0

### TC-R9-004: Capture Delivery Location
- **Requirement**: R9
- **Test Condition**: Verify system captures delivery location (confirm printer address/zip) from S&S delivery confirmation
- **Test Type**: Functional
- **Priority**: P0

### TC-R9-007: Calculate Box Count Comparison
- **Requirement**: R9
- **Test Condition**: Verify system automatically calculates boxes_delivered vs boxes_shipped comparison
- **Test Type**: Functional
- **Priority**: P0

### TC-R9-009: Delivery Confirmation Visible in V3/V4
- **Requirement**: R9
- **Test Condition**: Verify delivery confirmation is visible in V3/V4 tracking view
- **Test Type**: Integration
- **Priority**: P0

### TC-R9-010: Triggered by Tracking Status Change to Delivered
- **Requirement**: R9
- **Test Condition**: Verify delivery confirmation capture is triggered by tracking status change to "Delivered"
- **Test Type**: Functional
- **Priority**: P0

### TC-R9-011: Query S&S API for Delivery Proof Data
- **Requirement**: R9
- **Test Condition**: Verify system queries S&S API for delivery proof data when status changes to "Delivered"
- **Test Type**: Integration
- **Priority**: P0

### TC-R9-012: Misshipment Detection at Shipment Stage
- **Requirement**: R9
- **Test Condition**: Verify misshipment alerts are generated when box count or weight mismatch detected at shipment stage
- **Test Type**: Functional
- **Priority**: P1

### TC-R9-013: Misshipment Detection at Printer Arrival Stage
- **Requirement**: R9
- **Test Condition**: Verify misshipment alerts are generated when box count or weight mismatch detected at printer arrival stage
- **Test Type**: Functional
- **Priority**: P1

### TC-R9-014: Misshipment Detection at Printer Shipment Stage
- **Requirement**: R9
- **Test Condition**: Verify misshipment alerts are generated when box count or weight mismatch detected at printer shipment stage (future requirement)
- **Test Type**: Functional
- **Priority**: P3

### TC-R9-015: Misshipment Detection at Customer Arrival Stage
- **Requirement**: R9
- **Test Condition**: Verify misshipment alerts are generated when box count or weight mismatch detected at customer arrival stage (future requirement)
- **Test Type**: Functional
- **Priority**: P3

---

## Cross-Requirement Test Conditions

### TC-XR-001: End-to-End Order Placement Flow
- **Requirement**: R0, R4, R5
- **Test Condition**: Verify complete end-to-end flow from order placement through cost capture and storage
- **Test Type**: Integration
- **Priority**: P0

### TC-XR-002: End-to-End Tracking Flow
- **Requirement**: R6, R7, R8, R9
- **Test Condition**: Verify complete end-to-end flow from shipment creation through delivery confirmation
- **Test Type**: Integration
- **Priority**: P0

### TC-XR-003: Rate Limiting During High Load
- **Requirement**: R1, R4
- **Test Condition**: Verify rate limiting system prevents violations during high load scenarios
- **Test Type**: Performance
- **Priority**: P0

### TC-XR-004: Error Handling During API Failures
- **Requirement**: R3, R4, R5, R6
- **Test Condition**: Verify error handling and alerting work correctly during API failures
- **Test Type**: Negative
- **Priority**: P0

### TC-XR-005: Data Consistency Across Systems
- **Requirement**: R2, R4, R5, R6, R7
- **Test Condition**: Verify data consistency across API, database, and V3/V4 interfaces
- **Test Type**: Integration
- **Priority**: P0

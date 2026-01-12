package com.dif.api.tests.products;

import com.dif.api.client.ProductsApiClient;
import com.dif.api.tests.BaseTest;
import io.qameta.allure.Description;
import io.qameta.allure.Feature;
import io.qameta.allure.Severity;
import io.qameta.allure.SeverityLevel;
import io.restassured.response.Response;
import org.testng.annotations.BeforeClass;
import org.testng.annotations.Test;

import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import static org.assertj.core.api.Assertions.assertThat;

/**
 * Tests for Products API endpoints.
 * Covers: GET /api/products, GET /api/products/{sku},
 * GET /api/products/{sku}/inventory, GET /api/products/inventory/batch
 */
@Feature("Products API")
public class ProductsTests extends BaseTest {
    
    private ProductsApiClient productsApi;
    
    // Known valid SKUs from the mock API data
    private static final String VALID_SKU = "G500-BLA-M";
    private static final String VALID_SKU_2 = "G500-BLA-L";
    private static final String INVALID_SKU = "INVALID-SKU-999";
    
    @BeforeClass
    @Override
    public void setUp() {
        super.setUp();
        productsApi = new ProductsApiClient();
    }
    
    @Test(groups = {"smoke", "products"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify listing products returns a paginated product list")
    public void listProducts_returnsPaginatedProductList() {
        logTestStart("listProducts_returnsPaginatedProductList");
        
        Response response = productsApi.listProducts();
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify pagination fields
        assertThat(response.jsonPath().getList("items"))
                .as("Items list should not be empty")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getInt("page"))
                .as("Page number should be 1")
                .isEqualTo(1);
        
        assertThat(response.jsonPath().getInt("totalItems"))
                .as("Total items should be greater than 0")
                .isGreaterThan(0);
        
        logTestEnd("listProducts_returnsPaginatedProductList");
    }
    
    @Test(groups = {"regression", "products"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify listing products with pagination parameters")
    public void listProducts_withPagination_returnsRequestedPage() {
        logTestStart("listProducts_withPagination_returnsRequestedPage");
        
        Response response = productsApi.listProducts(1, 5);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        assertThat(response.jsonPath().getInt("pageSize"))
                .as("Page size should match requested size")
                .isEqualTo(5);
        
        assertThat(response.jsonPath().getList("items").size())
                .as("Items count should be at most the page size")
                .isLessThanOrEqualTo(5);
        
        logTestEnd("listProducts_withPagination_returnsRequestedPage");
    }
    
    @Test(groups = {"regression", "products"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify listing products with style code filter")
    public void listProducts_withStyleCodeFilter_returnsFilteredProducts() {
        logTestStart("listProducts_withStyleCodeFilter_returnsFilteredProducts");
        
        Map<String, String> filters = new HashMap<>();
        filters.put("styleCode", "G500");
        
        Response response = productsApi.listProducts(filters);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        List<String> styleCodes = response.jsonPath().getList("items.styleCode");
        assertThat(styleCodes)
                .as("All returned products should have the filtered style code")
                .isNotEmpty()
                .allMatch(code -> code.equals("G500"));
        
        logTestEnd("listProducts_withStyleCodeFilter_returnsFilteredProducts");
    }
    
    @Test(groups = {"smoke", "products"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify getting a product by valid SKU returns product details")
    public void getProductBySku_withValidSku_returnsProduct() {
        logTestStart("getProductBySku_withValidSku_returnsProduct");
        
        Response response = productsApi.getProductBySku(VALID_SKU);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify product data
        assertThat(response.jsonPath().getString("data.sku"))
                .as("SKU should match requested SKU")
                .isEqualTo(VALID_SKU);
        
        assertThat(response.jsonPath().getString("data.styleCode"))
                .as("Style code should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getString("data.brandName"))
                .as("Brand name should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getDouble("data.blankCost"))
                .as("Blank cost should be greater than 0")
                .isGreaterThan(0);
        
        logTestEnd("getProductBySku_withValidSku_returnsProduct");
    }
    
    @Test(groups = {"negative", "products"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting a product by invalid SKU returns 404")
    public void getProductBySku_withInvalidSku_returns404() {
        logTestStart("getProductBySku_withInvalidSku_returns404");
        
        Response response = productsApi.getProductBySku(INVALID_SKU);
        
        assertStatusCode(response, 404);
        assertFailure(response);
        
        assertThat(response.jsonPath().getString("message"))
                .as("Error message should indicate product not found")
                .containsIgnoringCase("not found");
        
        logTestEnd("getProductBySku_withInvalidSku_returns404");
    }
    
    @Test(groups = {"smoke", "products"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify getting inventory for a valid SKU returns stock levels")
    public void getInventory_withValidSku_returnsStockLevels() {
        logTestStart("getInventory_withValidSku_returnsStockLevels");
        
        Response response = productsApi.getInventory(VALID_SKU);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify inventory data
        List<Map<String, Object>> inventoryList = response.jsonPath().getList("data");
        assertThat(inventoryList)
                .as("Inventory list should not be empty")
                .isNotEmpty();
        
        // Verify first inventory item has required fields
        assertThat(response.jsonPath().getString("data[0].warehouseCode"))
                .as("Warehouse code should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getInt("data[0].quantityAvailable"))
                .as("Quantity available should be non-negative")
                .isGreaterThanOrEqualTo(0);
        
        logTestEnd("getInventory_withValidSku_returnsStockLevels");
    }
    
    @Test(groups = {"negative", "products"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify getting inventory for invalid SKU returns empty data")
    public void getInventory_withInvalidSku_returnsEmptyData() {
        logTestStart("getInventory_withInvalidSku_returnsEmptyData");
        
        Response response = productsApi.getInventory(INVALID_SKU);
        
        // API returns 200 with empty data for non-existent SKU inventory
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        List<Map<String, Object>> inventoryList = response.jsonPath().getList("data");
        assertThat(inventoryList)
                .as("Inventory list should be empty for invalid SKU")
                .isEmpty();
        
        assertThat(response.jsonPath().getString("message"))
                .as("Message should indicate no inventory found")
                .containsIgnoringCase("no inventory");
        
        logTestEnd("getInventory_withInvalidSku_returnsEmptyData");
    }
    
    @Test(groups = {"smoke", "products"})
    @Severity(SeverityLevel.CRITICAL)
    @Description("Verify batch inventory returns data for multiple SKUs")
    public void getBatchInventory_withValidSkus_returnsMultipleSkuInventory() {
        logTestStart("getBatchInventory_withValidSkus_returnsMultipleSkuInventory");
        
        List<String> skus = Arrays.asList(VALID_SKU, VALID_SKU_2);
        Response response = productsApi.getBatchInventory(skus);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        // Verify data contains inventory for requested SKUs
        Map<String, Object> data = response.jsonPath().getMap("data");
        assertThat(data)
                .as("Data should contain inventory for requested SKUs")
                .containsKey(VALID_SKU)
                .containsKey(VALID_SKU_2);
        
        logTestEnd("getBatchInventory_withValidSkus_returnsMultipleSkuInventory");
    }
    
    @Test(groups = {"regression", "products"})
    @Severity(SeverityLevel.NORMAL)
    @Description("Verify listing products returns expected product fields")
    public void listProducts_returnsExpectedProductFields() {
        logTestStart("listProducts_returnsExpectedProductFields");
        
        Response response = productsApi.listProducts();
        
        assertStatusCode(response, 200);
        
        // Verify first product has all expected fields
        assertThat(response.jsonPath().getString("items[0].sku"))
                .as("SKU field should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getString("items[0].styleCode"))
                .as("Style code field should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getString("items[0].brandName"))
                .as("Brand name field should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getString("items[0].color"))
                .as("Color field should be present")
                .isNotEmpty();
        
        assertThat(response.jsonPath().getString("items[0].size"))
                .as("Size field should be present")
                .isNotEmpty();
        
        logTestEnd("listProducts_returnsExpectedProductFields");
    }
    
    @Test(groups = {"regression", "products"})
    @Severity(SeverityLevel.MINOR)
    @Description("Verify listing products with distributor filter")
    public void listProducts_withDistributorFilter_returnsFilteredProducts() {
        logTestStart("listProducts_withDistributorFilter_returnsFilteredProducts");
        
        Map<String, String> filters = new HashMap<>();
        filters.put("distributorId", "ss");
        
        Response response = productsApi.listProducts(filters);
        
        assertStatusCode(response, 200);
        assertSuccess(response);
        
        List<String> distributorIds = response.jsonPath().getList("items.distributorId");
        assertThat(distributorIds)
                .as("All returned products should belong to the filtered distributor")
                .isNotEmpty()
                .allMatch(id -> id.equals("ss"));
        
        logTestEnd("listProducts_withDistributorFilter_returnsFilteredProducts");
    }
}

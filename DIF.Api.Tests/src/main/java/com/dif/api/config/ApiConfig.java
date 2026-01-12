package com.dif.api.config;

import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/**
 * Configuration class for API test settings.
 * Loads configuration from config.properties file.
 */
public class ApiConfig {
    
    private static final Properties properties = new Properties();
    private static final String CONFIG_FILE = "config.properties";
    
    static {
        loadProperties();
    }
    
    private static void loadProperties() {
        try (InputStream inputStream = ApiConfig.class.getClassLoader().getResourceAsStream(CONFIG_FILE)) {
            if (inputStream != null) {
                properties.load(inputStream);
            } else {
                throw new RuntimeException("Configuration file '" + CONFIG_FILE + "' not found in classpath");
            }
        } catch (IOException e) {
            throw new RuntimeException("Failed to load configuration file: " + CONFIG_FILE, e);
        }
    }
    
    /**
     * Gets the base URL for the API.
     * @return Base URL (e.g., "http://localhost:5000")
     */
    public static String getBaseUrl() {
        return getProperty("base.url", "http://localhost:5000");
    }
    
    /**
     * Gets the API timeout in milliseconds.
     * @return Timeout value
     */
    public static int getTimeout() {
        return Integer.parseInt(getProperty("api.timeout", "30000"));
    }
    
    /**
     * Checks if request logging is enabled.
     * @return true if request logging is enabled
     */
    public static boolean isRequestLoggingEnabled() {
        return Boolean.parseBoolean(getProperty("log.request", "true"));
    }
    
    /**
     * Checks if response logging is enabled.
     * @return true if response logging is enabled
     */
    public static boolean isResponseLoggingEnabled() {
        return Boolean.parseBoolean(getProperty("log.response", "true"));
    }
    
    /**
     * Gets the default distributor ID for tests.
     * @return Default distributor ID
     */
    public static String getDefaultDistributorId() {
        return getProperty("default.distributor.id", "ss");
    }
    
    /**
     * Gets a property value with a default fallback.
     * @param key Property key
     * @param defaultValue Default value if property not found
     * @return Property value
     */
    public static String getProperty(String key, String defaultValue) {
        return properties.getProperty(key, defaultValue);
    }
    
    /**
     * Gets a property value.
     * @param key Property key
     * @return Property value or null if not found
     */
    public static String getProperty(String key) {
        return properties.getProperty(key);
    }
}

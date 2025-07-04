# entain2 Test Automation Framework

## Overview

This project contains automated integration tests for the Pet Store API. It uses MSTest for test execution, a custom `Client` for API calls, and includes logging and configuration management.

## Project Structure

- `Base.cs` - Base test class managing setup, teardown, and shared HttpClient.
- `PetControllerTests.cs` - Positive tests validating core Pet Store features.
- `PetControllerNegativeTests.cs` - Negative tests targeting invalid scenarios.
- `ConfigManager.cs` - Loads configuration from `appsettings.json`.
- `Logger.cs` - Logs test execution details to file and test context.
- `PetGenerator.cs` - Generates pet data for tests.
- `RawJsonClient.cs` - Used for sending raw JSON requests to API.

## Configuration

Use `appsettings.json` to configure:

```json
{
  "BaseUrl": "http://localhost:8080/api",
  "TimeoutSeconds": 30,
  "Environment": "local"
}
```

Only `BaseUrl` is required.

## Running Tests

1. Start the Pet Store API, for example using Docker:
   
   ```bash
   docker run -d -p 8080:8080 swaggerapi/petstore
   ```

2. Run tests:
   
   ```bash
   dotnet test
   ```

3. Stop the Docker container:
   
   ```bash
   docker stop <container_id>
   docker rm <container_id>
   ```

## Logging

Logs are saved in the `Logs` directory with timestamps. Logs are also output to MSTest `TestContext`.

## Notes

- The tests cover positive and negative scenarios.
- Negative tests include cases with missing required fields and invalid IDs.
- Configuration is loaded once per test run using lazy loading.
- The framework cleans up created test pets after each test.

---

Created by Marko Baletić  
2025  
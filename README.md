# entain2 Test Automation Framework

## Overview

This project provides automated integration tests for the Pet Store API using MSTest and a custom `Client`.
It includes configuration via `appsettings.json` and logging of test execution.

## Project Structure

- `Base.cs` — shared test setup, teardown, HttpClient instance
- `PetControllerTests.cs` — positive tests for creating, updating, deleting, and querying pets
- `PetControllerNegativeTests.cs` — negative tests for invalid requests and missing fields
- `ConfigManager.cs` — reads `appsettings.json` for BaseUrl (and optional settings)
- `Logger.cs` — writes logs to `Logs` folder and MSTest `TestContext` output
- `PetGenerator.cs` — generates test pet objects
- `RawJsonClient.cs` — sends raw JSON HTTP requests
- `pullDockerAndRunTests.ps1` — PowerShell script to start Docker, run tests, and stop container

## Configuration

Place an `appsettings.json` file next to your test project `.csproj` with at least:

```json
{
  "BaseUrl": "http://localhost:8080/api"
}
```

Optionally add `TimeoutSeconds` and `Environment` keys, for example:

```json
{
  "BaseUrl": "http://localhost:8080/api",
  "TimeoutSeconds": 30,
  "Environment": "local"
}
```

## Running Tests on Windows

### Prerequisites
- Docker Desktop
- .NET SDK 8.0+

### Manual steps

1. Open PowerShell and start the Pet Store API:

   ```powershell
   docker run -d -p 8080:8080 swaggerapi/petstore
   ```

2. Run tests:

   ```powershell
   dotnet test
   ```

3. Stop and remove the container:

   ```powershell
   docker stop <container_id>
   docker rm <container_id>
   ```

### Automated via script

Run the PowerShell script to pull the image, start the container, run tests, and clean up:

```powershell
.\pullDockerAndRunTests.ps1
```

## Logging

Logs are saved under `Logs/` with timestamps and echoed to MSTest test output.

## Approach

- Configuration is loaded once per run via `ConfigManager`.
- Tests cover positive and negative scenarios for all Pet endpoints.
- Each test creates its own data via `PetGenerator` and cleans up after itself.

## Future Work

- Expand tests to cover every API method, including orders, users, authentication, and login.
- Add data-driven tests for different input combinations.

---
![image](https://github.com/user-attachments/assets/4c49a061-cebb-4c28-b0f0-552f7a19772d)


Created by Marko Baletić  
2025

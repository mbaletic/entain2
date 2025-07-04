# entain2 Test Automation Framework

## Overview

This project provides automated integration tests for the Pet Store API using MSTest and a custom Client.
It includes configuration via appsettings.json and logging of test execution.

## Project Structure

- Base.cs — shared test setup, teardown, HttpClient instance
- PetControllerTests.cs — positive tests for creating, updating, deleting, and querying pets
- PetControllerNegativeTests.cs — negative tests for invalid requests and missing fields
- ConfigManager.cs — reads appsettings.json for BaseUrl (and optional settings)
- Logger.cs — writes logs to Logs folder and MSTest TestContext output
- PetGenerator.cs — generates test pet objects
- RawJsonClient.cs — sends raw JSON HTTP requests
- pullDockerAndRunTests.ps1 — PowerShell script to start Docker, run tests, and stop container

## Configuration

Place an appsettings.json next to your test project .csproj with at least:

json prefix { prefix "BaseUrl": "http://localhost:8080/api" prefix } prefix

Optionally add TimeoutSeconds and Environment keys with defaults in code.

## Running Tests on Windows

### Prerequisite
- Docker Desktop
- .NET SDK 8.0+

### Manual steps

1. Open PowerShell and run the Pet Store API container:

powershell prefix docker run -d -p 8080:8080 swaggerapi/petstore prefix

2. Run tests:

powershell prefix dotnet test prefix

3. Stop and remove the container:

powershell prefix docker stop <container_id> prefix docker rm <container_id> prefix

### Automated via script

Run pullDockerAndRunTests.ps1 in PowerShell:

powershell prefix .\pullDockerAndRunTests.ps1 prefix

This script pulls the Docker image, starts the container, runs dotnet test,
then stops and removes the container automatically.

## Logging

Logs are saved under Logs\ with timestamps and echoed to MSTest test output.

## Approach

- Configuration is loaded once per run via ConfigManager.
- Tests cover positive and negative scenarios for all Pet endpoints.
- Each test creates its own data via PetGenerator and cleans up after itself.

## Future Work

- Expand tests to cover every API method, including orders, users, authentication, and login.
- Add data-driven tests for different input combinations.

---

Created by Marko Baletić
2025
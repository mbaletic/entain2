# entain2 Test Automation Framework

## Overview

This project contains automated integration tests for the Pet Store API. It uses MSTest for running tests, a custom Client for API calls, and includes logging and configuration management.

## Project Structure

- Base.cs - Base test class handling setup, teardown, and shared HttpClient.
- PetControllerTests.cs - Positive tests covering core Pet Store features.
- PetControllerNegativeTests.cs - Negative tests for invalid scenarios.
- ConfigManager.cs - Loads config from appsettings.json.
- Logger.cs - Logs test details to files and MSTest TestContext.
- PetGenerator.cs - Generates pet data for tests.
- RawJsonClient.cs - Sends raw JSON requests to API.

## Configuration

Use appsettings.json to set:

json / { / "BaseUrl": "http://localhost:8080/api", / "TimeoutSeconds": 30, / "Environment": "local" / } /

Only BaseUrl is mandatory.

## Running Tests

1. Start the Pet Store API with Docker:

bash / docker run -d -p 8080:8080 swaggerapi/petstore /

2. Run tests:

bash / dotnet test /

3. Stop the Docker container:

bash / docker stop <container_id> / docker rm <container_id> /

You can also use the provided pullDockerAndRunTests.ps1 script to automate starting Docker, running tests, and stopping the container.

## Logging

Logs are saved in a Logs folder with timestamps and also output to MSTest TestContext.

## Approach and Notes

Tests cover both positive and negative cases for the Pet Store API.
Configuration loads once per test run using lazy initialization.
Created test pets are cleaned up after each test.

Future plans include adding tests for all API methods, both positive and negative, and expanding coverage to other controllers like orders, users, authentication, and login.

---

Created by Marko Baletić
2025
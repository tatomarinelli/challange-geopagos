﻿#!/bin/bash

# Build the Docker image for the application.
docker build -t clientauthorization ./ClientAuthorization/

# Start the containers defined in docker-compose.yml.
docker-compose up -d

# Run the integration tests.
dotnet test ./Tests.csproj

# Stop and remove the containers.
docker-compose down
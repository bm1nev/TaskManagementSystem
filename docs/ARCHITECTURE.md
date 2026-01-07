# Architecture Overview

This project follows a Clean Architecture–inspired approach with clear separation of concerns.

## Layers

### Domain
Contains core business entities, enums, and business rules.
This layer has no dependencies on external frameworks.

### Application
Contains application services, DTOs, and interfaces.
Implements use cases and orchestrates domain logic.

### Infrastructure
Responsible for data persistence and external concerns.
Includes Entity Framework Core, repositories, and migrations.

### WebApi
ASP.NET Core Web API layer.
Handles HTTP requests, authentication, authorization, and middleware.

## Key Principles
- Separation of concerns
- Business logic outside controllers
- Infrastructure isolated from core logic
- Easily extendable and testable design

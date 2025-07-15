# Copilot Instructions for Bookify

## Overview
Bookify is a pragmatic Clean Architecture application designed with separation of concerns, dependency inversion, and explicit dependencies. The project is structured into four main layers:

- **Domain Layer**: Implements core business logic using Domain-Driven Design (DDD) principles.
- **Application Layer**: Orchestrates domain logic and implements use cases.
- **Infrastructure Layer**: Handles external concerns like database, identity providers, and messaging.
- **Presentation Layer**: Provides the entry point to the system via RESTful APIs.

## Key Conventions
- **File-Scoped Namespaces**: All C# files must use file-scoped namespace declarations.
- **CQRS**: Commands and queries are separated using MediatR for loose coupling.
- **Validation and Logging**: Implemented as pipeline behaviors in the Application layer.
- **Minimal APIs**: Controllers are being migrated to minimal API endpoints.
- **Database**: Uses EF Core with PostgreSQL. Migrations are located in `src/Bookify.Infrastructure/Migrations`.

## Developer Workflows
### Build and Run
- Build the project: `dotnet build src/Bookify.Api/Bookify.Api.csproj`
- Run the project: `dotnet run --project src/Bookify.Api`
- Watch mode: Use the `watch` task in VS Code.

### Testing
- Run all tests: `dotnet test`
- Test types:
  - Unit Tests: Validate domain and application logic.
  - Integration Tests: Use Testcontainers for database integration.
  - Functional Tests: End-to-end tests with Testcontainers.
  - Architecture Tests: Enforce layer rules and design conventions.

### Database
- Auto-created and seeded in the `Development` environment.
- Create a migration: `dotnet ef migrations add -p src/Bookify.Infrastructure -s src/Bookify.Api <Migration_Name>`

## Patterns and Practices
- **Domain Layer**:
  - Rich domain model with encapsulated business logic.
  - Use `ValueObjects` for immutable types (e.g., `Money`, `DateRange`).
  - Domain events are handled via MediatR.
- **Application Layer**:
  - Commands and queries are defined in `src/Bookify.Application`.
  - Handlers implement `ICommandHandler` or `IQueryHandler`.
  - Cross-cutting concerns (e.g., logging, validation) are implemented as pipeline behaviors.
- **Infrastructure Layer**:
  - Implements abstractions defined in the Domain and Application layers.
  - Uses the Transactional Outbox pattern for reliable domain events.
  - Identity management is integrated with Keycloak.
- **Presentation Layer**:
  - RESTful APIs are defined in `src/Bookify.Api/Controllers` and `src/Bookify.Api/Endpoints`.
  - Use DTOs for requests and responses.

## External Dependencies
- **Keycloak**: Used for authentication and authorization.
- **PostgreSQL**: Primary database.
- **Serilog**: For logging.
- **Testcontainers**: For integration and functional tests.

## Examples
- **Command Handler**: See `src/Bookify.Application/Bookings/ReserveBooking/ReserveBookingCommandHandler.cs`.
- **Validation**: See `src/Bookify.Application/Bookings/ReserveBooking/ReserveBookingCommandValidator.cs`.
- **Minimal API Endpoint**: See `src/Bookify.Api/Endpoints/Bookings/BookingsEndpoints.cs`.

## Additional Guidelines

- file:/instructions/csharp.instructions.md
- file:/instructions/aspnet.instructions.md
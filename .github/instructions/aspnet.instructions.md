---
applyTo: "**/*.cs"
description: "ASP.NET Core performance & reliability best practices based on: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices, Project Architecture & Coding Standards based on Milan Jovanovic"
---

# Performance & Reliability

## General

* Cache aggressively, where results can be reused.
* Identify and optimise hot code paths with profilers (PerfView, VS Diagnostics).
* Use environment-specific configuration files.
* Use secure communication with HTTPS.
* Implement health checks for the application.
* Implement proper caching strategies.

## Asynchrony

* Avoid blocking calls (`Task.Wait`, `.Result`, sync over async).
* Make controller/Razor actions fully asynchronous; prefer `async/await`.
* Use async/await for I/O‑bound operations.
* Call all data‑access APIs asynchronously.
* Prefer asynchronous read/write (`ReadToEndAsync`, `ReadFormAsync`, `DeserializeAsync`).

## Data & I/O

* Query only required data; use `AsNoTracking` for read‑only EF Core queries.
* Consider caching and connection pooling (`DbContext` pooling, compiled queries) when measurable gains justify complexity.
* Use Entity Framework Core for database operations.
* Use strongly-typed configuration with IOptions pattern.
* Use Guid for identifiers unless otherwise specified.

## Pagination & Streaming

* Paginate large result sets; stream with `IAsyncEnumerable<T>` where appropriate.

## Memory

* Minimise large object allocations (≥ 85 kB); reuse buffers via `ArrayPool<T>`.
* Cache frequently used large objects to reduce LOH pressure.

## HTTP & Networking

* Use `HttpClientFactory`; never create/dispose `HttpClient` per request.
* Compress responses; bundle / minify static assets.
* Upgrade to the latest ASP.NET Core version to inherit platform optimisations.

# Request‑Pipeline Guidelines

## Middleware

* Keep early pipeline middleware fast; avoid long‑running tasks.
* Do not write to the response body before delegating if downstream middleware must run.
* Modify headers only before `HttpResponse.HasStarted` or via `OnStarting`.
* Use middleware for cross-cutting concerns.

## HttpContext Safety

* Never store `HttpContext` in fields; access it via `IHttpContextAccessor` on demand.
* Do not access `HttpContext` from multiple threads or after the request completes.
* In background work, copy needed data and create a scoped DI lifetime.

## Stream Handling

* Avoid buffering large request/response bodies in memory.

# Project Architecture & Coding Standards

## Language & Typing

* Use C# 12 language features where appropriate.
* Favor explicit typing (this is very important). Only use `var` when evident.
* Use `is null` checks instead of `== null`; likewise for `is not null` and `!= null`.

## Design Principles

* Follow SOLID principles in class and interface design.
* Implement dependency injection for loose coupling.
* Use primary constructors for dependency injection in services, use cases, etc.
* Make types `internal` and `sealed` by default unless otherwise specified.
* Prefer record types for immutable data structures.

## API Design

* Prefer minimal APIs endpoints over controllers.
* Implement proper versioning for APIs.
* Implement proper model validation.
* Implement proper CORS policies.
* Use Swagger/OpenAPI for API documentation.
* Implement proper authentication and authorization.

## Logging & Diagnostics

* Implement proper exception handling and logging.
* Implement proper logging with structured logging.

## Testing

* Implement unit tests for business logic.
* Use integration tests for API endpoints.

## Background Work

* Use background services for long-running tasks.
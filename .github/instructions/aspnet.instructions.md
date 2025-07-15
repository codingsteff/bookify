---
applyTo: "**/*.cs"
description: "ASP.NET Core performance & reliability best practices based on: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices"
---

# Performance & Reliability

## General
- Cache aggressively, where results can be reused.  
- Identify and optimise hot code paths with profilers (PerfView, VS Diagnostics).  

## Asynchrony
- Avoid blocking calls (`Task.Wait`, `.Result`, sync over async).  
- Make controller/Razor actions fully asynchronous; prefer `async/await`.  

## Data & I/O
- Query only required data; use `AsNoTracking` for read‑only EF Core queries.  
- Call all data‑access APIs asynchronously.  
- Consider caching and connection pooling (`DbContext` pooling, compiled queries) when measurable gains justify complexity.  

## Pagination & Streaming
- Paginate large result sets; stream with `IAsyncEnumerable<T>` where appropriate.  

## Memory
- Minimise large object allocations (≥ 85 kB); reuse buffers via `ArrayPool<T>`.  
- Cache frequently used large objects to reduce LOH pressure.  

## HTTP & Networking
- Use `HttpClientFactory`; never create/dispose `HttpClient` per request.  
- Compress responses; bundle / minify static assets.  
- Upgrade to the latest ASP.NET Core version to inherit platform optimisations.  

# Request‑Pipeline Guidelines

## Middleware
- Keep early pipeline middleware fast; avoid long‑running tasks.  
- Do not write to the response body before delegating if downstream middleware must run.  
- Modify headers only before `HttpResponse.HasStarted` or via `OnStarting`.  

## HttpContext Safety
- Never store `HttpContext` in fields; access it via `IHttpContextAccessor` on demand.  
- Do not access `HttpContext` from multiple threads or after the request completes.  
- In background work, copy needed data and create a scoped DI lifetime.  

## Stream Handling
- Prefer asynchronous read/write (`ReadToEndAsync`, `ReadFormAsync`, `DeserializeAsync`).  
- Avoid buffering large request/response bodies in memory.  

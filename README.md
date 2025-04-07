# bookify
Pragmatic Clean Architecture application
* [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
* Separation of concerns
* Encapsulation
* Dependency inversion (flow inwards, inner Layers define interfaces, outer layers implement them)
* Explicit dependencies
* Single responsibility principle



## TODO
[ ] Finish the course  
[ ] Migrate all Controllers to Minimal APIs-Endpoints  
[ ] Get rid of MediatR 
[ ] Add Aggregates and AggregateRoots  
[ ] SignalR communication  
[ ] Change Caching to [HybridCache](https://devblogs.microsoft.com/dotnet/hybrid-cache-is-now-ga/)  
[ ] Use EF Core RawSql with Unmapped Types as [Alternative to Dapper](https://www.youtube.com/watch?v=0ArU_C0gPdA&list=WL&index=53)  
[ ] Message Brokers (RabbitMQ)  


## Start
Start the DevContainer

Hit [F5]

or `dotnet run --project src/Bookify.Api` *TODO: not working docker binds to 127.0.0.1?*

### Tests

### Database
* Database will auto created and seeded with data on application start in `Development` environment
* [pgAdmin](http://localhost:8080) *user@user.io, hunter2*
* Create new migration: `dotnet ef migrations add -p src/Bookify.Infrastructure -s src/Bookify.Api <Migration_Name>`



## Layers

### Domain Layer
* Persistence Ignorance
* Domain Driven Design (Bounded Contexts, Entities, ValueObjects, DomainServices, DomainEvents)
* Repositories + Unit of Work (only abstractions)
* Rich Domain Model (Business Logic, Validation in Entities)
* Entities (Object with Identity, private setters, encapsulation)
  * Behavior-, State-Transitions-Methods and Commands (raise DomainEvents)
* Value Objects (Immutable, no identity, in C# with record) e.g. Money, Address, DateRange
* Domain Services (Business Logic don't fit to Entity, can call Repositories)
* Domain Events (when something happens in the domain, can be handled by other parts of the system via MediatR.Contracts)

### Application Layer
* Orchestrates the domain
* Business Logic not fitting to Domain
* Use Cases (Driver of the application, taking Domain Model and telling them what to do)
* Application Services (use Repositories, DomainServices)
* CQRS (Command Query Responsibility Segregation), Loose Coupling with MediatR
* Cross-Cutting Concerns/Pipeline behaviors (Logging, Validation)

> Pragmatic: use of Dapper for access ReadModels, normaly don't directly use external libs

### Infrastructure Layer
* External concerns (Database, Message Brokers, Identity Providers, File Systems, ...)
* Implementations of abstractions
* Persistence (EF Core and PostgresSQL)
* Transactional Outbox Message Pattern for reliable DomainEvents in distributed systems (UnitOfWork when saving Entities)
* Identity Provider: Keycloak

### Presentation Layer
* Entry Point to the system
* WebApi, gRPC, SPA ...
* RESTful API
  * Request/Response
  * Controllers and Endpoints creates Commands and Queries
  * don't expose Commands directly, use DTOs (Request)
  * MediatR handles Commands and Queries (loose coupling)
  * Dependency setup for all layers
  * Middlewares for Cross-Cutting Concerns (Logging)

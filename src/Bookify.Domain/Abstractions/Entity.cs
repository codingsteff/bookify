namespace Bookify.Domain.Abstractions;
public abstract class Entity
{
    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity() { }

    public Guid Id { get; init; } // init is used to set the value only once

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
    public void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
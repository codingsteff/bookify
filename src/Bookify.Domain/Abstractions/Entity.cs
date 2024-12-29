namespace Bookify.Domain.Abstractions;
public abstract class Entity
{
    protected Entity(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; init; } // init is used to set the value only once
}
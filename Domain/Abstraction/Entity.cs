using System.Collections.Generic;

namespace Domain.Abstraction;
public abstract class Entity
{
    public readonly List<IDomainEvent> _domainEvents = new();
    public Entity(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; init; }

    public IReadOnlyList<IDomainEvent> GetDomainEvents() { return _domainEvents; }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}


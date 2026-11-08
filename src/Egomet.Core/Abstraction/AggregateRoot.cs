
using System;
using Egomet.Core.Interfaces;

namespace Egomet.Core.Abstraction;

public abstract class AggregateRoot<T, TId>(TId id) : Entity<T, TId>(id), IAggregateRoot<TId>
    where T : AggregateRoot<T, TId>
{
    private readonly List<IDomainEvent> events = [];

    public IReadOnlyCollection<IDomainEvent> Events
        => [.. events];

    protected void RaiseEvent(IDomainEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        events.Add(@event);
    }

    public void ClearEvents()
        => events.Clear();

    public override string ToString()
        => $"{GetType().Name}:{base.ToString()}";
}

// 🗹 Done
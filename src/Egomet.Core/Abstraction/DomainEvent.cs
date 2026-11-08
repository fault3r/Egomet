
using System;
using Egomet.Core.Interfaces;

namespace Egomet.Core.Abstraction;

public abstract record DomainEvent : IDomainEvent
{
    public Guid EventId { get; }
    public DateTimeOffset OccurredOn { get; }

    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTimeOffset.UtcNow;
    }

    public override string ToString()
        => $"[{OccurredOn:O}] {GetType().Name} (EventId={EventId})";
}

// 🗹 Done

using System;

namespace Egomet.Core.Interfaces;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTimeOffset OccurredOn { get; }
}
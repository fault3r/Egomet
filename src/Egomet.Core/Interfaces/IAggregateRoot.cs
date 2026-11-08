
using System;

namespace Egomet.Core.Interfaces;

public interface IAggregateRoot<TId>
{
    TId Id { get; }
}

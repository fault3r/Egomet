
using System;

namespace Egomet.Core.Interfaces;

public interface IEntity<TId>
{
    TId Id { get; }
}

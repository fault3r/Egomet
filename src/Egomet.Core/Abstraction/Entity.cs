
using System;
using Egomet.Core.Interfaces;

namespace Egomet.Core.Abstraction;

public abstract class Entity<T, TId>(TId id) : IEntity<TId>, IEquatable<T>
    where T : Entity<T, TId>
{
    public TId Id { get; } = id;

    public override string ToString()
        => $"{Id}";

    public override bool Equals(object? obj)
        => obj is T other && Equals(other);

    public bool Equals(T? other)
    {
        if (ReferenceEquals(this, other))
            return true;

        if (other is null)
            return false;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode()
        => HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity<T, TId>? left, Entity<T, TId>? right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right as T);
    }

    public static bool operator !=(Entity<T, TId>? left, Entity<T, TId>? right)
        => !(left == right);
}

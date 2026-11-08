
using System;
using Egomet.Core.Abstraction;

namespace Egomet.Core.ValueObjects;

public sealed class IdentityValueObject : ValueObject<IdentityValueObject>
{
    public Guid Value { get; }

    private IdentityValueObject(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Identity cannot be empty.", nameof(value));

        Value = value;
    }

    private IdentityValueObject(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        value = value.Trim();

        if (!Guid.TryParse(value, out var guid) || guid == Guid.Empty)
            throw new FormatException("Invalid identity.");

        Value = guid;
    }

    public static IdentityValueObject From(Guid value)
        => new(value);

    public static IdentityValueObject Parse(string value)
        => new(value);

    public static bool TryParse(string? value, out IdentityValueObject? identity)
    {
        identity = null;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (!Guid.TryParse(value.Trim(), out var guid) || guid == Guid.Empty)
            return false;

        identity = new IdentityValueObject(guid);

        return true;
    }

    public override string ToString()
        => Value.ToString();

    public static implicit operator string(IdentityValueObject identity)
        => identity.ToString();

    public static explicit operator IdentityValueObject(string value)
        => Parse(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

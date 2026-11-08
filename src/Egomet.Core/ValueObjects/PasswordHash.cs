
using System;
using Egomet.Core.Abstraction;

namespace Egomet.Core.ValueObjects;

public sealed class PasswordHashValueObject : ValueObject<PasswordHashValueObject>
{
    public string Value { get; }

    private PasswordHashValueObject(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (!IsValid(value))
            throw new FormatException("Invalid password hash.");

        Value = value;
    }

    private static bool IsValid(string value)
        => value.Length <= 4096;

    public static PasswordHashValueObject Parse(string value)
        => new(value);

    public static bool TryParse(string? value, out PasswordHashValueObject? hash)
    {
        hash = null;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (!IsValid(value))
            return false;

        hash = new PasswordHashValueObject(value);
        return true;
    }

    public override string ToString()
        => Value;

    public static implicit operator string(PasswordHashValueObject hash)
        => hash.Value;

    public static explicit operator PasswordHashValueObject(string value)
        => Parse(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

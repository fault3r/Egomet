
using System;
using Egomet.Core.Abstraction;

namespace Egomet.Core.ValueObjects;

public sealed class RoleValueObject : ValueObject<RoleValueObject>
{
    public enum RoleType
    {
        Unsupported = 0,
        User = 1,
        Admin = 101
    }

    public RoleType Value { get; }

    private RoleValueObject(RoleType roleType)
    {
        if (!IsValid(roleType))
            throw new ArgumentOutOfRangeException(nameof(roleType), roleType, "Unsupported role.");

        Value = roleType;
    }

    private RoleValueObject(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        value = value.Trim();

        if (!Enum.TryParse(value, ignoreCase: true, out RoleType roleType)
            || !IsValid(roleType)
            || !Enum.IsDefined(roleType))
        {
            throw new FormatException("Invalid role.");
        }

        Value = roleType;
    }

    private static bool IsValid(RoleType roleType)
        => roleType is RoleType.User or RoleType.Admin;

    public static RoleValueObject From(RoleType roleType)
        => new(roleType);

    public static RoleValueObject Parse(string value)
        => new(value);

    public static bool TryParse(string? value, out RoleValueObject? role)
    {
        role = null;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (!Enum.TryParse(value.Trim(), true, out RoleType roleType)
            || !IsValid(roleType))
        {
            return false;
        }

        role = new RoleValueObject(roleType);
        return true;
    }

    public bool IsUser => Value == RoleType.User;

    public bool IsAdmin => Value == RoleType.Admin;

    public override string ToString()
        => Value.ToString().ToLowerInvariant();

    public static implicit operator string(RoleValueObject role)
        => role.ToString();

    public static explicit operator RoleValueObject(string value)
        => Parse(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static readonly RoleValueObject Admin = new(RoleType.Admin);

    public static readonly RoleValueObject User = new(RoleType.User);
}

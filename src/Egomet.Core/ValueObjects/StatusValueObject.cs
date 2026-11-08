
using System;
using Egomet.Core.Abstraction;

namespace Egomet.Core.ValueObjects;

public sealed class StatusValueObject : ValueObject<StatusValueObject>
{
    public enum StatusType
    {
        Unsupported = 0,
        Locked = 1,
        Pending = 2,
        Active = 3
    }

    public StatusType Value { get; }

    private StatusValueObject(StatusType statusType)
    {
        if (!IsValid(statusType))
            throw new ArgumentOutOfRangeException(nameof(statusType), statusType, "Unsupported status.");

        Value = statusType;
    }

    private StatusValueObject(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        value = value.Trim();

        if (!Enum.TryParse(value, true, out StatusType statusType)
            || !IsValid(statusType))
        {
            throw new FormatException(
                $"Unsupported status: {value}");
        }

        Value = statusType;
    }

    private static bool IsValid(StatusType statusType)
        => statusType is StatusType.Locked or StatusType.Pending or StatusType.Active;

    public static StatusValueObject From(StatusType statusType)
        => new(statusType);

    public static StatusValueObject Parse(string value)
        => new(value);

    public static bool TryParse(string? value, out StatusValueObject? status)
    {
        status = null;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (!Enum.TryParse(value.Trim(), true, out StatusType statusType)
            || !IsValid(statusType))
            return false;

        status = new StatusValueObject(statusType);
        return true;
    }

    public bool IsLocked => Value == StatusType.Locked;
    public bool IsPending => Value == StatusType.Pending;
    public bool IsActive => Value == StatusType.Active;

    public override string ToString()
        => Value.ToString().ToLowerInvariant();

    public static implicit operator string(StatusValueObject status)
        => status.ToString();

    public static explicit operator StatusValueObject(string value)
        => Parse(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static readonly StatusValueObject Locked  = new(StatusType.Locked);
    public static readonly StatusValueObject Pending  = new(StatusType.Pending);
    public static readonly StatusValueObject Active  = new(StatusType.Active);
}

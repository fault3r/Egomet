
using System;
using System.Text.RegularExpressions;
using Egomet.Core.Abstraction;

namespace Egomet.Core.ValueObjects;

public sealed class EmailValueObject : ValueObject<EmailValueObject>
{
    public string Value { get; }

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.CultureInvariant | RegexOptions.Compiled
    );

    private EmailValueObject(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        value = value.Trim();

        if (value.Length > 254 || !IsValid(value))
            throw new FormatException("Invalid email address.");

        Value = Normalize(value);
    }

    private static bool IsValid(string value)
        => EmailRegex.IsMatch(value);

    private static string Normalize(string value)
    {
        var atIndex = value.IndexOf('@');

        var local = value[..atIndex];
        var domain = value[(atIndex + 1)..];

        return $"{local}@{domain.ToLowerInvariant()}";
    }

    public static EmailValueObject Parse(string value)
        => new(value);

    public static bool TryParse(string? value, out EmailValueObject? email)
    {
        email = null;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        try
        {
            email = Parse(value);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public override string ToString()
        => Value;

    public static implicit operator string(EmailValueObject email)
        => email.Value;

    public static explicit operator EmailValueObject(string value)
        => Parse(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}


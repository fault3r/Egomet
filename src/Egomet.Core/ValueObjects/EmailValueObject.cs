
using System;
using System.Text.RegularExpressions;
using Egomet.Core.Abstraction;

namespace Egomet.Core.ValueObjects;

public sealed class EmailValueObject : ValueObject<EmailValueObject>
{
    public string Value { get; }

    private EmailValueObject(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(value);

        value = value.Trim();

        if (!IsValid(value))
            throw new InvalidOperationException(value);

        Value = Normalize(value);
    }

    private static readonly Regex EmailRegex = new(
        pattern: @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        options: RegexOptions.CultureInvariant | RegexOptions.Compiled
    );

    private static bool IsValid(string value)
        => EmailRegex.IsMatch(value);

    private static string Normalize(string value)
    {
        var atIndex = value.IndexOf('@');
        var local = value[..atIndex];
        var domain = value[++atIndex..];

        return $"{local}@{domain.ToLowerInvariant()}";
    }

    public static EmailValueObject Parse(string value)
        => new(value);

    public static bool TryParse(string value, out EmailValueObject? email)
    {
        try
        {
            email = Parse(value);
            return true;
        }
        catch
        {
            email = null;
            return false;
        }
    }

    public override string ToString()
        => Value;

    public static implicit operator string(EmailValueObject email)
        => email.Value;

    public static explicit operator EmailValueObject(string value)
        => Parse(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
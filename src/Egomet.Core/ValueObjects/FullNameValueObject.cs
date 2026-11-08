
using System;
using Egomet.Core.Abstraction;

namespace Egomet.Core.ValueObjects;

public sealed class FullNameValueObject : ValueObject<FullNameValueObject>
{
    public string FirstName { get; }
    public string LastName { get; }

    private FullNameValueObject(string firstName, string lastName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);

        FirstName = Normalize(firstName);
        LastName = Normalize(lastName);

        if (!IsValid(FirstName, LastName))
            throw new FormatException("Invalid fullname.");
    }

    private FullNameValueObject(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var parts = Normalize(value)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
            throw new FormatException("Fullname must contain at least two parts.");

        FirstName = parts[0];
        LastName = string.Join(' ', parts.Skip(1));

        if (!IsValid(FirstName, LastName))
            throw new FormatException("Invalid fullname.");
    }

    private static string Normalize(string value)
        => string.Join(' ', value.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries));

    private static bool IsValid(string firstName, string lastName)
    {
        var fullName = $"{firstName} {lastName}";

        return fullName.Length >= 5 && fullName.Length <= 100;
    }

    public static FullNameValueObject From(string firstName, string lastName)
        => new(firstName, lastName);

    public static FullNameValueObject Parse(string value)
        => new(value);

    public static bool TryParse(string? value, out FullNameValueObject? fullName)
    {
        fullName = null;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        try
        {
            fullName = Parse(value);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public override string ToString()
        => $"{FirstName} {LastName}";

    public static implicit operator string(FullNameValueObject fullName)
        => fullName.ToString();

    public static explicit operator FullNameValueObject(string value)
        => Parse(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }

    #region EF Core
    private FullNameValueObject()
    {
        FirstName = null!;
        LastName = null!;
    }
    #endregion
}



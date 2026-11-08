
using System;
using Egomet.Core.Abstraction;
using Egomet.Core.ValueObjects;

namespace Egomet.Core.Aggregates.UserAggregate.Events;

public sealed record UserPasswordChangedEvent : DomainEvent
{
    public IdentityValueObject UserId { get; }
    public EmailValueObject Email { get; }
    public FullNameValueObject FullName { get; }

    public UserPasswordChangedEvent(
        IdentityValueObject userId,
        EmailValueObject email,
        FullNameValueObject fullName)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(fullName);

        UserId = userId;
        Email = email;
        FullName = fullName;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, FullName={FullName}";
}
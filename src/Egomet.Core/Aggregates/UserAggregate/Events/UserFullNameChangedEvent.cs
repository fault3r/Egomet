
using System;
using Egomet.Core.Abstraction;
using Egomet.Core.ValueObjects;

namespace Egomet.Core.Aggregates.UserAggregate.Events;

public sealed record UserFullNameChangedEvent : DomainEvent
{
    public IdentityValueObject UserId { get; }
    public EmailValueObject Email { get; }
    public FullNameValueObject NewFullName { get; }

    public UserFullNameChangedEvent(
        IdentityValueObject userId,
        EmailValueObject email,
        FullNameValueObject newFullName)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(newFullName);

        UserId = userId;
        Email = email;
        NewFullName = newFullName;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, NewFullName={NewFullName}";
}

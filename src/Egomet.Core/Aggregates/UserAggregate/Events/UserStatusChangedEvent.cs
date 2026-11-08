
using System;
using Egomet.Core.Abstraction;
using Egomet.Core.ValueObjects;

namespace Egomet.Core.Aggregates.UserAggregate.Events;

public sealed record UserStatusChangedEvent : DomainEvent
{
    public IdentityValueObject UserId { get; }
    public EmailValueObject Email { get; }
    public StatusValueObject NewStatus { get; }

    public UserStatusChangedEvent(
        IdentityValueObject userId,
        EmailValueObject email,
        StatusValueObject newStatus)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(newStatus);

        UserId = userId;
        Email = email;
        NewStatus = newStatus;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, NewStatus={NewStatus}";
}

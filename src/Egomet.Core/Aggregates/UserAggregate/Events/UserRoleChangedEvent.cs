
using System;
using Egomet.Core.Abstraction;
using Egomet.Core.ValueObjects;

namespace Egomet.Core.Aggregates.UserAggregate.Events;

public sealed record UserRoleChangedEvent : DomainEvent
{
    public IdentityValueObject UserId { get; }
    public EmailValueObject Email { get; }
    public RoleValueObject NewRole { get; }

    public UserRoleChangedEvent(
        IdentityValueObject userId,
        EmailValueObject email,
        RoleValueObject newRole)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(newRole);

        UserId = userId;
        Email = email;
        NewRole = newRole;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, NewRole={NewRole}";
}

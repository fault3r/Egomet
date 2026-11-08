
using System;
using Egomet.Core.Abstraction;
using Egomet.Core.Aggregates.UserAggregate.Events;
using Egomet.Core.ValueObjects;

namespace Egomet.Core.Aggregates.UserAggregate;

public sealed class UserAggregate : AggregateRoot<UserAggregate, IdentityValueObject>
{
    public EmailValueObject Email { get; private set; }
    public PasswordHashValueObject PasswordHash { get; private set; }
    public FullNameValueObject FullName { get; private set; }
    public RoleValueObject Role { get; private set; }
    public StatusValueObject Status { get; private set; }

    private UserAggregate(
        IdentityValueObject id,
        EmailValueObject email,
        PasswordHashValueObject passwordHash,
        FullNameValueObject fullName,
        RoleValueObject role,
        StatusValueObject status)
        : base(id)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(passwordHash);
        ArgumentNullException.ThrowIfNull(fullName);
        ArgumentNullException.ThrowIfNull(role);
        ArgumentNullException.ThrowIfNull(status);

        Email = email;
        PasswordHash = passwordHash;
        FullName = fullName;
        Role = role;
        Status = status;
    }

    public static UserAggregate Create(
        IdentityValueObject id,
        EmailValueObject email,
        PasswordHashValueObject passwordHash,
        FullNameValueObject fullName)
    {
        ArgumentNullException.ThrowIfNull(id);

        var user = new UserAggregate(
            id,
            email,
            passwordHash,
            fullName,
            RoleValueObject.User,
            StatusValueObject.Pending);

        user.RaiseEvent(
            new UserRegisteredEvent(user.Id, user.Email, user.FullName));

        return user;
    }

    public void ChangePassword(PasswordHashValueObject newPasswordHash)
    {
        ArgumentNullException.ThrowIfNull(newPasswordHash);

        if (newPasswordHash == PasswordHash)
            return;

        PasswordHash = newPasswordHash;

        RaiseEvent(new UserPasswordChangedEvent(Id, Email, FullName));
    }

    public void ChangeFullName(FullNameValueObject newFullName)
    {
        ArgumentNullException.ThrowIfNull(newFullName);

        if (newFullName == FullName)
            return;

        FullName = newFullName;

        RaiseEvent(new UserFullNameChangedEvent(Id, Email, FullName));
    }

    public void PromoteToAdmin()
    {
        if (Role.IsAdmin)
            return;

        Role = RoleValueObject.Admin;

        RaiseEvent(new UserRoleChangedEvent(Id, Email, Role));
    }

    public void DemoteToUser()
    {
        if (Role.IsUser)
            return;

        Role = RoleValueObject.User;

        RaiseEvent(new UserRoleChangedEvent(Id, Email, Role));
    }

    public void Lock()
    {
        if (Status.IsLocked)
            return;

        Status = StatusValueObject.Locked;

        RaiseEvent(new UserStatusChangedEvent(Id, Email, Status));
    }

    public void Unlock()
    {
        if (!Status.IsLocked)
            return;

        Status = StatusValueObject.Active;

        RaiseEvent(new UserStatusChangedEvent(Id, Email, Status));
    }

    public void Activate()
    {
        if (Status.IsActive)
            return;

        Status = StatusValueObject.Active;

        RaiseEvent(new UserStatusChangedEvent(Id, Email, Status));
    }

    #region EF Core
    public UserAggregate(IdentityValueObject id) : base(id)
    {
        Email = null!;
        PasswordHash = null!;
        FullName = null!;
        Role = null!;
        Status = null!;
    }
    #endregion
}

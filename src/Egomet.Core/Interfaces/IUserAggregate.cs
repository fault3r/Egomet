
using System;
using Egomet.Core.ValueObjects;

namespace Egomet.Core.Interfaces;

public interface IUserAggregate
{
    IdentityValueObject Id { get; }
    EmailValueObject Email { get; }
    PasswordHashValueObject PasswordHash { get; }
    FullNameValueObject FullName { get; }
    RoleValueObject Role { get; }
    StatusValueObject Status { get; }

    void ChangePassword(PasswordHashValueObject newPasswordHash);
    void ChangeFullName(FullNameValueObject newFullName);
    void PromoteToAdmin();
    void DemoteToUser();
    void Activate();
    void Deactivate();
}

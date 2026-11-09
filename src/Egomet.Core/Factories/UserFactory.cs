
using System;
using Egomet.Core.Aggregates.UserAggregate;
using Egomet.Core.ValueObjects;

namespace Egomet.Core.Factories;

public sealed class UserFactory
{
    public static UserAggregate Create(
        EmailValueObject email,
        PasswordHashValueObject passwordHash,
        FullNameValueObject fullname)
    {
        var defaultId = IdentityValueObject.From(Guid.NewGuid());
        var defaultRole = RoleValueObject.User;
        var defaultStatus = StatusValueObject.Pending;

        return UserAggregate.Create(
            id: defaultId,
            email: email,
            passwordHash: passwordHash,
            fullName: fullname,
            role: defaultRole,
            status: defaultStatus
        );
    }

    public static UserAggregate From(
        IdentityValueObject id,
        EmailValueObject email,
        PasswordHashValueObject passwordHash,
        FullNameValueObject fullName,
        RoleValueObject role,
        StatusValueObject status)
    {
        return UserAggregate.Create(id, email, passwordHash, fullName, role, status);
    }
}

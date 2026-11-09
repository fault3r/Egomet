
using System;
using Egomet.Core.Aggregates.UserAggregate;
using Egomet.Core.Common;
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

    public static Result<UserAggregate> TryCreate(
        EmailValueObject email,
        PasswordHashValueObject passwordHash,
        FullNameValueObject fullName)
    {
        try
        {
            var user = Create(email, passwordHash, fullName);
            return Result<UserAggregate>.Success(user);
        }
        catch (Exception ex)
        {
            return Result<UserAggregate>.Failure(ex.Message);
        }
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
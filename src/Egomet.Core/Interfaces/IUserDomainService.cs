
using System;
using Egomet.Core.Aggregates.UserAggregate;
using Egomet.Core.ValueObjects;

namespace Egomet.Core.Interfaces;

public interface IUserDomainService
{
    Task<bool> VerifyCanCreateAsync(
        EmailValueObject email,
        CancellationToken ct
    );

    Task<UserAggregate?> VerifyCredentialsAsync(
        EmailValueObject email,
        PasswordHashValueObject passwordHash,
        CancellationToken ct
    );
}
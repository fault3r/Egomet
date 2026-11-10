
using System;
using Egomet.Core.Aggregates.UserAggregate;
using Egomet.Core.Common;
using Egomet.Core.ValueObjects;

namespace Egomet.Core.Interfaces;

public interface IUserDomainService
{
    Task<Result> VerifyCanCreateAsync(
        EmailValueObject email,
        CancellationToken ct = default);

    Task<Result<UserAggregate>> VerifyCredentialsAsync(
        EmailValueObject email,
        string password,
        CancellationToken ct = default);
}

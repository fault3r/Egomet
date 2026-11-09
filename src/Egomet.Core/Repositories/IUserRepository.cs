
using System;
using Egomet.Core.Aggregates.UserAggregate;
using Egomet.Core.ValueObjects;

namespace Egomet.Core.Repositories;

public interface IUserRepository
{
    Task CreateAsync(UserAggregate user, CancellationToken ct);

    Task<UserAggregate?> GetByIdAsync(IdentityValueObject id, CancellationToken ct);
    Task<UserAggregate?> GetByEmailAsync(EmailValueObject email, CancellationToken ct);

    Task UpdateAsync(UserAggregate user, CancellationToken ct);
    
    Task DeleteAsync(IdentityValueObject id, CancellationToken ct);
}

using System;
using Egomet.Core.Aggregates.UserAggregate;
using Egomet.Core.ValueObjects;

namespace Egomet.Core.Repositories;

public interface IUserRepository
{
    Task CreateAsync(UserAggregate user, CancellationToken cancellationToken);

    Task<UserAggregate?> GetByIdAsync(IdentityValueObject identity, CancellationToken cancellationToken);
    Task<UserAggregate?> GetByEmailAsync(EmailValueObject email, CancellationToken cancellationToken);

    Task UpdateAsync(UserAggregate user, CancellationToken cancellationToken);
    
    Task DeleteAsync(IdentityValueObject identity, CancellationToken cancellationToken);
}
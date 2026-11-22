
using System;
using Egomet.Core.Aggregates.UserAggregate;
using Egomet.Core.Repositories;
using Egomet.Core.ValueObjects;
using Egomet.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Egomet.Infrastructure.Repositories;

public sealed class UserRepository(
    IDatabaseContext context
) : IUserRepository
{
    public async Task CreateAsync(
        UserAggregate user,
        CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }

    public async Task<UserAggregate?> GetByIdAsync(
        IdentityValueObject identity,
        CancellationToken cancellationToken)
    {
        return await context.Users
            .FirstOrDefaultAsync(
                x => x.Id == identity,
                cancellationToken);
    }

    public async Task<UserAggregate?> GetByEmailAsync(
        EmailValueObject email,
        CancellationToken cancellationToken)
    {
        return await context.Users
            .FirstOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);
    }

    public Task UpdateAsync(
        UserAggregate user,
        CancellationToken cancellationToken)
    {
        context.Users.Update(user);

        return Task.CompletedTask;
    }

    public async Task DeleteAsync(
        IdentityValueObject identity,
        CancellationToken cancellationToken)
    {
        var user = await GetByIdAsync(identity, cancellationToken);

        if (user is null)
            return;

        context.Users.Remove(user);
    }
}

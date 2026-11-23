
using System;
using Egomet.Core.Aggregates.UserAggregate;
using Egomet.Core.Repositories;
using Egomet.Core.ValueObjects;
using Egomet.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Egomet.Infrastructure.Repositories;

public sealed class UserRepository(
    EfPostgresDbContext context
) : IUserRepository
{
    private readonly EfPostgresDbContext _context =
        context ?? throw new ArgumentNullException(nameof(context));

    public async Task CreateAsync(
        UserAggregate user,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        await _context.Users.AddAsync(user, cancellationToken);
    }

    public async Task<UserAggregate?> GetByIdAsync(
        IdentityValueObject identity,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(identity);

        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == identity, cancellationToken);
    }

    public async Task<UserAggregate?> GetByEmailAsync(
        EmailValueObject email,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(email);

        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public Task UpdateAsync(
        UserAggregate user,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        _context.Users.Update(user);

        return Task.CompletedTask;
    }

    public async Task DeleteAsync(
        IdentityValueObject identity,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(identity);

        var user = await GetByIdAsync(identity, cancellationToken);

        if (user is not null)
            _context.Users.Remove(user);
    }
}

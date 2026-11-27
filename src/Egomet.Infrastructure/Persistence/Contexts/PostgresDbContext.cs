
using System;
using Egomet.Core.Aggregates.UserAggregate;
using Egomet.Core.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Egomet.Infrastructure.Persistence.Contexts;

public sealed class PostgresDbContext(
    DbContextOptions<PostgresDbContext> options
) : DbContext(options), IDatabaseContext
{
    public DbSet<UserAggregate> Users => Set<UserAggregate>();
    public DbSet<EventMessage> Events => Set<EventMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(PostgresDbContext).Assembly);
    }
}
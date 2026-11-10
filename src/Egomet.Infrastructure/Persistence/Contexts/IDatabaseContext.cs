
using System;
using Egomet.Core.Aggregates.UserAggregate;
using Egomet.Core.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Egomet.Infrastructure.Persistence.Contexts;

public interface IDatabaseContext
{
     DbSet<UserAggregate> Users { get; }
     DbSet<EventMessage> Events { get; }

     ChangeTracker ChangeTracker { get; }
     DatabaseFacade Database { get; }
  
     Task<int> SaveChangesAsync(CancellationToken ct);
}

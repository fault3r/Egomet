
using System;
using Egomet.Core.Messaging.Outbox;
using Egomet.Core.Repositories;
using Egomet.Core.UnitOfWork;
using Egomet.Infrastructure.Persistence.Contexts;

namespace Egomet.Infrastructure.UnitOfWork;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly IDatabaseContext _dbContext;

    public IEventOutbox EventOutbox { get; }
    public IUserRepository UserRepository { get; }

    public EfUnitOfWork(
        IDatabaseContext dbContext,
        IEventOutbox outbox,
        IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(outbox);
        ArgumentNullException.ThrowIfNull(userRepository);

        _dbContext = dbContext;
        EventOutbox = outbox;
        UserRepository = userRepository;
    }

    public Task<int> CommitAsync(
        CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}

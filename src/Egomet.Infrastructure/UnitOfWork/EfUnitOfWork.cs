
using System;
using Egomet.Core.Messaging.Outbox;
using Egomet.Core.Repositories;
using Egomet.Core.UnitOfWork;
using Egomet.Infrastructure.Persistence.Contexts;

namespace Egomet.Infrastructure.UnitOfWork;

public sealed class EfUnitOfWork
    : IUnitOfWork
{
    private readonly IDatabaseContext _context;

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

        _context = dbContext;
        EventOutbox = outbox;
        UserRepository = userRepository;
    }

    public async Task<int> CommitAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context
            .SaveChangesAsync(cancellationToken);
    }
}

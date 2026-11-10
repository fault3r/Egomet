
using System;
using Egomet.Core.Messaging.Outbox;
using Egomet.Core.Repositories;

namespace Egomet.Core.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken ct);
    
    IEventOutbox EventOutbox { get; }

    IUserRepository UserRepository { get; }
}
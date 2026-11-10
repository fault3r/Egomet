
using System;

namespace Egomet.Core.Security.Authentication;

public interface ISessionService
{
    Task CreateAsync(SessionData session, CancellationToken ct);

    Task<SessionData?> GetAsync(string sessionId, CancellationToken ct);
    Task<bool> ExistsAsync(string sessionId, CancellationToken ct);

    Task UpdateAsync(SessionData session, CancellationToken ct);
    
    Task InvalidateAsync(string sessionId, CancellationToken ct);
    Task InvalidateAllAsync(string userId, CancellationToken ct);
}

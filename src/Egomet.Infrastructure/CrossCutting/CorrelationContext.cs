
using System;
using Egomet.Application.Interfaces;

namespace Egomet.Infrastructure.CrossCutting;

public sealed class CorrelationContext : ICorrelationContext
{
    private static readonly AsyncLocal<string?> _correlationId = new();

    public string? CorrelationId => _correlationId.Value;

    public void Set(string correlationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        _correlationId.Value = correlationId.Trim();
    }

    public void Clear()
    {
        _correlationId.Value = null;
    }
}

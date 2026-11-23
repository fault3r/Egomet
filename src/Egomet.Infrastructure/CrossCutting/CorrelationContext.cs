
using System;
using Egomet.Application.Interfaces;

namespace Egomet.Infrastructure.CrossCutting;

public sealed class CorrelationContext : ICorrelationContext
{
    private static readonly AsyncLocal<string?> _correlation = new();

    public string? CorrelationId => _correlation.Value;

    public void Set(string correlationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        _correlation.Value = correlationId.Trim();
    }

    public void Clear()
    {
        _correlation.Value = null;
    }
}

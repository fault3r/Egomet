
using System;

namespace Egomet.Application.Interfaces;

public interface ICorrelationContext
{
    string? CorrelationId { get; }

    void Set(string correlationId);

    void Clear();
}
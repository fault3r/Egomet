
using System;

namespace Egomet.Infrastructure.Settings;

public sealed class AppSettings
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public float Version { get; set; }
}

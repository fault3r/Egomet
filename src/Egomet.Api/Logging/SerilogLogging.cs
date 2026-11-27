
using System;
using Serilog;
using Serilog.Events;

namespace Egomet.Api.Logging;

public static class SerilogLogging
{
    public static IHostBuilder AddSerilogLogging(
        this IHostBuilder host)
    {
        host.UseSerilog((context, config) =>
        {
            var filename = context.Configuration
                .GetValue<string>("Logging:SerilogSettings:Filename")
                    ?? throw new InvalidOperationException(
                        $"Missing configuration section: Logging:SerilogSettings:Filename");

            if (string.IsNullOrWhiteSpace(filename))
                throw new InvalidOperationException(
                    "Logging:SerilogSettings:Filename cannot be null or empty.");

            var logDirectory = Path.Combine(
                AppContext.BaseDirectory, "Logs");

            Directory.CreateDirectory(logDirectory);

            config
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.File(
                    path: logDirectory,
                    rollingInterval: RollingInterval.Hour,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] "
                        + "{SourceContext} {CorrelationId} {Message:lj} {NewLine} {Exception}"
                );
        });

        return host;
    }
}

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
                .GetValue<string>($"Logging:SerilogSettings:Filename")
                    ?? throw new InvalidOperationException(
                        $"Missing configuration section: Logging:SerilogSettings");

            config
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.File(
                    path: Path.Combine("Logs", filename),
                    rollingInterval: RollingInterval.Hour,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext} {CorrelationId} {Message:lj} {NewLine} {Exception}"
                );
        })
            .ConfigureServices((_, services) =>
            {
                services.AddLogging(config =>
                {
                    config.AddSerilog();
                });
            });

        return host;
    }
}
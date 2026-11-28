
using System;
using Egomet.Infrastructure.Settings;
using Egomet.Infrastructure.Exceptions;
using Egomet.Infrastructure.DependencyInjection;
using Serilog;
using Egomet.Api.Logging;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
    .GetSection(nameof(AppSettings))
    .Get<AppSettings>()
        ?? throw new InfrastructureException(
            $"Missing or invalid configuration section '{nameof(AppSettings)}'.");

builder.Host.AddSerilogLogging();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () =>
    $"Name: {settings.Name}\nVersion: {settings.Version}");

Log.Information("Application started.");

app.Run();

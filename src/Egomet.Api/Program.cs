
using System;
using Egomet.Infrastructure.Settings;
using Egomet.Infrastructure.Exceptions;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
    .GetSection(nameof(AppSettings))
    .Get<AppSettings>()
    ?? throw new InfrastructureException($"Missing {nameof(AppSettings)}.");

var app = builder.Build();

app.MapGet("/", () => $"Name: {settings.Name}\nVersion: {settings.Version}");

app.Run();

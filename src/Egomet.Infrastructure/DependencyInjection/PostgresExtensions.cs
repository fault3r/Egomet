
using System;
using Egomet.Infrastructure.Exceptions;
using Egomet.Infrastructure.Persistence.Contexts;
using Egomet.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Egomet.Infrastructure.DependencyInjection;

public static class PostgresExtensions
{
    public static IServiceCollection AddEfPostgresDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(PostgresSettings))
            .Get<PostgresSettings>()
            ?? throw new InfrastructureException(
                $"Missing or invalid configuration section '{nameof(PostgresSettings)}'.");

        var connectionString = settings.ToConnectionString();

        services.AddDbContext<PostgresDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                postgres =>
                {
                    postgres.MigrationsAssembly(
                        typeof(PostgresDbContext).Assembly.FullName);

                    postgres.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null);
                });
        });

        services.AddScoped<IDatabaseContext>(
            provider => provider.GetRequiredService<PostgresDbContext>());

        return services;
    }
}


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
            ?? throw new InfrastructureException();

        var connectionString = settings.ToConnectionString();

        services.AddDbContext<EfPostgresDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                postgres =>
                {
                    postgres.MigrationsAssembly(
                        typeof(EfPostgresDbContext).Assembly.FullName);

                    postgres.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5));
                });
        });

        services.AddScoped<IDatabaseContext>(
            provider => provider.GetRequiredService<EfPostgresDbContext>());

        return services;
    }
}

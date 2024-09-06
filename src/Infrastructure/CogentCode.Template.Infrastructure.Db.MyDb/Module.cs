namespace CogentCode.Template.Infrastructure.Db.MyDb
{
    using CogentCode.Template.Application.Contracts;
    using CogentCode.Template.Infrastructure.Db.MyDb.Internal;
    using CogentCode.Template.Infrastructure.Db.MyDb.Internal.Repositories;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    public static class Module
    {
        public static IServiceCollection AddInfrastructureMyDbConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetRequiredSection(MyDbSettings.Key).Get<MyDbSettings>() ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<MyDbContext>(options => options.UseNpgsql(settings.ConnectionString));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IApplicationBuilder MigrateMyDb(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();

            using var dbContext = scope.ServiceProvider.GetService<MyDbContext>();

            if (dbContext is null)
            {
                throw new ApplicationException(nameof(dbContext));
            }

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
            else
            {
                throw new InvalidOperationException(
                    $"""Create "InitialCreate" migration so that database can be created""");
            }

            return builder;
        }
    }

    internal class MyDbSettings
    {
        public const string Key = nameof(MyDbSettings);
        public string? ConnectionString { get; set; } = default;
    }
}

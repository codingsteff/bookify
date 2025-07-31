using Asp.Versioning;
using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Caching;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Email;
using Bookify.Application.Authentication;
using Bookify.Domain.Shared;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Authentication;
using Bookify.Infrastructure.Authorization;
using Bookify.Infrastructure.Caching;
using Bookify.Infrastructure.Clock;
using Bookify.Infrastructure.Data;
using Bookify.Infrastructure.Email;
using Bookify.Infrastructure.Outbox;
using Bookify.Infrastructure.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;

namespace Bookify.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();

        AddPersitence(services, configuration);
        AddAuthentication(services, configuration);
        AddKeycloak(services, configuration);
        AddAuthorization(services, configuration);
        AddCaching(services, configuration);
        AddHealthChecks(services, configuration);
        AddApiVersioning(services);
        AddBackgroundJobs(services, configuration);

        return services;
    }

    private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication() // JwtBearerDefaults.AuthenticationScheme) it's not necessary anymore in .NET 8
            .AddJwtBearer();

        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
        services.ConfigureOptions<JwtBearerOptionsSetup>();
    }

    private static void AddKeycloak(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));
        services.AddTransient<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAuthenticationService, AuthenticationService>((ServiceProvider, httpClient) =>
            {
                var keycloakOptions = ServiceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
                httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IJwtService, JwtService>((ServiceProvider, httpClient) =>
            {
                var keycloakOptions = ServiceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
                httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
            });
    }

    private static void AddAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        services.AddScoped<AuthorizationService>();
        services.AddTransient<Microsoft.AspNetCore.Authentication.IClaimsTransformation, CustomClaimsTransformation>();

        services.AddTransient<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }

    private static void AddPersitence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ??
                              throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention(); // PostgreSQL uses snake_case instead of CamelCase (SQL Server)
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IApartmentRepository, ApartmentRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        // use service provider to resolve ApplicationDbContext as the implementation of IUnitOfWork
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler()); // Register custom Dapper type handler for DateOnly conversion
    }

    private static void AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Cache") ??
                              throw new ArgumentNullException(nameof(configuration));

        services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);
        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!)
            .AddRedis(configuration.GetConnectionString("Cache")!)
            .AddUrlGroup(new Uri(configuration["Keycloak:BaseUrl"]!), HttpMethod.Get, name: "keycloak");
    }

    private static void AddApiVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc();
    }

    private static void AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));
        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true); // Wait for jobs to complete before shutting down
        services.ConfigureOptions<OutboxProcessorJobSetup>();
    }

}
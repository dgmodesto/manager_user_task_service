namespace Sdk.Api;

using Interfaces;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class Setup
{
    public static IServiceCollection AddApiSdk(
        this IServiceCollection services,
        Action<IApiSettings> setupAction)
    {
        services
            .AddApiWithMediatR(setupAction)
            .AddApiVersioningWithSwagger(setupAction)
            .AddJsonOptions();

        return services;
    }

    public static WebApplication UseApiSdk(
        this WebApplication app,
        string? apiName = null,
        Action<IHealthCheckOverride>? healthChecksSettings = null)
    {
        app.UseHealthChecks(healthChecksSettings);

        if (!app.Environment.IsDevelopment())
            return app;

        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwagger();
        app.UseSwaggerUI(o =>
        {

            if (string.IsNullOrEmpty(apiName)) apiName = "API";

            foreach (var description in provider.ApiVersionDescriptions)
                o.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    $"{apiName} - {description.GroupName.ToUpper()}"
                );
        });

        app.UseReDoc(c =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                c.DocumentTitle = $"{apiName} - {description.GroupName.ToUpper()} - REDOC API Documentation";
                c.SpecUrl = $"/swagger/{description.GroupName}/swagger.json";
            }
        });

        return app;
    }

    #region private methods

    private static IServiceCollection AddApiWithMediatR(
        this IServiceCollection services,
        Action<IApiSettings> setupAction)
    {
        IApiSettings settings = new ApiSettings();
        setupAction.Invoke(settings);

        if (settings.MediatorHandlersAssembyType is null)
            throw new InvalidOperationException($"{nameof(settings.MediatorHandlersAssembyType)} is null");

        services.AddMediatorSdk(config =>
        {
            config.ValidatorHandlerAssemblyType = settings.ValidatorHandlerAssemblyType;
            config.MediatorHandlersAssembyType = settings.MediatorHandlersAssembyType;
        });

        return services;
    }

    private static IServiceCollection AddApiVersioningWithSwagger(
        this IServiceCollection services,
        Action<IApiSettings> setupAction)
    {
        IApiSettings settings = new ApiSettings();
        setupAction.Invoke(settings);

        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(ConfigureSwaggerGenOptions)
            .AddApiVersioning();

        services.AddTransient<IApiOptions>(_ => new ApiOptions(settings.ApiName));
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

        return services;
    }

    private static IServiceCollection AddJsonOptions(this IServiceCollection services)
    {
        // Configure enums in json responses
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        return services;
    }

    // https://markgossa.com/2022/05/asp-net-6-api-versioning-swagger.html
    // https://blog.christian-schou.dk/how-to-use-api-versioning-in-net-core-web-api/
    private static IServiceCollection AddApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(o =>
        {
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
            o.ReportApiVersions = true;
            o.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        services.AddVersionedApiExplorer(o =>
        {
            o.GroupNameFormat = "'v'VVV";
            o.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    private static void ConfigureSwaggerGenOptions(SwaggerGenOptions o)
    {
        o.OperationFilter<SwaggerDefaultValues>();
        o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });

        o.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    }

    private static IApplicationBuilder UseHealthChecks(
        this IApplicationBuilder app,
        Action<IHealthCheckOverride>? overrideConfig = null)
    {
        IHealthCheckOverride healthCheckOverride;
        if (overrideConfig is { })
        {
            healthCheckOverride = new HealthCheckOverride();
            overrideConfig.Invoke(healthCheckOverride);
        }
        else
        {
            healthCheckOverride = new HealthCheckOverride();
        }
        app.UseRouting();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {

            endpoints.MapGet(healthCheckOverride.LivenessRoute, async context =>
            {
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(
                        new HealthCheckResult(
                            HealthStatus.Healthy,
                            description: "HealthCheck",
                            exception: null,
                            data: null),
                        new JsonSerializerOptions
                        {
                            WriteIndented = false,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        }));
            });

            endpoints.MapGet(healthCheckOverride.ReadinessRoute, async context =>
            {
                var healthCheckService = app.ApplicationServices.GetService<HealthCheckService>() ??
                                         throw new ArgumentException(
                                             "app.ApplicationServices.GetService<HealthCheckService>()");

                var report = await healthCheckService.CheckHealthAsync();

                if (report.Entries.Any(c => c.Value.Status != HealthStatus.Healthy))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(
                        report.Entries.ToList(),
                        new JsonSerializerOptions
                        {
                            WriteIndented = false,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        }));
                }
                else
                {
                    await context.Response.WriteAsync(JsonSerializer.Serialize(
                        report.Entries.ToList(),
                        new JsonSerializerOptions
                        {
                            WriteIndented = false,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        }));
                }
            });
        });

        return app;
    }

    #endregion
}
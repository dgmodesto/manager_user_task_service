using ManagerUserTaskApi.Api;
using Sdk.Api;
using Sdk.Api.Errors;
using Sdk.Api.HealthCheck;
using Sdk.Api.Prometheus;
using Serilog;

const string apiName = "ManagerUserTaskApi.Api";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddControllers();
builder.Services.AddDependencies(builder.Configuration, apiName);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiSdk(apiName);
app.UseMigrations();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.UseHealthCheckConfiguration();
app.UsePrometheusConfiguration();
app.Run();


using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace Sdk.Api.Prometheus;

public static class PrometheusConfig
{

    public static void AddPrometheusConfig(this IServiceCollection services)
    {

    }


    public static void UsePrometheusConfiguration(this IApplicationBuilder app)
    {
        // Custom Metrics to count requests for each endpoint and the method
        var counter = Metrics.CreateCounter("ManagerUserTaskMetric", "Counts requests to the ManagerUserTaskMetrics API endpoints",
            new CounterConfiguration
            {
                LabelNames = new[] { "method", "endpoint" }
            });

        app.Use((context, next) =>
        {
            counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
            return next();
        });

        // Use the prometheus middleware
        app.UseMetricServer();
        app.UseHttpMetrics();
    }
}

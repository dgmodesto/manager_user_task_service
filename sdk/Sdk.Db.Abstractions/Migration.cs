namespace Sdk.Db.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public sealed class Migration
{
    private readonly IServiceProvider _serviceProvider;

    public Migration(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void WithEventStore<TContext>()
        where TContext : DbContext
    {
        Migrate<TContext>();
    }

    public void Migrate<TContext>()
     where TContext : DbContext
    {
        var service = _serviceProvider.GetService<IServiceScopeFactory>();
        if (service == null)
            throw new InvalidOperationException(
                $"Instance of {nameof(IServiceScopeFactory)} is null for performing migrations");

        using (var scope = service.CreateScope())
        {
            try
            {
                using var domainContext = scope.ServiceProvider.GetRequiredService<TContext>();
                domainContext.Database.Migrate();
            }
            catch (Exception)
            {
                Console.WriteLine($"Migrations for {typeof(TContext).Name} have already been applied.");
                return;
            }
        }
    }
}
namespace ManagerUserTaskApi.Infrastructure.Database;

using Interfaces;
using ManagerUserTaskApi.Infrastructure.Database.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories;
using Sdk.Db.EventStore.Interfaces;
using Sdk.Db.EventStore.Repositories;
using Sdk.Db.PostgreSQL;

public static class SetupDatabase
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // event store repository for auditing events
        services.AddScoped<IEventStoreRepository, EventStoreRepository<EventStoreDbContext>>();

        // domain repositories
        services.AddScoped<IUserTaskRepository, UserTaskRepository>();

        return services;
    }

    public static IHost UseDatabaseMigrations(this IHost app)
    {
        // Run migrations
        app.Services
            .MigrateDatabase<ManagerUserTaskApiDbContext>()
            .WithEventStore<EventStoreDbContext>();

        ////Apply Seed
        //using (var scope = app.Services.CreateScope())
        //{
        //    var services = scope.ServiceProvider;
        //    var context = services.GetRequiredService<ManagerUserTaskApiDbContext>();
        //    Seed(context);
        //}

        return app;
    }

    public static void Seed(ManagerUserTaskApiDbContext context)
    {
        if (!context.UserTasks.Any())
        {
            var tasks = new List<UserTask>
            {
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 7), StartTime = new DateTime(2024, 6, 7, 8, 0, 0), EndTime = new DateTime(2024, 6, 7, 9, 0, 0), Subject = "Team Meeting", Description = "Team Meeting" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 7), StartTime = new DateTime(2024, 6, 7, 9, 30, 0), EndTime = new DateTime(2024, 6, 7, 10, 30, 0), Subject = "Project Planning Session", Description = "Project Planning Session" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 7), StartTime = new DateTime(2024, 6, 7, 13, 0, 0), EndTime = new DateTime(2024, 6, 7, 14, 0, 0), Subject = "Client Call", Description = "Client Call" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 7), StartTime = new DateTime(2024, 6, 7, 15, 0, 0), EndTime = new DateTime(2024, 6, 7, 16, 0, 0), Subject = "Code Review", Description = "Code Review" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 7), StartTime = new DateTime(2024, 6, 7, 16, 30, 0), EndTime = new DateTime(2024, 6, 7, 17, 30, 0), Subject = "Prepare Presentation", Description = "Prepare Presentation for Tomorrow's Meeting" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 8), StartTime = new DateTime(2024, 6, 8, 9, 0, 0), EndTime = new DateTime(2024, 6, 8, 10, 0, 0), Subject = "Presentation Meeting", Description = "Presentation Meeting" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 8), StartTime = new DateTime(2024, 6, 8, 10, 30, 0), EndTime = new DateTime(2024, 6, 8, 11, 30, 0), Subject = "Development Sprint Kickoff", Description = "Development Sprint Kickoff" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 8), StartTime = new DateTime(2024, 6, 8, 13, 0, 0), EndTime = new DateTime(2024, 6, 8, 14, 0, 0), Subject = "Design Review", Description = "Design Review" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 8), StartTime = new DateTime(2024, 6, 8, 15, 0, 0), EndTime = new DateTime(2024, 6, 8, 16, 0, 0), Subject = "Marketing Strategy Discussion", Description = "Marketing Strategy Discussion" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 9), StartTime = new DateTime(2024, 6, 9, 8, 0, 0), EndTime = new DateTime(2024, 6, 9, 9, 0, 0), Subject = "Daily Standup", Description = "Daily Standup" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 9), StartTime = new DateTime(2024, 6, 9, 9, 30, 0), EndTime = new DateTime(2024, 6, 9, 10, 30, 0), Subject = "Feature Planning", Description = "Feature Planning" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 9), StartTime = new DateTime(2024, 6, 9, 13, 0, 0), EndTime = new DateTime(2024, 6, 9, 14, 0, 0), Subject = "Testing New Features", Description = "Testing New Features" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 9), StartTime = new DateTime(2024, 6, 9, 15, 0, 0), EndTime = new DateTime(2024, 6, 9, 16, 0, 0), Subject = "Bug Fixing Session", Description = "Bug Fixing Session" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 10), StartTime = new DateTime(2024, 6, 10, 8, 0, 0), EndTime = new DateTime(2024, 6, 10, 9, 0, 0), Subject = "Project Update Meeting", Description = "Project Update Meeting" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 10), StartTime = new DateTime(2024, 6, 10, 9, 30, 0), EndTime = new DateTime(2024, 6, 10, 10, 30, 0), Subject = "Research and Development", Description = "Research and Development" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 10), StartTime = new DateTime(2024, 6, 10, 13, 0, 0), EndTime = new DateTime(2024, 6, 10, 14, 0, 0), Subject = "Team Retrospective", Description = "Team Retrospective" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 10), StartTime = new DateTime(2024, 6, 10, 15, 0, 0), EndTime = new DateTime(2024, 6, 10, 16, 0, 0), Subject = "Product Demo Preparation", Description = "Product Demo Preparation" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 11), StartTime = new DateTime(2024, 6, 11, 9, 0, 0), EndTime = new DateTime(2024, 6, 11, 10, 0, 0), Subject = "Weekly Sync-Up", Description = "Weekly Sync-Up" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 11), StartTime = new DateTime(2024, 6, 11, 10, 30, 0), EndTime = new DateTime(2024, 6, 11, 11, 30, 0), Subject = "Customer Feedback Analysis", Description = "Customer Feedback Analysis" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 11), StartTime = new DateTime(2024, 6, 11, 13, 0, 0), EndTime = new DateTime(2024, 6, 11, 14, 0, 0), Subject = "Feature Implementation", Description = "Feature Implementation" },
                new UserTask { User = "dgmodesto", Date = new DateTime(2024, 6, 11), StartTime = new DateTime(2024, 6, 11, 15, 0, 0), EndTime = new DateTime(2024, 6, 11, 16, 0, 0), Subject = "Documentation Update", Description = "Documentation Update" }
            };

            context.UserTasks.AddRange(tasks);
            context.SaveChanges();
        }
    }
}
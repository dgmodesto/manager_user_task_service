using Ocelot.Authorization;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Sdk.Api.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiAuthentication();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);


var app = builder.Build();


app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

var configuration = new OcelotPipelineConfiguration
{
    AuthenticationMiddleware = async (ctx, next) =>
    {
        string token = ctx.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        if (string.IsNullOrEmpty(token))
        {
            ctx.Items.SetError(new UnauthorizedError("Invalid Token Jwt"));
            return;
        }

        await next.Invoke();
    }
};
app.UseOcelot(configuration).Wait();

app.Run();

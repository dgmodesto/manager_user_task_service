using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Sdk.Api.Authentication;

public static class SetupAuthentication
{
    public const string SecurityKey = "secretJWTsigningKey@123";

    public static IServiceCollection AddApiAuthentication(
    this IServiceCollection services)
    {

        var providerUri = Environment.GetEnvironmentVariable("IDP_BaseUrl") ?? string.Empty;

        services.AddAuthentication("IdentityApiKey")
        .AddJwtBearer("IdentityApiKey", options =>
       {
           options.Authority = $"{providerUri}/realms/ManagerUserTaskApiRealm"; // Replace with your Keycloak server and realm
           options.Audience = "ManagerUserTaskApi-client-id"; // Replace with your Keycloak client ID
           options.RequireHttpsMetadata = false;
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuerSigningKey = true,
               ValidateIssuer = true,
               ValidateAudience = false,
               ValidIssuer = $"{providerUri}/realms/ManagerUserTaskApiRealm", // Replace with your Keycloak server and realm
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey))
           };
       });

        return services;
    }
}

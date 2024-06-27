using ManagerUserTaskApi.Infrastructure.Services.Authentication.Requests;
using ManagerUserTaskApi.Infrastructure.Services.Authentication.Responses;
using Microsoft.Extensions.Logging;
using Sdk.Api.Exceptions;
using System.Net;
using System.Text.Json;

namespace ManagerUserTaskApi.Infrastructure.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{

    private readonly HttpClient _client;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(HttpClient client, ILogger<AuthenticationService> logger)
    {
        _client = client;
        _logger = logger;


        var baseUrl = Environment.GetEnvironmentVariable("IDP_BaseUrl") ?? string.Empty;
        _client.BaseAddress = new Uri(baseUrl);
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        try
        {
            var path = Environment.GetEnvironmentVariable("IDP_PATH_Auth") ?? string.Empty;
            string clientId = Environment.GetEnvironmentVariable("REALM_LOOKUP_ClientId") ?? string.Empty;
            string clientSecret = Environment.GetEnvironmentVariable("REALM_LOOKUP_ClientSecret") ?? string.Empty;
            string realm = Environment.GetEnvironmentVariable("REALM_LOOKUP_Name") ?? string.Empty;
            var builder = new UriBuilder(_client.BaseAddress!)
            {
                Path = string.Format(path, realm)
            };

            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("username", request.Username),
                new KeyValuePair<string, string>("password", request.Password)
            });

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, builder.Uri);
            httpRequest.Content = requestBody;

            var result = await _client.SendAsync(httpRequest);
            var resultContent = await result.Content.ReadAsStringAsync();

            if (result.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<LoginResponse>(resultContent);
                return response;

            }

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException();
            }

            if (result.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new ForbiddenException();
            }

            if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                var detail = JsonSerializer.Deserialize<IdPError>(resultContent);
                throw new BadRequestException(detail is null ? "" : detail.Description);
            }

            _logger.LogWarning("Fail to login user {Username}, status code {StatusCode}, detail {Detail}",
                request.Username, result.StatusCode, resultContent);

            throw new UnauthorizedException("Fail to login, please see logs");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[{MethodName}] - Error when try to login user {Username}", nameof(Login),
                request.Username);

            throw;
        }
    }


}

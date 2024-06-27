namespace ManagerUserTaskApi.Infrastructure.Services.Authentication.Config;

public class RealmLookupOptions
{
    public string IdentityBaseUrl { get; set; } = "";
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
    public string RealmBaseUrl { get; set; } = "";
}

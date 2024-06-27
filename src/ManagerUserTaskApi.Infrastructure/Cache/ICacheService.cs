namespace ManagerUserTaskApi.Infrastructure.Cache;

public interface ICacheService<TModel> where TModel : class
{
    Task SetStringAsync(string key, string value, TimeSpan? expiry = null);
    Task<TModel> GetStringAsync<TModel>(string key);

}
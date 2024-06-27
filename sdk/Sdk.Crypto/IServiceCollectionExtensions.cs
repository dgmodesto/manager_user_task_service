using Microsoft.Extensions.DependencyInjection;

namespace Sdk.Crypto
{
    public static class IServiceCollectionExtensions
    {
        public static void AddPersonalCryptoService(
            this IServiceCollection services)
        {
            services.AddSingleton<IPersonalCryptoService, PersonalCryptoService>();
        }
    }
}

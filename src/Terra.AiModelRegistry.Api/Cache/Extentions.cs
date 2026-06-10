using Cite.Tools.Cache;
using Terra.AiModelRegistry.App.Exception;

namespace Terra.AiModelRegistry.Api.Cache
{
    public static class Extensions
    {
        public static IServiceCollection AddCacheServices(
            this IServiceCollection services,
            IConfigurationSection cacheConfigurationSection)
        {
            ProviderType type = cacheConfigurationSection.GetValue("Type", ProviderType.None);

            switch (type)
            {
                case ProviderType.None:
                    {
                        services.AddDistributedNullCache();
                        break;
                    }
                case ProviderType.InProc:
                    {
                        services.AddDistributedMemoryCache();
                        break;
                    }
                default: throw new TerraApplicationException($"unrecognized cache provider type {type}");
            }

            return services;
        }
    }
}

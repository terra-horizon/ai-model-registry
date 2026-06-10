using Cite.Tools.Configuration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Terra.AiModelRegistry.App.Formatting
{
	public static class Extensions
	{
		public static IServiceCollection AddFormattingServices(
			this IServiceCollection services,
			IConfigurationSection configurationSection,
			IConfigurationSection cacheConfigurationSection)
		{
			services.AddScoped<IFormattingService, FormattingService>();
			services.ConfigurePOCO<FormattingServiceConfig>(configurationSection);

			return services;
		}
	}
}

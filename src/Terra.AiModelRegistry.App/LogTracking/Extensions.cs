using Cite.Tools.Configuration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Terra.AiModelRegistry.App.LogTracking
{
	public static class Extensions
	{
		public static IServiceCollection AddLogTrackingServices(
			this IServiceCollection services,
			IConfigurationSection logTrackingCorrelationConfigurationSection,
			IConfigurationSection logTrackingPrincipalConfigurationSection)
		{
			services.ConfigurePOCO<LogTrackingCorrelationConfig>(logTrackingCorrelationConfigurationSection);
			services.AddScoped<LogCorrelationScope>();

			services.ConfigurePOCO<LogTrackingPrincipalConfig>(logTrackingPrincipalConfigurationSection);

			return services;
		}
	}
}

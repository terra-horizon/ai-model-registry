using Microsoft.Extensions.DependencyInjection;

namespace Terra.AiModelRegistry.App.Service.Convention
{
	public static class Extensions
	{
		public static IServiceCollection AddConventionServices(this IServiceCollection services)
		{
			services.AddSingleton<IConventionService, ConventionService>();
			return services;
		}
	}
}

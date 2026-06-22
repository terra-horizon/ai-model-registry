using Microsoft.Extensions.DependencyInjection;

namespace Terra.AiModelRegistry.App.Service.AiModel
{
	public static class Extensions
	{
		public static IServiceCollection AddAiModelServices(this IServiceCollection services)
		{
			services.AddScoped<IAiModelService, AiModelService>();
			return services;
		}
	}
}

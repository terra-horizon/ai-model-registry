using Microsoft.Extensions.DependencyInjection;

namespace Terra.AiModelRegistry.App.Event
{
	public static class Extensions
	{
		public static IServiceCollection AddEventBroker(this IServiceCollection services)
		{
			services.AddSingleton<EventBroker>();

			return services;
		}
	}
}

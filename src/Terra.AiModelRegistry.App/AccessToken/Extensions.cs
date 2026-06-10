using Microsoft.Extensions.DependencyInjection;

namespace Terra.AiModelRegistry.App.AccessToken
{
	public static class Extensions
	{
		public static IServiceCollection AddAccessTokenServices(this IServiceCollection services)
		{
			services
				.AddScoped<RequestTokenIntercepted>()
				.AddTransient<IAccessTokenService, AccessTokenService>();

			return services;
		}
	}
}

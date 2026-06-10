using Microsoft.AspNetCore.Authorization;

namespace Terra.AiModelRegistry.Api.Authorization
{
	public class OwnedResourceRequirement : IAuthorizationRequirement
	{
		public OwnedResourceRequirement() { }
	}
}


namespace Terra.AiModelRegistry.App.Authorization
{
	public interface IAuthorizationContentResolver
	{
        public Boolean HasAuthenticated();

        public String CurrentUser();

        public Task<Boolean> HasPermission(params String[] permissions);

        public ISet<String> PermissionsOfContextRoles(IEnumerable<String> roles);
    }
}

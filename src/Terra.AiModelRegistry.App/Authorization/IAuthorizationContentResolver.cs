
namespace Terra.AiModelRegistry.App.Authorization
{
	public interface IAuthorizationContentResolver
	{
		Boolean HasAuthenticated();
		String CurrentUser();

		Task<Guid?> CurrentUserId();
		Task<String> SubjectIdOfCurrentUser();
		Task<String> SubjectIdOfUserId(Guid? userId);
		Task<String> SubjectIdOfUserIdentifier(String userIdentifier);

		Task<Boolean> HasPermission(params String[] permissions);

		ISet<String> PermissionsOfContextRoles(IEnumerable<String> roles);

		Task<List<String>> ContextRolesOf();
	}
}

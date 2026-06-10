using Cite.Tools.Auth.Claims;
using Cite.Tools.Data.Query;
using Cite.WebTools.CurrentPrincipal;

namespace Terra.AiModelRegistry.App.Authorization
{
	public class AuthorizationContentResolver : IAuthorizationContentResolver
	{
		private readonly ICurrentPrincipalResolverService _currentPrincipalResolverService;
		private readonly IAuthorizationService _authorizationService;
		private readonly ClaimExtractor _extractor;
		private readonly IPermissionPolicyService _permissionPolicyService;
		private readonly QueryFactory _queryFactory;

		public AuthorizationContentResolver(
			ICurrentPrincipalResolverService currentPrincipalResolverService,
			IAuthorizationService authorizationService,
			IPermissionPolicyService permissionPolicyService,
			QueryFactory queryFactory,
			ClaimExtractor extractor)
		{
			this._currentPrincipalResolverService = currentPrincipalResolverService;
			this._authorizationService = authorizationService;
			this._permissionPolicyService = permissionPolicyService;
			this._queryFactory = queryFactory;
			this._extractor = extractor;
		}

		public Boolean HasAuthenticated()
		{
			return this._currentPrincipalResolverService.CurrentPrincipal() != null;
		}

		public String CurrentUser()
		{
			String currentUser = this._extractor.SubjectString(this._currentPrincipalResolverService.CurrentPrincipal());
			return currentUser;
		}

		public async Task<Boolean> HasPermission(params String[] permissions)
		{
			return await this._authorizationService.Authorize(permissions);
		}

		public ISet<String> PermissionsOfContextRoles(IEnumerable<String> roles)
		{
			return this._permissionPolicyService.PermissionsOfContext(roles);
		}
	}
}

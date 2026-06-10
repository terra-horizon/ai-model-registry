using Cite.Tools.Auth.Claims;
using Cite.Tools.Data.Query;
using Cite.WebTools.CurrentPrincipal;
using Terra.AiModelRegistry.App.Common.Auth;
using Terra.AiModelRegistry.App.Query;
using Terra.AiModelRegistry.App.Service.AAI;

namespace Terra.AiModelRegistry.App.Authorization
{
	public class AuthorizationContentResolver : IAuthorizationContentResolver
	{
		private readonly ICurrentPrincipalResolverService _currentPrincipalResolverService;
		private readonly IAuthorizationService _authorizationService;
		private readonly ClaimExtractor _extractor;
		private readonly IPermissionPolicyService _permissionPolicyService;
		private readonly QueryFactory _queryFactory;
		private readonly IAAIService _aaiService;

		public AuthorizationContentResolver(
			ICurrentPrincipalResolverService currentPrincipalResolverService,
			IAuthorizationService authorizationService,
			IPermissionPolicyService permissionPolicyService,
			IAAIService aaiService,
			QueryFactory queryFactory,
			ClaimExtractor extractor)
		{
			this._currentPrincipalResolverService = currentPrincipalResolverService;
			this._authorizationService = authorizationService;
			this._permissionPolicyService = permissionPolicyService;
			this._queryFactory = queryFactory;
			this._aaiService = aaiService;
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

		public async Task<Guid?> CurrentUserId()
		{
			String currentUser = this.CurrentUser();
			if (String.IsNullOrEmpty(currentUser)) return null;
			Guid userId = await this._queryFactory.Query<UserQuery>().IdpSubjectIds(currentUser).DisableTracking().FirstAsync(x=> x.Id);
			if(userId == default(Guid)) return null;
			return userId;
		}

		public async Task<String> SubjectIdOfCurrentUser()
		{
			Guid? currentUserId = await this.CurrentUserId();
			return await this.SubjectIdOfUserId(currentUserId);
		}

		public async Task<String> SubjectIdOfUserId(Guid? userId)
		{
			if(!userId.HasValue) return null;
			String subjectId = await this._queryFactory.Query<UserQuery>().Ids(userId.Value).DisableTracking().FirstAsync(x => x.IdpSubjectId);
			if (String.IsNullOrEmpty(subjectId)) return null;
			return subjectId;
		}

		public async Task<String> SubjectIdOfUserIdentifier(String userIdentifier)
		{
			if (String.IsNullOrEmpty(userIdentifier)) return null;
			String subjectId = null;
			if (Guid.TryParse(userIdentifier, out Guid userId))
			{
				subjectId = await this._queryFactory.Query<UserQuery>().Ids(userId).DisableTracking().FirstAsync(x => x.IdpSubjectId);
				if (!String.IsNullOrEmpty(subjectId)) return subjectId;
			}
			if(String.IsNullOrEmpty(subjectId))
			{
				subjectId = await this._queryFactory.Query<UserQuery>().IdpSubjectIds(userIdentifier).DisableTracking().FirstAsync(x => x.IdpSubjectId);
			}
			if (String.IsNullOrEmpty(subjectId)) return null;
			return subjectId;
		}

		public async Task<Boolean> HasPermission(params String[] permissions)
		{
			return await this._authorizationService.Authorize(permissions);
		}

		public ISet<String> PermissionsOfContextRoles(IEnumerable<String> roles)
		{
			return this._permissionPolicyService.PermissionsOfContext(roles);
		}

		public async Task<List<String>> ContextRolesOf()
		{
			String subjectId = await this.SubjectIdOfCurrentUser();
			if (String.IsNullOrEmpty(subjectId)) return Enumerable.Empty<String>().ToList();

			List<ContextGrant> grants = await this._aaiService.LookupUserContextGrants(subjectId);
			List<String> accesses = grants.Select(x => x.Role).Distinct().ToList();

			return accesses;
		}
	}
}

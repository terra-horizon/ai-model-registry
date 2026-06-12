
namespace Terra.AiModelRegistry.App.Authorization
{
	public static class Permission
	{
		//Authorization
		public const String LookupContextGrantOther = "LookupContextGrantOther";
		public const String LookupContextGrantGroups = "LookupContextGrantGroups";
		public const String AddUserToContextGrantGroup = "AddUserToContextGrantGroup";
		public const String RemoveUserFromContextGrantGroup = "RemoveUserFromContextGrantGroup";
		//AiModelDefinition
		public const String CreateAiModelDefinition = "CreateAiModelDefinition";
		public const String EditAiModelDefinition = "EditAiModelDefinition";
		//User
		public const String BrowseUser = "BrowseUser";
		public const String BrowseUserGroup = "BrowseUserGroup";
	}
}

using Cite.Tools.FieldSet;

namespace Terra.AiModelRegistry.App.Service.Version
{
	public interface IVersionInfoService
	{
		Task<List<Model.VersionInfo>> CurrentAsync(IFieldSet fields = null);
	}
}

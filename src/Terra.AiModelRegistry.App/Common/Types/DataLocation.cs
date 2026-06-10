using Terra.AiModelRegistry.App.Common.Enum;

namespace Terra.AiModelRegistry.App.Common.Types
{
	public class DataLocation
	{
		public ModelReferenceKind Kind { get; set; }
		public string Location { get; set; }
	}
}

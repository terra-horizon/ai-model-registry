using MongoDB.Bson;

namespace Terra.AiModelRegistry.App.Event
{
	public struct OnAiModelDefinitionTouchedArgs
	{
		public OnAiModelDefinitionTouchedArgs(IEnumerable<ObjectId> aiModelDefinitionIds)
		{
			this.AiModelDefinitionIds = aiModelDefinitionIds;
		}

		public IEnumerable<ObjectId> AiModelDefinitionIds { get; private set; }
	}
}

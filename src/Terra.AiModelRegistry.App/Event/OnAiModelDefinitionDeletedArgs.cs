using MongoDB.Bson;

namespace Terra.AiModelRegistry.App.Event
{
	public struct OnAiModelDefinitionDeletedArgs
	{
		public OnAiModelDefinitionDeletedArgs(IEnumerable<ObjectId> aiModelDefinitionIds)
		{
			this.AiModelDefinitionIds = aiModelDefinitionIds;
		}

		public IEnumerable<ObjectId> AiModelDefinitionIds { get; private set; }
	}
}

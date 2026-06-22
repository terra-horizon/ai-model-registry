namespace Terra.AiModelRegistry.App.Event
{
	public struct OnAiModelDefinitionDeletedArgs
	{
		public OnAiModelDefinitionDeletedArgs(IEnumerable<Guid> aiModelDefinitionIds)
		{
			this.AiModelDefinitionIds = aiModelDefinitionIds;
		}

		public IEnumerable<Guid> AiModelDefinitionIds { get; private set; }
	}
}

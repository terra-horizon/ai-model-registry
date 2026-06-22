namespace Terra.AiModelRegistry.App.Event
{
	public struct OnAiModelDefinitionTouchedArgs
	{
		public OnAiModelDefinitionTouchedArgs(IEnumerable<Guid> aiModelDefinitionIds)
		{
			this.AiModelDefinitionIds = aiModelDefinitionIds;
		}

		public IEnumerable<Guid> AiModelDefinitionIds { get; private set; }
	}
}

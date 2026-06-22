
namespace Terra.AiModelRegistry.App.Event
{
	public class EventBroker
	{
		#region AiModelDefinition Touched

		private EventHandler<OnAiModelDefinitionTouchedArgs> _AiModelDefinitionTouched;
		public event EventHandler<OnAiModelDefinitionTouchedArgs> AiModelDefinitionTouched
		{
			add { this._AiModelDefinitionTouched += value; }
			remove { this._AiModelDefinitionTouched -= value; }
		}

		public void EmitAiModelDefinitionTouched(Guid aiModelDefinitionId)
		{
			this.EmitAiModelDefinitionTouched(this, new List<Guid>() { aiModelDefinitionId });
		}

		public void EmitAiModelDefinitionTouched(IEnumerable<Guid> aiModelDefinitionIds)
		{
			this.EmitAiModelDefinitionTouched(this, aiModelDefinitionIds);
		}

		public void EmitAiModelDefinitionTouched(IEnumerable<OnAiModelDefinitionTouchedArgs> events)
		{
			this.EmitAiModelDefinitionTouched(this, events);
		}

		public void EmitAiModelDefinitionTouched(object sender, IEnumerable<Guid> aiModelDefinitionIds)
		{
			this._AiModelDefinitionTouched?.Invoke(sender, new OnAiModelDefinitionTouchedArgs(aiModelDefinitionIds));
		}

		public void EmitAiModelDefinitionTouched(object sender, IEnumerable<OnAiModelDefinitionTouchedArgs> events)
		{
			if (events == null) return;
			foreach (OnAiModelDefinitionTouchedArgs ev in events) this._AiModelDefinitionTouched?.Invoke(sender, ev);
		}

		#endregion

		#region AiModelDefinition Deleted

		private EventHandler<OnAiModelDefinitionDeletedArgs> _AiModelDefinitionDeleted;
		public event EventHandler<OnAiModelDefinitionDeletedArgs> AiModelDefinitionDeleted
		{
			add { this._AiModelDefinitionDeleted += value; }
			remove { this._AiModelDefinitionDeleted -= value; }
		}

		public void EmitAiModelDefinitionDeleted(Guid aiModelDefinitionId)
		{
			this.EmitAiModelDefinitionDeleted(this, new List<Guid>() { aiModelDefinitionId });
		}

		public void EmitAiModelDefinitionDeleted(IEnumerable<Guid> aiModelDefinitionIds)
		{
			this.EmitAiModelDefinitionDeleted(this, aiModelDefinitionIds);
		}

		public void EmitAiModelDefinitionDeleted(object sender, IEnumerable<Guid> aiModelDefinitionIds)
		{
			this._AiModelDefinitionDeleted?.Invoke(sender, new OnAiModelDefinitionDeletedArgs(aiModelDefinitionIds));
		}

		#endregion
	}
}

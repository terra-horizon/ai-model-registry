using Cite.Tools.Data.Builder;
using Cite.Tools.Data.Query;
using Cite.Tools.FieldSet;
using Cite.Tools.Json;
using Cite.Tools.Logging;
using Cite.Tools.Logging.Extensions;
using Microsoft.Extensions.Logging;
using Terra.AiModelRegistry.App.Authorization;
using Terra.AiModelRegistry.App.Service.Convention;

namespace Terra.AiModelRegistry.App.Model.Builder
{
	public class AiModelDefinitionBuilder : Builder<Model.AiModelDefinition, Data.AiModelDefinition>
	{
		private readonly QueryFactory _queryFactory;
		private readonly BuilderFactory _builderFactory;
		private readonly IAuthorizationContentResolver _authorizationContentResolver;
		private readonly JsonHandlingService _jsonHandlingService;
		private AuthorizationFlags _authorize { get; set; } = AuthorizationFlags.None;

		public AiModelDefinitionBuilder(
			QueryFactory queryFactory,
			IConventionService conventionService,
			BuilderFactory builderFactory,
			IAuthorizationContentResolver authorizationContentResolver,
			ILogger<AiModelDefinitionBuilder> logger,
			JsonHandlingService jsonHandlingService) : base(conventionService, logger)
		{
			this._queryFactory = queryFactory;
			this._builderFactory = builderFactory;
			this._authorizationContentResolver = authorizationContentResolver;
			this._jsonHandlingService = jsonHandlingService;
		}

		public AiModelDefinitionBuilder Authorize(AuthorizationFlags flags) { this._authorize = flags; return this; }

		public override async Task<List<Model.AiModelDefinition>> Build(IFieldSet fields, IEnumerable<Data.AiModelDefinition> datas)
		{
			this._logger.Debug("building for {count} items requesting {fields} fields", datas?.Count(), fields?.Fields?.Count);
			this._logger.Trace(new DataLogEntry("requested fields", fields));
			if (fields == null || fields.IsEmpty()) return Enumerable.Empty<Model.AiModelDefinition>().ToList();

			List<Model.AiModelDefinition> models = [];
			foreach (Data.AiModelDefinition d in datas ?? [])
			{
				Model.AiModelDefinition m = new Model.AiModelDefinition();
				if (fields.HasField(nameof(Model.AiModelDefinition.Hash))) m.Hash = this.HashValue(d.UpdatedAt);
				if (fields.HasField(nameof(Model.AiModelDefinition.Id))) m.Id = d.Id;
				if (fields.HasField(nameof(Model.AiModelDefinition.Name))) m.Name = d.Name;
				if (fields.HasField(nameof(Model.AiModelDefinition.Description))) m.Description = d.Description;
				if (fields.HasField(nameof(Model.AiModelDefinition.Version))) m.Version = d.Version;
				if (fields.HasField(nameof(Model.AiModelDefinition.Metadata))) m.Metadata = this._jsonHandlingService.FromJsonSafe<Dictionary<string, object>>(d.Metadata);
				if (fields.HasField(nameof(Model.AiModelDefinition.IsActive))) m.IsActive = d.IsActive;
				if (fields.HasField(nameof(Model.AiModelDefinition.CreatedAt))) m.CreatedAt = d.CreatedAt;
				if (fields.HasField(nameof(Model.AiModelDefinition.UpdatedAt))) m.UpdatedAt = d.UpdatedAt;
				if (fields.HasField(nameof(Model.AiModelDefinition.ModelReference))) m.ModelReference = d.ModelReference;
				models.Add(m);
			}
			this._logger.Debug("build {count} items", models?.Count);
			return models;
		}
	}
}

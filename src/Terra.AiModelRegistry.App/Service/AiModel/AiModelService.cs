using Cite.Tools.Data.Builder;
using Cite.Tools.Data.Deleter;
using Cite.Tools.Data.Query;
using Cite.Tools.FieldSet;
using Cite.Tools.Json;
using Cite.Tools.Logging;
using Cite.Tools.Logging.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using System.Text;
using Terra.AiModelRegistry.App.Authorization;
using Terra.AiModelRegistry.App.Common;
using Terra.AiModelRegistry.App.Data;
using Terra.AiModelRegistry.App.ErrorCode;
using Terra.AiModelRegistry.App.Event;
using Terra.AiModelRegistry.App.Exception;
using Terra.AiModelRegistry.App.Formatting;
using Terra.AiModelRegistry.App.Model.Builder;
using Terra.AiModelRegistry.App.Service.Convention;
using Terra.AiModelRegistry.App.Service.S3ObjectStorage;

namespace Terra.AiModelRegistry.App.Service.AiModel
{
	public class AiModelService : IAiModelService
	{
		private readonly AppDbContext _dbContext;
		private readonly BuilderFactory _builderFactory;
		private readonly DeleterFactory _deleterFactory;
		private readonly JsonHandlingService _jsonHandlingService;
		private readonly IStringLocalizer<Resources.MySharedResources> _localizer;
		private readonly IAuthorizationService _authorizationService;
		private readonly ILogger<AiModelService> _logger;
		private readonly ErrorThesaurus _errors;
		private readonly QueryFactory _queryFactory;
		private readonly IFormattingService _formattingService;
		private readonly IAuthorizationContentResolver _authorizationContentResolver;
		private readonly EventBroker _eventBroker;
		private readonly IS3ObjectStorage _s3ObjectStorage;
		private readonly IConventionService _conventionService;

		public AiModelService(
			AppDbContext dbContext,
			BuilderFactory builderFactory,
			DeleterFactory deleterFactory,
			JsonHandlingService jsonHandlingService,
			IStringLocalizer<Resources.MySharedResources> localizer,
			IAuthorizationService authorizationService,
			ILogger<AiModelService> logger,
			ErrorThesaurus errors,
			QueryFactory queryFactory,
			IFormattingService formattingService,
			IAuthorizationContentResolver authorizationContentResolver,
			EventBroker eventBroker,
			IS3ObjectStorage s3ObjectStorage)
		{
			this._dbContext = dbContext;
			this._builderFactory = builderFactory;
			this._deleterFactory = deleterFactory;
			this._jsonHandlingService = jsonHandlingService;
			this._localizer = localizer;
			this._authorizationService = authorizationService;
			this._logger = logger;
			this._errors = errors;
			this._queryFactory = queryFactory;
			this._formattingService = formattingService;
			this._authorizationContentResolver = authorizationContentResolver;
			this._eventBroker = eventBroker;
			this._s3ObjectStorage = s3ObjectStorage;
		}

		public async Task<Model.AiModelDefinition> CreateAsync(Model.AiModelDefinitionCreate model, IFieldSet fields = null)
		{
			_logger.Debug(new MapLogEntry("creating AiModelDefinition").And("model", model).And("fields", fields));

			await this._authorizationService.AuthorizeForce(Permission.CreateAiModelDefinition);

			Data.AiModelDefinition data = new AiModelDefinition
			{
				Name = model.Name.Trim(),
				Description = model.Description.Trim(),
				Version = model.Version.Trim(),
				Metadata = model.Metadata != null ? _jsonHandlingService.ToJsonSafe(model.Metadata) : null,
				ModelReferenceKind = model.ModelReferenceKind.Value,
				ModelReference = model.ModelReference?.Trim(),
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
			};

			this._dbContext.AiModelDefinitions.Add(data);
			await _dbContext.SaveChangesAsync();

			if (data.ModelReferenceKind == ModelReferenceKind.S3)
			{
				using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(model.ModelReference));
				await this._s3ObjectStorage.UploadAsync(stream, data.Name, MediaTypeNames.Application.Octet);
			}

			this._logger.Trace("emiting event {0} for {1} items", typeof(OnAiModelDefinitionTouchedArgs), 1);
			this._eventBroker.EmitAiModelDefinitionTouched(data.Id);

			Model.AiModelDefinition created = await _builderFactory.Builder<AiModelDefinitionBuilder>()
				.Authorize(AuthorizationFlags.Any)
				.Build(FieldSet.Build(fields, nameof(Model.AiModelDefinition.Id), nameof(Model.AiModelDefinition.Hash)), data);
			return created;
		}

		public async Task<Model.AiModelDefinition> PatchAsync(Model.AiModelDefinitionPatch model, IFieldSet fields = null)
		{
			_logger.Debug(new MapLogEntry("patching AiModelDefinition").And("model", model).And("fields", fields));

			await this._authorizationService.AuthorizeForce(Permission.EditAiModelDefinition);

			Data.AiModelDefinition data = await _dbContext.AiModelDefinitions.FindAsync(model.Id.Value);

			if (data == null) throw new TerraNotFoundException(_localizer["general_notFound", model.Id.Value, nameof(Model.AiModelDefinition)]);
			if (!string.Equals(model.Hash, _conventionService.HashValue(data.UpdatedAt))) throw new TerraValidationException(_errors.HashConflict.Code, string.Format(this._errors.HashConflict.Message, data.Id, nameof(Data.AiModelDefinition)));

			data.Name = model.Name?.Trim();
			data.Description = model.Description?.Trim();
			data.Version = model.Version?.Trim();
			data.Metadata = model.Metadata != null ? _jsonHandlingService.ToJsonSafe(model.Metadata) : null;
			data.UpdatedAt = DateTime.UtcNow;

			await _dbContext.SaveChangesAsync();

			this._logger.Trace("emiting event {0} for {1} items", typeof(OnAiModelDefinitionTouchedArgs), 1);
			this._eventBroker.EmitAiModelDefinitionTouched(data.Id);

			Model.AiModelDefinition patched = await _builderFactory.Builder<AiModelDefinitionBuilder>()
				.Authorize(AuthorizationFlags.Any)
				.Build(FieldSet.Build(fields, nameof(Model.AiModelDefinition.Id), nameof(Model.AiModelDefinition.Hash)), data);
			return patched;
		}


	}
}

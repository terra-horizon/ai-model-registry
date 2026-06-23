using Cite.Tools.Data.Builder;
using Cite.Tools.Data.Censor;
using Cite.Tools.Data.Query;
using Cite.Tools.FieldSet;
using Cite.Tools.Logging;
using Cite.Tools.Logging.Extensions;
using Cite.WebTools.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;
using Terra.AiModelRegistry.Api.Model;
using Terra.AiModelRegistry.Api.OpenApi;
using Terra.AiModelRegistry.Api.Validation;
using Terra.AiModelRegistry.App.Accounting;
using Terra.AiModelRegistry.App.Censor;
using Terra.AiModelRegistry.App.Common;
using Terra.AiModelRegistry.App.ErrorCode;
using Terra.AiModelRegistry.App.Exception;
using Terra.AiModelRegistry.App.Service.AiModel;

namespace Terra.AiModelRegistry.Api.Controllers
{
	[Route("api/ai-model-definition")]
	public class AiModelDefinitionController : ControllerBase
	{
		private readonly CensorFactory _censorFactory;
		private readonly QueryFactory _queryFactory;
		private readonly BuilderFactory _builderFactory;
		private readonly ILogger<AiModelDefinitionController> _logger;
		private readonly IAccountingService _accountingService;
		private readonly IAiModelService _aiModelService;
		private readonly ErrorThesaurus _errors;
		private readonly IStringLocalizer<Resources.MySharedResources> _localizer;

		public AiModelDefinitionController(
			CensorFactory censorFactory,
			QueryFactory queryFactory,
			BuilderFactory builderFactory,
			ILogger<AiModelDefinitionController> logger,
			IAccountingService accountingService,
			IAiModelService aiModelService,
			IStringLocalizer<Resources.MySharedResources> localizer,
			ErrorThesaurus errors)
		{
			this._censorFactory = censorFactory;
			this._queryFactory = queryFactory;
			this._builderFactory = builderFactory;
			this._logger = logger;
			this._accountingService = accountingService;
			this._aiModelService = aiModelService;
			this._localizer = localizer;
			this._errors = errors;
		}

		[HttpGet("{id}")]
		[Authorize]
		[ModelStateValidationFilter]
		[SwaggerOperation(Summary = "Lookup ai model definition by id")]
		[SwaggerResponse(statusCode: 200, description: "The matching ai model definition", type: typeof(QueryResult<App.Model.AiModelDefinition>))]
		[SwaggerResponse(statusCode: 400, description: "Validation problem with the request")]
		[SwaggerResponse(statusCode: 401, description: "The request is not authenticated")]
		[SwaggerResponse(statusCode: 404, description: "Could not locate item with the provided id")]
		[SwaggerResponse(statusCode: 403, description: "The requested operation is not permitted based on granted permissions")]
		[SwaggerResponse(statusCode: 500, description: "Internal error")]
		[SwaggerResponse(statusCode: 503, description: "An underpinning service indicated failure")]
		[Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
		public async Task<App.Model.AiModelDefinition> Get(
			[FromRoute]
			[SwaggerParameter(description: "The id of the item to lookup", Required = true)]
			ObjectId id,
			[ModelBinder(Name = "f")]
			[SwaggerParameter(description: "The fields to include in the response model", Required = true)]
			[LookupFieldSetQueryStringOpenApi]
			IFieldSet fieldSet)
		{
			this._logger.Debug(new MapLogEntry("get").And("type", nameof(App.Model.AiModelDefinition)).And("id", id).And("fields", fieldSet));

			IFieldSet censoredFields = await this._censorFactory.Censor<AiModelDefinitionCensor>().Censor(fieldSet, CensorContext.AsCensor());
			if (fieldSet.CensoredAsUnauthorized(censoredFields)) throw new TerraForbiddenException(this._errors.Forbidden.Code, this._errors.Forbidden.Message);

			var model = await this._aiModelService.GetAsync(id, censoredFields);

			this._accountingService.AccountFor(KnownActions.Query, KnownResources.AiModel.AsAccountable());

			return model;
		}

		[HttpPost("create")]
		[Authorize]
		[ModelStateValidationFilter]
		[ValidationFilter(typeof(App.Model.AiModelDefinitionCreate.CreateValidator), "model")]
		[SwaggerOperation(Summary = "Create ai model definition")]
		[SwaggerResponse(statusCode: 200, description: "The persisted ai model definition", type: typeof(App.Model.AiModelDefinition))]
		[SwaggerResponse(statusCode: 400, description: "Validation problem with the request")]
		[SwaggerResponse(statusCode: 401, description: "The request is not authenticated")]
		[SwaggerResponse(statusCode: 404, description: "Could not locate item with the provided id")]
		[SwaggerResponse(statusCode: 403, description: "The requested operation is not permitted based on granted permissions")]
		[SwaggerResponse(statusCode: 500, description: "Internal error")]
		[SwaggerResponse(statusCode: 503, description: "An underpinning service indicated failure")]
		[Consumes(System.Net.Mime.MediaTypeNames.Application.Json)]
		[Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
		public async Task<App.Model.AiModelDefinition> Persist(
			[FromBody]
			[SwaggerRequestBody(description: "The model to persist", Required = true)]
			App.Model.AiModelDefinitionCreate model,
			[ModelBinder(Name = "f")]
			[SwaggerParameter(description: "The fields to include in the response model", Required = true)]
			[LookupFieldSetQueryStringOpenApi]
			IFieldSet fieldSet)
		{
			this._logger.Debug(new MapLogEntry("persisting").And("type", nameof(App.Model.AiModelDefinitionCreate)).And("fields", fieldSet));

			IFieldSet censoredFields = await this._censorFactory.Censor<AiModelDefinitionCensor>().Censor(fieldSet, CensorContext.AsCensor());
			if (fieldSet.CensoredAsUnauthorized(censoredFields)) throw new TerraForbiddenException(this._errors.Forbidden.Code, this._errors.Forbidden.Message);

			App.Model.AiModelDefinition persisted = await this._aiModelService.CreateAsync(model, censoredFields);

			this._accountingService.AccountFor(KnownActions.Create, KnownResources.AiModel.AsAccountable());

			return persisted;
		}


		[HttpPost("patch")]
		[Authorize]
		[ModelStateValidationFilter]
		[ValidationFilter(typeof(App.Model.AiModelDefinitionPatch.PatchValidator), "model")]
		[SwaggerOperation(Summary = "Patch ai model definition")]
		[SwaggerResponse(statusCode: 200, description: "The patched ai model definition", type: typeof(App.Model.AiModelDefinition))]
		[SwaggerResponse(statusCode: 400, description: "Validation problem with the request")]
		[SwaggerResponse(statusCode: 401, description: "The request is not authenticated")]
		[SwaggerResponse(statusCode: 404, description: "Could not locate item with the provided id")]
		[SwaggerResponse(statusCode: 403, description: "The requested operation is not permitted based on granted permissions")]
		[SwaggerResponse(statusCode: 500, description: "Internal error")]
		[SwaggerResponse(statusCode: 503, description: "An underpinning service indicated failure")]
		[Consumes(System.Net.Mime.MediaTypeNames.Application.Json)]
		[Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
		public async Task<App.Model.AiModelDefinition> Patch(
			[FromBody]
			[SwaggerRequestBody(description: "The model to patch", Required = true)]
			App.Model.AiModelDefinitionPatch model,
			[ModelBinder(Name = "f")]
			[SwaggerParameter(description: "The fields to include in the response model", Required = true)]
			[LookupFieldSetQueryStringOpenApi]
			IFieldSet fieldSet)
		{
			this._logger.Debug(new MapLogEntry("patching").And("type", nameof(App.Model.AiModelDefinitionPatch)).And("fields", fieldSet));

			//GOTCHA: Ommiting browse permission check in case of new
			IFieldSet censoredFields = await this._censorFactory.Censor<AiModelDefinitionCensor>().Censor(fieldSet, CensorContext.AsCensor());
			if (fieldSet.CensoredAsUnauthorized(censoredFields)) throw new TerraForbiddenException(this._errors.Forbidden.Code, this._errors.Forbidden.Message);

			App.Model.AiModelDefinition persisted = await this._aiModelService.PatchAsync(model, censoredFields);

			this._accountingService.AccountFor(KnownActions.Patch, KnownResources.AiModel.AsAccountable());

			return persisted;
		}

		[HttpDelete("{id}")]
		[Authorize]
		[ModelStateValidationFilter]
		[SwaggerOperation(Summary = "Deletes the ai model definition by id")]
		[SwaggerResponse(statusCode: 200, description: "Ai model definition deleted")]
		[SwaggerResponse(statusCode: 400, description: "Validation problem with the request")]
		[SwaggerResponse(statusCode: 401, description: "The request is not authenticated")]
		[SwaggerResponse(statusCode: 404, description: "Could not locate item with the provided id")]
		[SwaggerResponse(statusCode: 403, description: "The requested operation is not permitted based on granted permissions")]
		[SwaggerResponse(statusCode: 500, description: "Internal error")]
		[SwaggerResponse(statusCode: 503, description: "An underpinning service indicated failure")]
		public async Task Delete(
			[FromRoute]
			[SwaggerParameter(description: "The id of the item to delete", Required = true)]
			ObjectId id)
		{
			this._logger.Debug(new MapLogEntry("delete").And("type", nameof(App.Model.AiModelDefinition)).And("id", id));

			await this._aiModelService.DeleteAndSaveAsync(id);

			this._accountingService.AccountFor(KnownActions.Delete, KnownResources.AiModel.AsAccountable());
		}
	}
}

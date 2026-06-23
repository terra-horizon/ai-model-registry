using Cite.Tools.Data.Query;
using Cite.Tools.Validation;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using Terra.AiModelRegistry.App.Common;
using Terra.AiModelRegistry.App.Common.Validation;
using Terra.AiModelRegistry.App.ErrorCode;
using Terra.AiModelRegistry.App.Query;

namespace Terra.AiModelRegistry.Api.Model.Lookup
{
	public class AiModelDefinitionLookup : Cite.Tools.Data.Query.Lookup
	{

		[SwaggerSchema(description: "Limit lookup to items with specific ids. If set, the list of ids must not be empty")]
		public List<Guid> Ids { get; set; }
		[SwaggerSchema(description: "Exclude from the lookup items with specific ids. If set, the list of ids must not be empty")]
		public List<Guid> ExcludedIds { get; set; }
		[SwaggerSchema(description: "Limit lookup to items with specific isActive values. If set, the list of isActive values must not be empty")]
		public List<IsActive> IsActive { get; set; }
		[SwaggerSchema(description: "Limit lookup to items whose name matches the pattern")]
		public string Like { get; set; }

		public AiModelDefinitionQuery Enrich(QueryFactory factory)
		{
			AiModelDefinitionQuery query = factory.Query<AiModelDefinitionQuery>();

			if (this.Ids != null) query.Ids(this.Ids);
			if (!String.IsNullOrEmpty(this.Like)) query.Like(this.Like);
			if (this.ExcludedIds != null) query.ExcludedIds(this.ExcludedIds);
			if (this.IsActive != null) query.IsActive(this.IsActive);

			this.EnrichCommon(query);
			return query;
		}

		public class QueryValidator : BaseValidator<AiModelDefinitionLookup>
		{
			public QueryValidator(
				IStringLocalizer<Resources.MySharedResources> localizer,
				ValidatorFactory validatorFactory,
				ILogger<QueryValidator> logger,
				ErrorThesaurus errors) : base(validatorFactory, logger, errors)
			{
				this._localizer = localizer;
			}

			private readonly IStringLocalizer<Resources.MySharedResources> _localizer;

			protected override IEnumerable<ISpecification> Specifications(AiModelDefinitionLookup item)
			{
				return [
					//ids must be null or not empty
					this.Spec()
						.Must(() => !item.Ids.IsNotNullButEmpty())
						.FailOn(nameof(AiModelDefinitionLookup.Ids)).FailWith(this._localizer["validation_setButEmpty", nameof(AiModelDefinitionLookup.Ids)]),
					//excludedIds must be null or not empty
					this.Spec()
						.Must(() => !item.ExcludedIds.IsNotNullButEmpty())
						.FailOn(nameof(AiModelDefinitionLookup.ExcludedIds)).FailWith(this._localizer["validation_setButEmpty", nameof(AiModelDefinitionLookup.ExcludedIds)]),
					//isActive must be null or not empty
					this.Spec()
						.Must(() => !item.IsActive.IsNotNullButEmpty())
						.FailOn(nameof(AiModelDefinitionLookup.IsActive)).FailWith(this._localizer["validation_setButEmpty", nameof(AiModelDefinitionLookup.IsActive)]),
					//paging without ordering not supported
					this.Spec()
						.If(()=> item.Page != null && !item.Page.IsEmpty)
						.Must(() => item.Order != null && !item.Order.IsEmpty)
						.FailOn(nameof(AiModelDefinitionLookup.Page)).FailWith(this._localizer["validation_pagingWithoutOrdering"]),
				];
			}
		}
	}
}

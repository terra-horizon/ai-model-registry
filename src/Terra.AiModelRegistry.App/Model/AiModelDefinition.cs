using Cite.Tools.Common.Extensions;
using Cite.Tools.Validation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Terra.AiModelRegistry.App.Common;
using Terra.AiModelRegistry.App.Common.Validation;
using Terra.AiModelRegistry.App.ErrorCode;

namespace Terra.AiModelRegistry.App.Model
{
	public class AiModelDefinition
	{
		public Guid? Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Version { get; set; }
		public Dictionary<string, object> Metadata { get; set; }
		public ModelReferenceKind? ModelReferenceKind { get; set; }
		public string ModelReference { get; set; }
		public IsActive? IsActive { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public string Hash { get; set; }
	}

	public class AiModelDefinitionCreate
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public string Version { get; set; }

		public Dictionary<string, object> Metadata { get; set; }

		public ModelReferenceKind? ModelReferenceKind { get; set; }

		public string ModelReference { get; set; }


		public class CreateValidator : BaseValidator<AiModelDefinitionCreate>
		{
			private static int NameMaxLength = typeof(Data.AiModelDefinition).MaxLengthOf(nameof(Data.AiModelDefinition.Name));

			public CreateValidator(
				IStringLocalizer<Terra.AiModelRegistry.Resources.MySharedResources> localizer,
				ValidatorFactory validatorFactory,
				ILogger<CreateValidator> logger,
				ErrorThesaurus errors) : base(validatorFactory, logger, errors)
			{
				this._localizer = localizer;
			}

			private readonly IStringLocalizer<Terra.AiModelRegistry.Resources.MySharedResources> _localizer;

			protected override IEnumerable<ISpecification> Specifications(AiModelDefinitionCreate item)
			{
				return [
					
					//name must always be set
					this.Spec()
						.Must(() => !this.IsEmpty(item.Name))
						.FailOn(nameof(AiModelDefinitionCreate.Name)).FailWith(this._localizer["validation_required", nameof(AiModelDefinitionCreate.Name)]),
					//name max length
					this.Spec()
						.If(() => !this.IsEmpty(item.Name))
						.Must(() => this.LessEqual(item.Name, CreateValidator.NameMaxLength))
						.FailOn(nameof(AiModelDefinitionCreate.Name)).FailWith(this._localizer["validation_maxLength", nameof(AiModelDefinitionCreate.Name)]),
					//version must always be set
					this.Spec()
						.Must(() => !this.IsEmpty(item.Version))
						.FailOn(nameof(AiModelDefinitionCreate.Version)).FailWith(this._localizer["validation_required", nameof(AiModelDefinitionCreate.Version)]),
					//ModelReferenceKind must always be set
					this.Spec()
						.Must(() => item.ModelReferenceKind != null)
						.FailOn(nameof(AiModelDefinitionCreate.ModelReferenceKind)).FailWith(this._localizer["validation_required", nameof(AiModelDefinitionCreate.ModelReferenceKind)]),
				];
			}
		}
	}

	public class AiModelDefinitionPatch
	{
		public Guid? Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Version { get; set; }
		public Dictionary<string, object> Metadata { get; set; }
		public string Hash { get; set; }


		public class PatchValidator : BaseValidator<AiModelDefinitionPatch>
		{
			private static int NameMaxLength = typeof(Data.AiModelDefinition).MaxLengthOf(nameof(Data.AiModelDefinition.Name));

			public PatchValidator(
				IStringLocalizer<Terra.AiModelRegistry.Resources.MySharedResources> localizer,
				ValidatorFactory validatorFactory,
				ILogger<PatchValidator> logger,
				ErrorThesaurus errors) : base(validatorFactory, logger, errors)
			{
				this._localizer = localizer;
			}

			private readonly IStringLocalizer<Terra.AiModelRegistry.Resources.MySharedResources> _localizer;

			protected override IEnumerable<ISpecification> Specifications(AiModelDefinitionPatch item)
			{
				return [
					//id must always be set
					this.Spec()
						.Must(() => this.IsValidGuid(item.Id))
						.FailOn(nameof(AiModelDefinitionPatch.Id)).FailWith(this._localizer["validation_required", nameof(AiModelDefinitionPatch.Id)]),
					//name must always be set
					this.Spec()
						.Must(() => !this.IsEmpty(item.Name))
						.FailOn(nameof(AiModelDefinitionPatch.Name)).FailWith(this._localizer["validation_required", nameof(AiModelDefinitionPatch.Name)]),
					//name max length
					this.Spec()
						.If(() => !this.IsEmpty(item.Name))
						.Must(() => this.LessEqual(item.Name, PatchValidator.NameMaxLength))
						.FailOn(nameof(AiModelDefinitionPatch.Name)).FailWith(this._localizer["validation_maxLength", nameof(AiModelDefinitionPatch.Name)]),
					//version must always be set
					this.Spec()
						.Must(() => !this.IsEmpty(item.Version))
						.FailOn(nameof(AiModelDefinitionPatch.Version)).FailWith(this._localizer["validation_required", nameof(AiModelDefinitionPatch.Version)]),
					//update existing item. Hash must be set
					this.Spec()
						.Must(() => this.IsValidHash(item.Hash))
						.FailOn(nameof(AiModelDefinitionPatch.Hash)).FailWith(this._localizer["validation_required", nameof(AiModelDefinitionPatch.Hash)]),
				];
			}
		}
	}
}

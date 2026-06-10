using Cite.Tools.Validation;
using Terra.AiModelRegistry.App.ErrorCode;
using Terra.AiModelRegistry.App.Service.Storage;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Terra.AiModelRegistry.App.Common.Types;

namespace Terra.AiModelRegistry.App.Common.Validation
{
	public class DataLocationValidator : BaseValidator<DataLocation>
	{
		public DataLocationValidator(
			IStringLocalizer<Terra.AiModelRegistry.Resources.MySharedResources> localizer,
			ValidatorFactory validatorFactory,
			ILogger<DataLocationValidator> logger,
			ErrorThesaurus errors,
			IStorageService storageService) : base(validatorFactory, logger, errors)
		{
			this._localizer = localizer;
			this._storageService = storageService;
		}
		private readonly IStringLocalizer<Terra.AiModelRegistry.Resources.MySharedResources> _localizer;
		private readonly IStorageService _storageService;

		protected override IEnumerable<ISpecification> Specifications(DataLocation item)
		{
			return [
				//Location must always be set
				this.Spec()
					.Must(() => !this.IsEmpty(item.Location))
					.FailOn(nameof(DataLocation.Location)).FailWith(this._localizer["validation_required", nameof(DataLocation.Location)]),
				this.Spec()
					.If(() => item.Kind == Enum.ModelReferenceKind.S3)
					.Must(() => item.Location.IsValidPath())
					.FailOn(nameof(DataLocation.Location)).FailWith(this._localizer["validation_invalidValue", nameof(DataLocation.Location)]),
				this.Spec()
					.If(() => item.Kind == Enum.ModelReferenceKind.Http)
					.Must(() => item.Location.IsValidHttp())
					.FailOn(nameof(DataLocation.Location)).FailWith(this._localizer["validation_invalidValue", nameof(DataLocation.Location)]),
			];
		}
	}
}

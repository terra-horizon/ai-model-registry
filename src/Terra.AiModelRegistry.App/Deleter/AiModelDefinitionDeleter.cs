using Cite.Tools.Data.Deleter;
using Cite.Tools.Data.Query;
using Cite.Tools.Logging;
using Cite.Tools.Logging.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Terra.AiModelRegistry.App.Authorization;
using Terra.AiModelRegistry.App.Common;
using Terra.AiModelRegistry.App.Data;
using Terra.AiModelRegistry.App.ErrorCode;
using Terra.AiModelRegistry.App.Event;

namespace Terra.AiModelRegistry.App
{
	public class AiModelDefinitionDeleter : IDeleter
	{
		private readonly QueryFactory _queryFactory;
		private readonly AppDbContext _dbContext;
		private readonly ILogger<AiModelDefinitionDeleter> _logger;
		private readonly ErrorThesaurus _errors;
		private readonly IStringLocalizer<Resources.MySharedResources> _localizer;
		private readonly EventBroker _eventBroker;
		private readonly DeleterFactory _deleterFactory;

		public AiModelDefinitionDeleter(
			AppDbContext dbContext,
			QueryFactory queryFactory,
			ILogger<AiModelDefinitionDeleter> logger,
			ErrorThesaurus errors,
			IStringLocalizer<Resources.MySharedResources> localizer,
			EventBroker eventBroker,
			DeleterFactory deleterFactory)
		{
			this._logger = logger;
			this._dbContext = dbContext;
			this._queryFactory = queryFactory;
			this._errors = errors;
			this._localizer = localizer;
			this._eventBroker = eventBroker;
			this._deleterFactory = deleterFactory;
		}

		public async Task DeleteAndSave(IEnumerable<Guid> ids)
		{
			this._logger.Debug(new MapLogEntry("collecting to delete").And("count", ids?.Count()).And("ids", ids));
			List<Data.AiModelDefinition> datas = await this._queryFactory.Query<AiModelDefinitionQuery>().Ids(ids).Authorize(Authorization.AuthorizationFlags.None).CollectAsync();
			this._logger.Trace("retrieved {0} items", datas?.Count);
			await this.DeleteAndSave(datas);
		}

		public async Task DeleteAndSave(IEnumerable<Data.AiModelDefinition> datas)
		{
			this._logger.Debug("will delete {0} items", datas?.Count());
			await this.Delete(datas);
			this._logger.Trace("saving changes");
			await this._dbContext.SaveChangesAsync();
			this._logger.Trace("changes saved");
		}

		public async Task Delete(IEnumerable<Data.AiModelDefinition> datas)
		{
			this._logger.Debug("will delete {0} items", datas?.Count());
			if (datas == null || !datas.Any()) return;

			List<Guid> ids = datas.Select(x => x.Id).Distinct().ToList();

			foreach (Data.AiModelDefinition item in datas)
			{
				this._logger.Trace("deleting item {id}", item.Id);
				item.IsActive = IsActive.Inactive;
				item.UpdatedAt = DateTime.UtcNow;
				this._logger.Trace("updating item");
				this._dbContext.Update(item);
				this._logger.Trace("updated item");
			}
			if (ids.Count > 0)
			{
				this._logger.Trace("emiting event {0} for {1} items", typeof(OnAiModelDefinitionDeletedArgs), datas.Count());
				this._eventBroker.EmitAiModelDefinitionDeleted(datas.Select(x => x.Id));
			}
		}
	}
}

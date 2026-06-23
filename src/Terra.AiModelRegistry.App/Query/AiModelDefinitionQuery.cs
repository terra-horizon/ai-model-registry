using Cite.Tools.Common.Extensions;
using Cite.Tools.Data.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terra.AiModelRegistry.App.Authorization;
using Terra.AiModelRegistry.App.Common;
using Terra.AiModelRegistry.App.Data;

namespace Terra.AiModelRegistry.App.Query
{
	public class AiModelDefinitionQuery : Query<Data.AiModelDefinition>
	{
		private List<Guid> _ids { get; set; }
		private List<Guid> _excludedIds { get; set; }
		private String _like { get; set; }
		private List<IsActive> _isActive { get; set; }
		private AuthorizationFlags _authorize { get; set; } = AuthorizationFlags.None;

		public AiModelDefinitionQuery(AppDbContext dbContext,
			IAuthorizationContentResolver authorizationContentResolver)
		{
			this._dbContext = dbContext;
			this._authorizationContentResolver = authorizationContentResolver;
		}

		private readonly AppDbContext _dbContext;
		private readonly IAuthorizationContentResolver _authorizationContentResolver;

		public AiModelDefinitionQuery Ids(IEnumerable<Guid> ids) { this._ids = this.ToList(ids); return this; }
		public AiModelDefinitionQuery Ids(Guid id) { this._ids = this.ToList(id.AsArray()); return this; }
		public AiModelDefinitionQuery ExcludedIds(IEnumerable<Guid> excludedIds) { this._excludedIds = this.ToList(excludedIds); return this; }
		public AiModelDefinitionQuery ExcludedIds(Guid excludedId) { this._excludedIds = this.ToList(excludedId.AsArray()); return this; }
		public AiModelDefinitionQuery Like(String like) { this._like = like; return this; }
		public AiModelDefinitionQuery IsActive(IEnumerable<IsActive> isActive) { this._isActive = this.ToList(isActive); return this; }
		public AiModelDefinitionQuery IsActive(IsActive isActive) { this._isActive = this.ToList(isActive.AsArray()); return this; }
		public AiModelDefinitionQuery EnableTracking() { base.NoTracking = false; return this; }
		public AiModelDefinitionQuery DisableTracking() { base.NoTracking = true; return this; }
		public AiModelDefinitionQuery AsDistinct() { base.Distinct = true; return this; }
		public AiModelDefinitionQuery AsNotDistinct() { base.Distinct = false; return this; }
		public AiModelDefinitionQuery Authorize(AuthorizationFlags flags) { this._authorize = flags; return this; }

		protected override bool IsFalseQuery()
		{
			return this.IsEmpty(this._ids) || this.IsEmpty(this._excludedIds) || this.IsEmpty(this._isActive);
		}

		public async Task<AiModelDefinition> Find(Guid id, Boolean tracked = true)
		{
			if (tracked) return await this._dbContext.AiModelDefinitions.FindAsync(id);
			else return await this._dbContext.AiModelDefinitions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
		}

		protected override IQueryable<AiModelDefinition> Queryable()
		{
			IQueryable<AiModelDefinition> query = this._dbContext.AiModelDefinitions.AsQueryable();
			return query;
		}

		protected override async Task<IQueryable<AiModelDefinition>> ApplyAuthzAsync(IQueryable<AiModelDefinition> query)
		{
			if (this._authorize.HasFlag(AuthorizationFlags.None)) return query;
			if (this._authorize.HasFlag(AuthorizationFlags.Permission))
			{
				if (await this._authorizationContentResolver.HasPermission(Permission.BrowseAiModelDefinition)) return query;
			}
			//AuthorizationFlags.Context and Owner not applicable
			return query.Where(x => false);
		}

		protected override async Task<IQueryable<AiModelDefinition>> ApplyFiltersAsync(IQueryable<AiModelDefinition> query)
		{
			if (this._ids != null) query = query.Where(x => this._ids.Contains(x.Id));
			if (this._isActive != null) query = query.Where(x => this._isActive.Contains(x.IsActive));
			if (this._excludedIds != null) query = query.Where(x => !this._excludedIds.Contains(x.Id));
			if (!String.IsNullOrEmpty(this._like)) query = query.Where(x => EF.Functions.ILike(x.Name, this._like) || EF.Functions.ILike(x.Description, this._like));
			return query;
		}

		protected override IOrderedQueryable<AiModelDefinition> OrderClause(IQueryable<AiModelDefinition> query, OrderingFieldResolver item)
		{
			IOrderedQueryable<AiModelDefinition> orderedQuery = null;
			if (this.IsOrdered(query)) orderedQuery = query as IOrderedQueryable<AiModelDefinition>;

			if (item.Match(nameof(Model.AiModelDefinition.Id))) orderedQuery = this.OrderOn(query, orderedQuery, item, x => x.Id);
			else if (item.Match(nameof(Model.AiModelDefinition.Name))) orderedQuery = this.OrderOn(query, orderedQuery, item, x => x.Name);
			else if (item.Match(nameof(Model.AiModelDefinition.Description))) orderedQuery = this.OrderOn(query, orderedQuery, item, x => x.Description);
			else if (item.Match(nameof(Model.AiModelDefinition.ModelReferenceKind))) orderedQuery = this.OrderOn(query, orderedQuery, item, x => x.ModelReferenceKind);
			else if (item.Match(nameof(Model.AiModelDefinition.IsActive))) orderedQuery = this.OrderOn(query, orderedQuery, item, x => x.IsActive);
			else if (item.Match(nameof(Model.AiModelDefinition.CreatedAt))) orderedQuery = this.OrderOn(query, orderedQuery, item, x => x.CreatedAt);
			else if (item.Match(nameof(Model.AiModelDefinition.UpdatedAt))) orderedQuery = this.OrderOn(query, orderedQuery, item, x => x.UpdatedAt);
			else return null;

			return orderedQuery;
		}

		protected override List<String> FieldNamesOf(IEnumerable<FieldResolver> items)
		{
			HashSet<String> projectionFields = new HashSet<String>();
			foreach (FieldResolver item in items)
			{
				if (item.Match(nameof(Model.Conversation.Id))) projectionFields.Add(nameof(Conversation.Id));
				else if (item.Match(nameof(Model.Conversation.Name))) projectionFields.Add(nameof(Conversation.Name));
				else if (item.Prefix(nameof(Model.Conversation.User))) projectionFields.Add(nameof(Conversation.UserId));
				else if (item.Prefix(nameof(Model.Conversation.Datasets))) projectionFields.Add(nameof(Conversation.Id));
				else if (item.Prefix(nameof(Model.Conversation.Messages))) projectionFields.Add(nameof(Conversation.Id));
				else if (item.Match(nameof(Model.Conversation.IsActive))) projectionFields.Add(nameof(Conversation.IsActive));
				else if (item.Match(nameof(Model.Conversation.CreatedAt))) projectionFields.Add(nameof(Conversation.CreatedAt));
				else if (item.Match(nameof(Model.Conversation.UpdatedAt))) projectionFields.Add(nameof(Conversation.UpdatedAt));
				else if (item.Match(nameof(Model.Conversation.ETag))) projectionFields.Add(nameof(Conversation.UpdatedAt));
			}
			return projectionFields.ToList();
		}
	}
}

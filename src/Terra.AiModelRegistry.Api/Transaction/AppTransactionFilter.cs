using Cite.WebTools.Data.Transaction;
using Terra.AiModelRegistry.App.Data;

namespace Terra.AiModelRegistry.Api.Transaction
{
	public class AppTransactionFilter : TransactionFilter
	{
		public AppTransactionFilter(AppDbContext dbContext, ILogger<AppTransactionFilter> logger) : base(dbContext, logger) { }
	}
}

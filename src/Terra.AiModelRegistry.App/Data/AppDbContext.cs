using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Terra.AiModelRegistry.App.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<AiModelDefinition> AiModelDefinitions { get; set; }
		public DbSet<VersionInfo> VersionInfos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<AiModelDefinition>().ToCollection("ai_model_definition");
			modelBuilder.Entity<VersionInfo>().ToCollection("version_info");
		}
	}
}

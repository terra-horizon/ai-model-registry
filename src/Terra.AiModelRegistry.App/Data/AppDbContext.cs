using Microsoft.EntityFrameworkCore;

namespace Terra.AiModelRegistry.App.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<AiModelDefinition> AiModelDefinitions { get; set; }
		public DbSet<VersionInfo> VersionInfos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			new AiModelDefinitionEntityConfiguration().Configure(modelBuilder.Entity<AiModelDefinition>());
			new VersionInfoEntityConfiguration().Configure(modelBuilder.Entity<VersionInfo>());
		}
	}
}

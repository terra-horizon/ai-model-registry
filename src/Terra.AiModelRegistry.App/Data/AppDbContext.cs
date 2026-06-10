using Microsoft.EntityFrameworkCore;

namespace Terra.AiModelRegistry.App.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<VersionInfo> VersionInfos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			new VersionInfoEntityConfiguration().Configure(modelBuilder.Entity<VersionInfo>());
		}
	}
}

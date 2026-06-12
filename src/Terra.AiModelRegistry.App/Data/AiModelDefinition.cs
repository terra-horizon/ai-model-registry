using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using Terra.AiModelRegistry.App.Common;
using Terra.AiModelRegistry.App.Common.Data;

namespace Terra.AiModelRegistry.App.Data
{
	public class AiModelDefinition
	{
		[Key]
		[Required]
		public Guid Id { get; set; }

		[Required]
		[MaxLength(300)]
		public string Name { get; set; }

		public string Description { get; set; }

		[Required]
		public string Version { get; set; }

		public string Metadata { get; set; }

		public ModelReferenceKind ModelReferenceKind { get; set; }

		[Required]
		public string ModelReference { get; set; }
	}

	public class AiModelDefinitionEntityConfiguration : EntityTypeConfigurationBase<AiModelDefinition>
	{
		public AiModelDefinitionEntityConfiguration() : base() { }

		public override void Configure(EntityTypeBuilder<AiModelDefinition> builder)
		{
			builder.ToTable("ai_model_definition");
			builder.Property(x => x.Id).HasColumnName("id");
			builder.Property(x => x.Name).HasColumnName("name");
			builder.Property(x => x.Version).HasColumnName("version");
			builder.Property(x => x.Description).HasColumnName("description");
			builder.Property(x => x.Metadata).HasColumnName("metadata");
			builder.Property(x => x.ModelReferenceKind).HasColumnName("model_reference_kind");
			builder.Property(x => x.ModelReference).HasColumnName("model_reference");
		}
	}
}

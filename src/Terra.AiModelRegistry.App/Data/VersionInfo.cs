using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Terra.AiModelRegistry.App.Data
{
	public class VersionInfo
	{
		[BsonId]
		[Required]
		[MaxLength(30)]
		public string Key { get; set; }

		[BsonElement("version")]
		[Required]
		[MaxLength(50)]
		public string Version { get; set; }

		[BsonElement("released_at")]
		public DateTime ReleasedAt { get; set; }

		[BsonElement("deployed_at")]
		public DateTime DeployedAt { get; set; }

		[BsonElement("description")]
		public string? Description { get; set; }
	}
}

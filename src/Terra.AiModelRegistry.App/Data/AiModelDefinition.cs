using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using Terra.AiModelRegistry.App.Common;

namespace Terra.AiModelRegistry.App.Data
{
	public class AiModelDefinition
	{
		[BsonId]
		public ObjectId Id { get; set; }

		[BsonElement("name")]
		[Required]
		[MaxLength(300)]
		public string Name { get; set; }

		[BsonElement("description")]
		public string Description { get; set; }

		[BsonElement("version")]
		[Required]
		public string Version { get; set; }

		[BsonElement("metadata")]
		public string Metadata { get; set; }

		[BsonElement("model_reference")]
		[Required]
		public string ModelReference { get; set; }

		[BsonElement("is_active")]
		[BsonRepresentation(BsonType.String)]
		[Required]
		public IsActive IsActive { get; set; }

		[BsonElement("created_at")]
		[Required]
		public DateTime CreatedAt { get; set; }

		[BsonElement("updated_at")]
		[Required]
		public DateTime UpdatedAt { get; set; }
	}
}

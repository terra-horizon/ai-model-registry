using Cite.Tools.FieldSet;
using MongoDB.Bson;
using Terra.AiModelRegistry.App.Model;

namespace Terra.AiModelRegistry.App.Service.AiModel
{
    public interface IAiModelService
    {
        Task<AiModelDefinition> CreateAsync(AiModelDefinitionCreate model, IFieldSet fields = null);
        Task<AiModelDefinition> PatchAsync(AiModelDefinitionPatch model, IFieldSet fields = null);
        Task<Model.AiModelDefinition> GetAsync(ObjectId id, IFieldSet fields = null);
		Task DeleteAndSaveAsync(ObjectId id);
    }
}
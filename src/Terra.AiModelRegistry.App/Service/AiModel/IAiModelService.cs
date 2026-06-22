using Cite.Tools.FieldSet;
using Terra.AiModelRegistry.App.Model;

namespace Terra.AiModelRegistry.App.Service.AiModel
{
    public interface IAiModelService
    {
        Task<AiModelDefinition> CreateAsync(AiModelDefinitionCreate model, IFieldSet fields = null);
        Task<AiModelDefinition> PatchAsync(AiModelDefinitionPatch model, IFieldSet fields = null);
    }
}
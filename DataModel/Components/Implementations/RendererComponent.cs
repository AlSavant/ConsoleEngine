using DataModel.Rendering;
using DataModel.StaticData.Component;

namespace DataModel.Components.Implementations
{
    public sealed class RendererComponent : Component<IRendererComponentStaticData>, IRendererComponent
    {
        public Material Material { get; set; }
    }
}

using DataModel.Rendering;
using DataModel.StaticData.Component;

namespace DataModel.Components
{
    public interface IRendererComponent : IComponent<IRendererComponentStaticData>
    {
        Material Material { get; set; }   
    }
}

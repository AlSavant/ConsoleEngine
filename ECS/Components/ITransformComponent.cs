using EntityComponent.StaticData.Component;
using System.Numerics;

namespace EntityComponent.Components
{
    public interface ITransformComponent : IComponent<ITransformComponentStaticData>
    {
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        Vector2 Forward { get; }
        Vector2 Back { get; }
        Vector2 Right { get; }
        Vector2 Left { get; }        
    }
}

using DataModel.Math.Structures;
using EntityComponent.StaticData.Component;

namespace EntityComponent.Components
{
    public interface ICameraComponent : IComponent<ICameraComponentStaticData>
    {
        float FieldOfView { get; set; }
        float FarClippingDistance { get; set; }
        float NearClippingDistance { get; set; }
        Rect ViewPort { get; set; }
    }
}

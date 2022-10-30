using DataModel.Math.Structures;
using DataModel.StaticData.Component;

namespace DataModel.Components
{
    public interface ICameraComponent : IComponent<ICameraComponentStaticData>
    {
        float FieldOfView { get; set; }
        float FarClippingDistance { get; set; }
        float NearClippingDistance { get; set; }
        Rect ViewPort { get; set; }
    }
}

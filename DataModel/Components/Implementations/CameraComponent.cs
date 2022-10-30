using DataModel.Math.Structures;
using DataModel.StaticData.Component;

namespace DataModel.Components.Implementations
{
    public sealed class CameraComponent : Component<ICameraComponentStaticData>, ICameraComponent
    {
        public float FieldOfView { get; set; } = 3.14159f / 4f;
        public float FarClippingDistance { get; set; } = 100f;
        public float NearClippingDistance { get; set; } = 0.5f;
        public Rect ViewPort { get; set; } = new Rect(0f, 0f, 1f, 1f);
    }
}

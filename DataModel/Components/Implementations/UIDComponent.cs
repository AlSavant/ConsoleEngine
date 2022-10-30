using DataModel.StaticData.Component;

namespace DataModel.Components.Implementations
{
    public sealed class UIDComponent : Component<IUIDComponentStaticData>, IUIDComponent
    {
        public int ID { get; set; }
    }
}

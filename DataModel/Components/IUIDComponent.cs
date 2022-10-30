using DataModel.StaticData.Component;

namespace DataModel.Components
{
    public interface IUIDComponent : IComponent<IUIDComponentStaticData>
    {
        int ID { get; set; }
    }
}

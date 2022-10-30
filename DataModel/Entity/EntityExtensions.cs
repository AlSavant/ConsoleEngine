using DataModel.Components;

namespace DataModel.Entity
{
    public static class EntityExtensions
    {
        public static bool IsNull(this IEntity entity)
        {
            if (entity == null)
            {
                return true;
            }
            return ((Implementations.Entity)entity) == null;
        }

        public static bool IsNull(this IComponent component)
        {
            if (component == null)
            {
                return true;
            }
            return component.Entity.IsNull();
        }
    }
}

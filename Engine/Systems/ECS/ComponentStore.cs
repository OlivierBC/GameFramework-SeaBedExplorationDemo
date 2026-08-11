namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS
{
    public class ComponentStore<T> : IComponentStore
        where T : IComponent
    {
        private readonly Dictionary<Entity, T> components = new();

        public int Count => components.Count;

        public IEnumerable<Entity> Entities => components.Keys;

        public void Add(Entity entity, T component)
        {
            components[entity] = component;
        }

        public bool TryGet(Entity entity, out T? component)
        {
            return components.TryGetValue(entity, out component);
        }

        public void Remove(Entity entity)
        {
            components.Remove(entity);
        }

        bool IComponentStore.TryGet(
            Entity entity,
            out IComponent? component
        )
        {
            if (components.TryGetValue(entity, out T? result))
            {
                component = result;
                return true;
            }

            component = null;
            return false;
        }
    }
}
namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS
{
    public interface IComponentStore
    {
        int Count { get; }
        IEnumerable<Entity> Entities { get; }

        bool TryGet(Entity entity, out IComponent? component);
        void Remove(Entity entity);
    }
}
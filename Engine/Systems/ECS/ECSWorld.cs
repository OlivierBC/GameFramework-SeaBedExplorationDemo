using GameFramework_SeaBedExplorationDemo.Engine.Registries.Observables;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS
{
    public class ECSWorld : IUpdatable, IDrawable3D, IUnloadable, ILoadable
    {
        public List<Entity> Entities { get; } = new();
        public List<ISystem> Systems { get; } = new();

        private readonly Dictionary<Type, IComponentStore> componentStores = new();

        private long nextEntityId;

        public Entity CreateEntity()
        {
            Entity entity = new(nextEntityId++);
            Entities.Add(entity);

            return entity;
        }

        public void DeleteEntity(Entity entity)
        {
            Entities.Remove(entity);

            foreach (IComponentStore store in componentStores.Values)
                store.Remove(entity);
        }

        public void AddComponent<T>(Entity entity, T component) where T : IComponent
        {
            ComponentStore<T> store = GetComponentStore<T>();
            store.Add(entity, component);
        }

        public ComponentStore<T> GetComponentStore<T>() where T : IComponent
        {
            if (!componentStores.TryGetValue(
                typeof(T),
                out IComponentStore? store
            ))
            {
                ComponentStore<T> newStore = new();

                componentStores.Add(typeof(T), newStore);

                return newStore;
            }

            return (ComponentStore<T>)store;
        }

        public void AddSystem(ISystem system)
        {
            Systems.Add(system);
        }

        public void Load()
        {
            foreach (ISystem system in Systems)
                system.Load();
        }

        public void Update(float dt)
        {
            foreach (ISystem system in Systems)
                system.Update(dt);
        }

        public void Draw()
        {
            foreach (ISystem system in Systems)
                system.Draw();
        }

        public void Unload()
        {
            foreach (ISystem system in Systems)
                system.Unload();
        }
        // The Query methods here have multiple overloads depending on the amount of queried component types,
        // Using a params array of T where T : IComponent would require casting after querying, because it would return IComponent values:
        // The overloads keep each component strongly typed
        public IEnumerable<(Entity Entity, T1 Component)> Query<T1>() where T1 : IComponent
        {
            ComponentStore<T1> store = GetComponentStore<T1>();

            foreach (Entity entity in store.Entities)
            {
                if (store.TryGet(entity, out T1? component))
                    yield return (entity, component!);
            }
        }
        public IEnumerable<(Entity Entity, T1 Component1, T2 Component2)> Query<T1, T2>() where T1 : IComponent where T2 : IComponent
        {
            ComponentStore<T1> store1 = GetComponentStore<T1>();
            ComponentStore<T2> store2 = GetComponentStore<T2>();

            IEnumerable<Entity> entities =
                store1.Count <= store2.Count
                    ? store1.Entities
                    : store2.Entities;

            foreach (Entity entity in entities)
            {
                if (!store1.TryGet(entity, out T1? component1))
                    continue;

                if (!store2.TryGet(entity, out T2? component2))
                    continue;

                yield return (
                    entity,
                    component1!,
                    component2!
                );
            }
        }
        public IEnumerable<(Entity Entity, T1 Component1, T2 Component2, T3 Component3)> Query<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
        {
            ComponentStore<T1> store1 = GetComponentStore<T1>();
            ComponentStore<T2> store2 = GetComponentStore<T2>();
            ComponentStore<T3> store3 = GetComponentStore<T3>();

            IComponentStore smallestStore = store1;

            if (store2.Count < smallestStore.Count)
                smallestStore = store2;

            if (store3.Count < smallestStore.Count)
                smallestStore = store3;

            foreach (Entity entity in smallestStore.Entities)
            {
                if (!store1.TryGet(entity, out T1? component1))
                    continue;

                if (!store2.TryGet(entity, out T2? component2))
                    continue;

                if (!store3.TryGet(entity, out T3? component3))
                    continue;

                yield return (
                    entity,
                    component1!,
                    component2!,
                    component3!
                );
            }
        }
    }
}
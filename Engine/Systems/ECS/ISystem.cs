namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS
{
    public interface ISystem
    {
        public virtual void Load() { }
        public virtual void Update(float dt) { }
        public virtual void Unload() { }
        public virtual void Draw() { }
    }
}

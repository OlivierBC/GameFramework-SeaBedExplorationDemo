using GameFramework_SeaBedExplorationDemo.Engine.Observers;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene
{
    public abstract class Scene
    {
        protected Registry registry = new();

        public virtual void Load() => registry.Load();
        public virtual void Update(float dt) => registry.Update(dt);
        public virtual void Draw() => registry.Draw();
        public virtual void Draw2D() => registry.Draw2D();
        public virtual void DrawUI() => registry.DrawUI();
        public virtual void Unload() => registry.Unload();
    }
}

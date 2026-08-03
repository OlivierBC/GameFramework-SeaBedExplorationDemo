using GameFramework_SeaBedExplorationDemo.Engine.Observers;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene
{
    public class Scene
    {
        protected Registry registry = new();

        public void Update(float dt) => registry.Update(dt);
        public void Draw() => registry.Draw();
        public void Draw2D() => registry.Draw2D();
        public void DrawUI() => registry.DrawUI();
        public void Unload() => registry.Unload();
    }
}

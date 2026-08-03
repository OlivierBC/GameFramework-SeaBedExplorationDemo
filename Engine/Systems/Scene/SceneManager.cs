using GameFramework_SeaBedExplorationDemo.Engine.Base;
using GameFramework_SeaBedExplorationDemo.Engine.Observers.Observables;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene
{
    internal class SceneManager : GameObject, IUpdatable, IDrawable2D, IDrawable3D, IDrawableUI, IUnloadable
    {
        public Scene currentScene { get; private set; } = new();
        Scene? sceneToLoad;

        public SceneManager() { }
        public SceneManager(Scene startingScene) : this()
        {
            currentScene = startingScene;
        }

        public void Update(float dt)
        {
            if (sceneToLoad != null)
            {
                currentScene.Unload();
                currentScene = sceneToLoad;
                sceneToLoad = null;
            }

            currentScene.Update(dt);
        }
        public void Draw() => currentScene.Draw();
        public void Draw2D() => currentScene.Draw2D();
        public void DrawUI() => currentScene.DrawUI();
        public void Unload() => currentScene.Unload();

        public void ChangeSceneNextFrame(Scene scene)
        {
            sceneToLoad = scene;
        }
    }
}

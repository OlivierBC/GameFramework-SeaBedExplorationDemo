using GameFramework_SeaBedExplorationDemo.Engine.Base;
using GameFramework_SeaBedExplorationDemo.Engine.Registries.Observables;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene
{
    internal class SceneManager : Element, IUpdatable, IDrawable2D, IDrawable3D, IDrawableUI, IUnloadable, ILoadable
    {
        public Scene CurrentScene { get; private set; }
        private Func<Scene>? initSceneToLoad;
        public SceneManager(Scene startingScene)
        {
            CurrentScene = startingScene;
        }

        public static SceneManager MainInstance = new(Scene.EmptyScene);

        public void Update(float dt)
        {
            CurrentScene.Update(dt);
        }
        public void Draw() => CurrentScene.Draw();
        public void Draw2D() => CurrentScene.Draw2D();
        public void DrawUI() => CurrentScene.DrawUI();
        public void Load() => CurrentScene.Load();
        public void Unload() => CurrentScene.Unload();

        public void ChangeSceneNextFrame<T>() where T : Scene, new()
        {
            initSceneToLoad = static () => new T();
        }

        public void ChangeScene<T>() where T : Scene, new()
        {
            ChangeSceneNextFrame<T>();
            ProcessTransition();
        }

        public void ProcessTransition()
        {
            if (initSceneToLoad == null)
                return;
            CurrentScene.Unload();

            CurrentScene = initSceneToLoad();
            initSceneToLoad = null;

            CurrentScene.Load();
        }
    }
}

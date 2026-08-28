using GameFramework_SeaBedExplorationDemo.Engine.Systems.Geometry;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene.SceneEditor;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.UI;

namespace GameFramework_SeaBedExplorationDemo.Project.Scenes
{
    public class EditorScene : Scene
    {
        SceneEditor sceneEditor;
        UIManager uiManager = new();

        string editedSceneName = "seabed";

        public EditorScene()
        {
            registry.Add(new Grid());

            List<SceneStaticGeometry> baseStaticGeometrys = SceneFileManager.LoadSceneElements(editedSceneName);

            sceneEditor = new(baseStaticGeometrys);

            uiManager.Add(sceneEditor.UIContainer);

            registry.Add(sceneEditor);
            registry.Add(uiManager);
        }

        public override void Unload()
        {
            sceneEditor.BakeSceneFile(editedSceneName);

            base.Unload();
        }
    }
}

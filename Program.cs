using GameFramework_SeaBedExplorationDemo.Engine.Observers;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.UI;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.UI.Elements.Interactable;
using GameFramework_SeaBedExplorationDemo.Project.Movements;
using GameFramework_SeaBedExplorationDemo.Project.Scenes;
using Raylib_cs;

const int screenWidth = 1240;
const int screenHeight = 720;

Raylib.InitWindow(screenWidth, screenHeight, "Sea Bed Exploration Demo");

Registry MainRegistry = new();

Scene scene = new GeometryTestScene();
SceneManager sceneManager = new SceneManager(scene);

UIManager uiManager = new UIManager();

UIButton toggleScenes = new(new(50, 50), new(220, 120), "Change Scene", () => sceneManager.ChangeSceneNextFrame((sceneManager.currentScene is GeometryTestScene) ? new GeometryPlaygroundScene() : new GeometryTestScene()));
uiManager.Add(toggleScenes);

Camera gameCamera = new();


MainRegistry.Add(uiManager);
MainRegistry.Add(sceneManager);

MainRegistry.Add(gameCamera);

while (!Raylib.WindowShouldClose())
{
    float dt = Raylib.GetFrameTime();

    MainRegistry.Update(dt);

    Raylib.BeginDrawing();
    Raylib.ClearBackground(new Color(8, 25, 45, 255));

    Raylib.BeginMode3D(Camera.instance);

    MainRegistry.Draw();

    Raylib.EndMode3D();

    MainRegistry.Draw2D();

    MainRegistry.DrawUI();

    Raylib.DrawFPS(screenWidth - 150, 10);

    Raylib.EndDrawing();
}

MainRegistry.Unload();

Raylib.CloseWindow();
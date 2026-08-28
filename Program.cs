using GameFramework_SeaBedExplorationDemo.Engine.Registries;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Inputs;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene;
using GameFramework_SeaBedExplorationDemo.Project.Movements;
using GameFramework_SeaBedExplorationDemo.Project.Scenes;
using Raylib_cs;

const int screenWidth = 1240;
const int screenHeight = 720;

Raylib.InitWindow(screenWidth, screenHeight, "Sea Bed Exploration Demo");

Registry MainRegistry = new();

SceneManager.MainInstance.ChangeScene<EditorScene>();

InputManager.MainInstance.Add(new("1", KeyboardKey.One, InputAction.InputCallbackMode.OnPressed, SceneManager.MainInstance.ChangeSceneNextFrame<GeometryTestScene>));
InputManager.MainInstance.Add(new("2", KeyboardKey.Two, InputAction.InputCallbackMode.OnPressed, SceneManager.MainInstance.ChangeSceneNextFrame<BoidScene>));
InputManager.MainInstance.Add(new("3", KeyboardKey.Three, InputAction.InputCallbackMode.OnPressed, SceneManager.MainInstance.ChangeSceneNextFrame<EditorScene>));

Camera gameCamera = new();

MainRegistry.Add(SceneManager.MainInstance);
MainRegistry.Add(InputManager.MainInstance);

MainRegistry.Add(gameCamera);


MainRegistry.Load();
while (!Raylib.WindowShouldClose())
{
    float dt = Raylib.GetFrameTime();

    SceneManager.MainInstance.ProcessTransition();

    MainRegistry.Update(dt);

    Raylib.BeginDrawing();
    Raylib.ClearBackground(new Color(8, 25, 45, 255));

    Raylib.BeginMode3D(Camera.Instance);

    MainRegistry.Draw();

    Raylib.EndMode3D();

    MainRegistry.Draw2D();

    MainRegistry.DrawUI();

    Raylib.DrawFPS(screenWidth - 150, 10);

    Raylib.EndDrawing();
}

MainRegistry.Unload();

Raylib.CloseWindow();
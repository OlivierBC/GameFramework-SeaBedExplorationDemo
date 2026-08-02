using Raylib_cs;
using UiIntegration.Engine.Observers;
using UiIntegration.Project.Movements;

const int screenWidth = 1240;
const int screenHeight = 720;

Raylib.InitWindow(screenWidth, screenHeight, "Sea Bed Exploration Demo");

Camera gameCamera = new();

while (!Raylib.WindowShouldClose())
{
    float dt = Raylib.GetFrameTime();

    Observers.Update(dt);

    Raylib.BeginDrawing();
    Raylib.ClearBackground(new Color(8, 25, 45, 255));

    Raylib.BeginMode3D(Camera.instance);

    Observers.Draw();

    Raylib.DrawGrid(32, 1);

    Raylib.EndMode3D();

    Observers.DrawUI();

    Raylib.DrawFPS(screenWidth - 150, 10);

    Raylib.EndDrawing();
}

Observers.Unload();

Raylib.CloseWindow();
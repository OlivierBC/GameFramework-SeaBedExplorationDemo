using Raylib_cs;
using System.Numerics;

const int screenWidth = 1240;
const int screenHeight = 720;

Raylib.InitWindow(screenWidth, screenHeight, "Sea Bed Exploration Demo");

Camera3D camera = new()
{
    Position = new Vector3(8.0f, 6.0f, 8.0f),
    Target = Vector3.Zero,
    Up = Vector3.UnitY,
    FovY = 45.0f,
    Projection = CameraProjection.Perspective
};

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(new Color(8, 25, 45, 255));

    Raylib.DrawText(
        "Initial Raylib rendering test",
        20,
        20,
        36,
        Color.SkyBlue
    );

    Raylib.DrawFPS(screenWidth - 100, 20);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();
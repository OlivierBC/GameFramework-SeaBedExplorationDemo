using GameFramework_SeaBedExplorationDemo.Engine.Types;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Base
{
    internal static class Mouse
    {
        public static bool IsHitDetectionDisabled { get; set; }
        public static bool IsHittingUi => uiHitsCount > 0;

        static int uiHitsCount = 0;

        public static Vector2 Position => Raylib.GetMousePosition();
        public static Vector2 Delta => Raylib.GetMouseDelta();

        /// <summary>Detect if a mouse button has been released once</summary>
        public static bool IsBtnReleased(MouseButton button) => Raylib.IsMouseButtonReleased(button);
        /// <summary>Detect if a mouse button has been pressed once</summary>
        public static bool IsBtnPressed(MouseButton button) => Raylib.IsMouseButtonPressed(button);
        /// <summary>Detect if a mouse button is being pressed</summary>
        public static bool IsBtnDown(MouseButton button) => Raylib.IsMouseButtonDown(button);
        /// <summary>Detect if a mouse button is NOT being pressed</summary>
        public static bool IsBtnUp(MouseButton button) => Raylib.IsMouseButtonUp(button);

        public static void AddUiHit() => uiHitsCount++;
        public static void RemoveUiHit() => uiHitsCount--;
    }
}

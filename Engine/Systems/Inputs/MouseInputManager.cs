using GameFramework_SeaBedExplorationDemo.Engine.Types;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Inputs
{
    internal static class MouseInputManager
    {
        public static bool IsHitDetectionDisabled { get; set; }
        public static bool IsHittingUi => uiHitSources.Count > 0;

        static readonly HashSet<object> uiHitSources = new();

        public static Vector2 Position => Raylib.GetMousePosition();
        public static Vector2 Delta => Raylib.GetMouseDelta();

        public static bool IsBtnReleased(MouseButton button) => Raylib.IsMouseButtonReleased(button);
        public static bool IsBtnPressed(MouseButton button) => Raylib.IsMouseButtonPressed(button);
        public static bool IsBtnDown(MouseButton button) => Raylib.IsMouseButtonDown(button);
        public static bool IsBtnUp(MouseButton button) => Raylib.IsMouseButtonUp(button);

        public static void AddUiHit(object source)
        {
            uiHitSources.Add(source);
        }

        public static void RemoveUiHit(object source)
        {
            uiHitSources.Remove(source);
        }
    }
}
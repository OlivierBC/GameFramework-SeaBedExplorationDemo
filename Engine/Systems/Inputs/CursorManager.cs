using Raylib_cs;
using System.Numerics;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Inputs
{
    public static class CursorManager
    {
        static object? owner;
        static Vector2 positionBeforeLock;

        public static bool IsLocked => owner != null;

        public static bool TryLock(object requester)
        {
            if (owner != null)
                return ReferenceEquals(owner, requester);

            owner = requester;
            positionBeforeLock = MouseInputManager.Position;

            Raylib.DisableCursor();

            MouseInputManager.IsHitDetectionDisabled = true;

            return true;
        }

        public static bool IsLockedBy(object requester)
        {
            return ReferenceEquals(owner, requester);
        }

        public static void Unlock(object requester)
        {
            if (!IsLockedBy(requester))
                return;

            owner = null;

            Raylib.EnableCursor();

            Raylib.SetMousePosition((int)positionBeforeLock.X, (int)positionBeforeLock.Y);

            MouseInputManager.IsHitDetectionDisabled = false;
        }
    }
}
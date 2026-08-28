using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Inputs
{
    public struct InputAction : IEquatable<InputAction>
    {
        public readonly string name;
        public readonly Action action;
        public readonly KeyboardKey key;
        public readonly InputCallbackMode callbackMode;

        public InputAction(string name)
        {
            this.name = name;
            this.action = () => { };
        }
        public InputAction(string name, KeyboardKey key, InputCallbackMode callbackMode, Action action)
        {
            this.name = name;
            this.key = key;
            this.callbackMode = callbackMode;
            this.action = action;
        }

        public static bool operator ==(InputAction a, InputAction b)
        {
            return a.name == b.name;
        }

        public static bool operator !=(InputAction a, InputAction b)
        {
            return !(a == b);
        }

        public bool Equals(InputAction other)
        {
            return name == other.name;
        }

        public override bool Equals(object? other)
        {
            return other is InputAction inputAction && Equals(inputAction);
        }

        public override int GetHashCode()
        {
            return name?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// OnPressed/OnReleased is one frame, OnDown/OnUp is for all frames when true
        /// </summary>
        public enum InputCallbackMode
        {
            OnDown,
            OnUp,
            OnPressed,
            OnReleased
        }
    }
}
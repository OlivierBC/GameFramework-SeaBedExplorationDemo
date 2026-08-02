using UiIntegration.Engine.Types;

namespace UiIntegration.Engine.Collisions
{
    public class UIRect
    {
        public Vector2 Min { get; init; }
        public Vector2 Max { get; init; }

        public UIRect(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public bool Contains(Vector2 pos)
        {
            return Min.X <= pos.X && Min.Y <= pos.Y && Max.X >= pos.X && Max.Y >= pos.Y;
        }

        public float Height => Max.Y - Min.Y;
        public float Width => Max.X - Min.X;
    }
}

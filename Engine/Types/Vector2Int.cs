namespace UiIntegration.Engine.Types
{
    public class Vector2Int : IEquatable<Vector2Int>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static Vector2Int Zero => new(0, 0);

        public Vector2Int()
        {
            X = 0;
            Y = 0;
        }

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2Int(Vector2 v2)
        {
            X = (int)v2.X;
            Y = (int)v2.Y;
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2Int operator -(Vector2Int v)
        {
            return new Vector2Int(-v.X, -v.Y);
        }

        public static bool operator ==(Vector2Int? a, Vector2Int? b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null || b is null)
                return false;

            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2Int? a, Vector2Int? b)
        {
            return !(a == b);
        }

        public bool Equals(Vector2Int? other)
        {
            return this == other;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector2Int other && this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
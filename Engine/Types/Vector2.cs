namespace GameFramework_SeaBedExplorationDemo.Engine.Types
{
    public class Vector2 : IEquatable<Vector2>
    {
        public float X { get; set; }
        public float Y { get; set; }

        public static Vector2 Zero => new(0, 0);
        public static Vector2 One => new(1, 1);
        public static Vector2 UnitX => new(1, 0);
        public static Vector2 UnitY => new(0, 1);

        public Vector2()
        {
            X = 0;
            Y = 0;
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float Length()
        {
            return MathF.Sqrt(LengthSquared());
        }

        public float LengthSquared()
        {
            return X * X + Y * Y;
        }

        public static Vector2 Normalize(Vector2 v)
        {
            float length = v.Length();

            if (length <= 0.0001f)
                return Zero;

            return v / length;
        }

        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }

        public static Vector2 operator *(Vector2 v, float scalar)
        {
            return new Vector2(v.X * scalar, v.Y * scalar);
        }

        public static Vector2 operator *(float scalar, Vector2 v)
        {
            return new Vector2(v.X * scalar, v.Y * scalar);
        }

        public static Vector2 operator /(Vector2 v, float scalar)
        {
            return new Vector2(v.X / scalar, v.Y / scalar);
        }

        public static bool operator ==(Vector2? a, Vector2? b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null || b is null)
                return false;

            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2? a, Vector2? b)
        {
            return !(a == b);
        }

        public bool Equals(Vector2? other)
        {
            return this == other;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector2 other && this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public System.Numerics.Vector2 ToSystemNumerics()
        {
            return new System.Numerics.Vector2(X, Y);
        }

        public static Vector2 FromSystemNumerics(System.Numerics.Vector2 v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static implicit operator System.Numerics.Vector2(Vector2 v)
        {
            return new System.Numerics.Vector2(v.X, v.Y);
        }

        public static implicit operator Vector2(System.Numerics.Vector2 v)
        {
            return new Vector2(v.X, v.Y);
        }
    }
}
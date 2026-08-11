namespace GameFramework_SeaBedExplorationDemo.Engine.Types
{
    public struct Quaternion
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public static Quaternion Identity =>
            new(0f, 0f, 0f, 1f);

        public Quaternion(
            float x,
            float y,
            float z,
            float w
        )
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public float LengthSquared()
        {
            return
                X * X +
                Y * Y +
                Z * Z +
                W * W;
        }

        public float Length()
        {
            return MathF.Sqrt(LengthSquared());
        }

        public Quaternion Normalized()
        {
            return Normalize(this);
        }

        public static Quaternion Normalize(
            Quaternion quaternion
        )
        {
            float length = quaternion.Length();

            if (length < 0.0001f)
                return Identity;

            return new Quaternion(
                quaternion.X / length,
                quaternion.Y / length,
                quaternion.Z / length,
                quaternion.W / length
            );
        }

        public static Quaternion CreateFromAxisAngle(
            Vector3 axis,
            float angle
        )
        {
            if (axis.LengthSquared() < 0.0001f)
                return Identity;

            axis = Vector3.Normalize(axis);

            float halfAngle = angle * 0.5f;
            float sin = MathF.Sin(halfAngle);
            float cos = MathF.Cos(halfAngle);

            return new Quaternion(
                axis.X * sin,
                axis.Y * sin,
                axis.Z * sin,
                cos
            );
        }

        public void ToAxisAngle(
            out Vector3 axis,
            out float angle
        )
        {
            Quaternion q = Normalize(this);

            angle =
                2f *
                MathF.Acos(
                    Math.Clamp(q.W, -1f, 1f)
                );

            float sinHalfAngle =
                MathF.Sqrt(
                    MathF.Max(
                        0f,
                        1f - q.W * q.W
                    )
                );

            if (sinHalfAngle < 0.0001f)
            {
                axis = Vector3.UnitY;
                angle = 0f;
                return;
            }

            axis = new Vector3(
                q.X / sinHalfAngle,
                q.Y / sinHalfAngle,
                q.Z / sinHalfAngle
            );
        }
    }
}
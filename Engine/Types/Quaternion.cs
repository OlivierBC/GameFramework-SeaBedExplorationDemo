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

        public static Quaternion CreateFromEulerDegrees(
            Vector3 rotation
        )
        {
            float pitch = rotation.X * MathF.PI / 180f;
            float yaw = rotation.Y * MathF.PI / 180f;
            float roll = rotation.Z * MathF.PI / 180f;

            System.Numerics.Quaternion quaternion =
                System.Numerics.Quaternion.CreateFromYawPitchRoll(
                    yaw,
                    pitch,
                    roll
                );

            return new Quaternion(
                quaternion.X,
                quaternion.Y,
                quaternion.Z,
                quaternion.W
            );
        }

        public static Quaternion Concatenate(
            Quaternion first,
            Quaternion second
        )
        {
            System.Numerics.Quaternion quaternion =
                System.Numerics.Quaternion.Concatenate(
                    first.AsSystemNumerics,
                    second.AsSystemNumerics
                );

            return new Quaternion(
                quaternion.X,
                quaternion.Y,
                quaternion.Z,
                quaternion.W
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

        public System.Numerics.Quaternion AsSystemNumerics =>
            new(X, Y, Z, W);
        public Vector3 ToEulerDegrees()
        {
            Quaternion q =
                Normalize(this);

            float pitch =
                MathF.Asin(
                    Math.Clamp(
                        2f * (
                            q.W * q.X -
                            q.Y * q.Z
                        ),
                        -1f,
                        1f
                    )
                );

            float yaw =
                MathF.Atan2(
                    2f * (
                        q.X * q.Z +
                        q.W * q.Y
                    ),
                    1f -
                    2f * (
                        q.X * q.X +
                        q.Y * q.Y
                    )
                );

            float roll =
                MathF.Atan2(
                    2f * (
                        q.X * q.Y +
                        q.W * q.Z
                    ),
                    1f -
                    2f * (
                        q.X * q.X +
                        q.Z * q.Z
                    )
                );

            float radiansToDegrees =
                180f / MathF.PI;

            return new Vector3(
                pitch * radiansToDegrees,
                yaw * radiansToDegrees,
                roll * radiansToDegrees
            );
        }
    }
}
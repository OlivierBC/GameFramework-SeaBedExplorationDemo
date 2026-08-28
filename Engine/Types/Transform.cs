using System.Numerics;

namespace GameFramework_SeaBedExplorationDemo.Engine.Types
{
    public class Transform
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public float Scale = 1f;

        public Transform() { }

        public Transform(
            Vector3 position,
            Vector3 rotation,
            float scale
        )
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Quaternion ToQuaternion()
        {
            return Quaternion.CreateFromEulerDegrees(Rotation);
        }

        public Matrix4x4 ToMatrix()
        {
            Quaternion rotation = ToQuaternion();

            return Matrix4x4.CreateScale(Scale)
                * Matrix4x4.CreateFromQuaternion(rotation.AsSystemNumerics)
                * Matrix4x4.CreateTranslation(Position);
        }

        public Matrix4x4 ToRaylibMatrix()
        {
            return Matrix4x4.Transpose(ToMatrix());
        }
    }
}
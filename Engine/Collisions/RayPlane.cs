using GameFramework_SeaBedExplorationDemo.Engine.Types;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene.SceneEditor
{
    public class RayPlane
    {
        public Vector3 Point;
        public Vector3 Normal;

        public RayPlane(Vector3 point, Vector3 normal)
        {
            Point = point;
            Normal = normal;
        }

        public bool Raycast(Ray ray, out Vector3 hit)
        {
            hit = Vector3.Zero;

            float denominator = Vector3.Dot(ray.Direction, Normal);

            if (MathF.Abs(denominator) < 0.0001f)
                return false;

            float distance = Vector3.Dot(Point.AsSystemNumerics - ray.Position, Normal) / denominator;

            if (distance < 0f)
                return false;

            hit = ray.Position + ray.Direction * distance;

            return true;
        }
    }
}
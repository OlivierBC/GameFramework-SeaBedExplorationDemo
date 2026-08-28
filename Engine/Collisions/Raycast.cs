using Raylib_cs;
using Matrix4x4 = System.Numerics.Matrix4x4;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene.SceneEditor
{
    public static class Raycast
    {
        public static unsafe bool TryModel(Ray ray, Model model, Matrix4x4 matrix, out RayCollision closestCollision)
        {
            closestCollision = new();
            float closestDistance = float.MaxValue;
            bool hit = false;

            for (int mesh = 0; mesh < model.MeshCount; mesh++)
            {
                RayCollision collision = Raylib.GetRayCollisionMesh(ray, model.Meshes[mesh], matrix);

                if (!collision.Hit || collision.Distance >= closestDistance)
                    continue;

                closestDistance = collision.Distance;
                closestCollision = collision;
                hit = true;
            }

            return hit;
        }

        public static bool TryStaticGeometrys(Ray ray, IEnumerable<SceneStaticGeometry> staticGeometrys, out SceneStaticGeometry? closestObject, out RayCollision closestCollision)
        {
            closestObject = null;
            closestCollision = new();

            float closestDistance = float.MaxValue;

            foreach (SceneStaticGeometry staticGeometry in staticGeometrys)
            {
                if (!TryModel(ray, staticGeometry.Model, staticGeometry.Transform.ToRaylibMatrix(), out RayCollision collision))
                    continue;

                if (collision.Distance >= closestDistance)
                    continue;

                closestDistance = collision.Distance;
                closestCollision = collision;
                closestObject = staticGeometry;
            }

            return closestObject != null;
        }
    }
}
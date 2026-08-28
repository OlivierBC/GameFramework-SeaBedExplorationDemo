using GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using GameFramework_SeaBedExplorationDemo.Project.ECS.Components;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Project.ECS.Systems
{
    public class ModelRenderSystem : ISystem
    {
        private readonly ECSWorld world;

        public ModelRenderSystem(ECSWorld world)
        {
            this.world = world;
        }

        public void Draw()
        {
            foreach (var (entity, transform, modelComp) in world.Query<TransformComponent, ModelComponent>())
            {
                Quaternion rotation =
                    Quaternion.CreateFromEulerDegrees(transform.Rotation);

                rotation.ToAxisAngle(
                    out Vector3 axis,
                    out float angle
                );

                Raylib.DrawModelEx(
                    modelComp.Model,
                    transform.Position,
                    axis,
                    angle * 180f / MathF.PI,
                    Vector3.One * transform.Scale,
                    Color.RayWhite
                );
            }
        }
    }
}
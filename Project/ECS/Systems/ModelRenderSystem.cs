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
                Quaternion rotation = Quaternion.Normalize(transform.Rotation);

                float angle =
                    2f * MathF.Acos(
                        Math.Clamp(rotation.W, -1f, 1f)
                    );

                float sinHalfAngle =
                    MathF.Sqrt(
                        MathF.Max(
                            0f,
                            1f - rotation.W * rotation.W
                        )
                    );

                Vector3 axis;

                if (sinHalfAngle < 0.001f)
                {
                    axis = Vector3.UnitY;
                }
                else
                {
                    axis = new Vector3(
                        rotation.X / sinHalfAngle,
                        rotation.Y / sinHalfAngle,
                        rotation.Z / sinHalfAngle
                    );
                }

                float angleDegrees = angle * 180f / MathF.PI;

                Raylib.DrawModelEx(
                    modelComp.Model,
                    transform.Position,
                    axis,
                    angleDegrees,
                    Vector3.One * 10f,
                    Color.RayWhite
                );
            }
        }
    }
}
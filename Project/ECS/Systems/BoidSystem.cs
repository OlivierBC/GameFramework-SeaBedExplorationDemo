using GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Geometry.Terrain;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Resource;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using GameFramework_SeaBedExplorationDemo.Project.ECS.Components;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Project.ECS.Systems
{
    public class BoidSystem : ISystem
    {
        private readonly ECSWorld world;

        private readonly int nbBoids;
        private readonly float spawnRadius;
        private readonly float speed;

        private readonly float terrainClearance;
        private readonly float terrainAvoidanceDistance;

        private readonly string boidModelName;

        private readonly float modelScale;

        public BoidSystem(
            ECSWorld world,
            int nbBoids = 200,
            float spawnRadius = 250f,
            float speed = 10f,
            float terrainClearance = 3f,
            float terrainAvoidanceDistance = 10f,
            string boidModelName = "fish",
            float modelScale = 15f
        )
        {
            this.world = world;
            this.nbBoids = nbBoids;
            this.spawnRadius = spawnRadius;
            this.speed = speed;
            this.terrainClearance = terrainClearance;
            this.terrainAvoidanceDistance = terrainAvoidanceDistance;
            this.boidModelName = boidModelName;
            this.modelScale = modelScale;
        }

        public void Load()
        {
            for (int i = 0; i < nbBoids; i++)
            {
                Entity boid = world.CreateEntity();
                Model model = ModelLibrary.GetModel(boidModelName);

                world.AddComponent(
                    boid,
                    new TransformComponent(new(new(), Vector3.Zero, modelScale))
                );

                world.AddComponent(
                    boid,
                    new VelocityComponent()
                );

                world.AddComponent(
                    boid,
                    new BoidComponent()
                );

                world.AddComponent(
                    boid,
                    new CameraRangeComponent(
                        400f,
                        100f,
                        250f
                    )
                );

                world.AddComponent(
                    boid,
                    new ModelComponent(model)
                );
            }

            foreach (var (entity, transform, velocity, boid) in
                world.Query<
                    TransformComponent,
                    VelocityComponent,
                    BoidComponent
                >())
            {
                transform.Position = RandomPosition();
                velocity.Velocity = RandomDirection() * speed;
            }
        }

        public void Update(float dt)
        {
            var boids = world.Query<
                TransformComponent,
                VelocityComponent,
                BoidComponent
            >().ToArray();

            Vector3[] newVelocities = new Vector3[boids.Length];

            for (int i = 0; i < boids.Length; i++)
            {
                var (
                    entity,
                    transform,
                    velocity,
                    boid
                ) = boids[i];

                Vector3 position = transform.Position;

                float currentSpeed = velocity.Velocity.Length();

                Vector3 direction =
                    currentSpeed > 0.001f
                        ? Vector3.Normalize(velocity.Velocity)
                        : RandomDirection();

                Vector3 separation = Vector3.Zero;
                Vector3 averagePosition = Vector3.Zero;
                Vector3 averageDirection = Vector3.Zero;

                int nbClose = 0;

                for (int j = 0; j < boids.Length; j++)
                {
                    if (i == j)
                        continue;

                    var (
                        otherEntity,
                        otherTransform,
                        otherVelocity,
                        otherBoid
                    ) = boids[j];

                    Vector3 away =
                        position - otherTransform.Position;

                    float distance = away.Length();

                    if (distance > boid.detectionDistance)
                        continue;

                    if (distance > 0.001f)
                    {
                        float strength =
                            1f -
                            distance /
                            boid.detectionDistance;

                        strength *= strength;

                        separation +=
                            Vector3.Normalize(away) *
                            strength;
                    }

                    averagePosition += otherTransform.Position;

                    if (otherVelocity.Velocity.LengthSquared() > 0.0001f)
                    {
                        averageDirection +=
                            Vector3.Normalize(
                                otherVelocity.Velocity
                            );
                    }

                    nbClose++;
                }

                Vector3 cohesion = Vector3.Zero;
                Vector3 alignment = Vector3.Zero;

                if (nbClose > 0)
                {
                    averagePosition /= nbClose;

                    cohesion =
                        averagePosition -
                        position;

                    if (cohesion.LengthSquared() > 0.0001f)
                        cohesion = Vector3.Normalize(cohesion);

                    averageDirection /= nbClose;

                    if (averageDirection.LengthSquared() > 0.0001f)
                    {
                        alignment =
                            Vector3.Normalize(
                                averageDirection
                            );
                    }
                }

                float terrainHeight = TerrainPlane.GetVertexHeight(position.X, position.Z);

                float heightAboveTerrain =
                    position.Y - terrainHeight;

                Vector3 terrainAvoidance =
                    Vector3.Zero;

                if (heightAboveTerrain < terrainAvoidanceDistance)
                {
                    float strength =
                        1f -
                        Math.Clamp(
                            heightAboveTerrain /
                            terrainAvoidanceDistance,
                            0f,
                            1f
                        );

                    terrainAvoidance =
                        Vector3.UnitY *
                        strength *
                        3f;
                }

                Vector3 targetDirection =
                    direction +
                    separation +
                    cohesion +
                    alignment +
                    terrainAvoidance;

                if (targetDirection.LengthSquared() > 0.0001f)
                    targetDirection =
                        Vector3.Normalize(targetDirection);
                else
                    targetDirection = direction;

                Vector3 newDirection =
                    Vector3.Lerp(
                        direction,
                        targetDirection,
                        boid.turnSpeed * dt
                    );

                if (newDirection.LengthSquared() > 0.0001f)
                    newDirection =
                        Vector3.Normalize(newDirection);

                if (currentSpeed <= 0.001f)
                    currentSpeed = speed;

                newVelocities[i] =
                    newDirection * currentSpeed;
            }

            for (int i = 0; i < boids.Length; i++)
            {
                var (
                    entity,
                    transform,
                    velocity,
                    boid
                ) = boids[i];

                velocity.Velocity = newVelocities[i];

                transform.Position +=
                    velocity.Velocity * dt;

                float terrainHeight =
                    TerrainPlane.GetVertexHeight(
                        transform.Position.X,
                        transform.Position.Z
                    );

                float minimumY =
                    terrainHeight +
                    terrainClearance;

                if (transform.Position.Y < minimumY)
                {
                    transform.Position = new Vector3(
                        transform.Position.X,
                        minimumY,
                        transform.Position.Z
                    );

                    if (velocity.Velocity.Y < 0)
                    {
                        velocity.Velocity = new Vector3(
                            velocity.Velocity.X,
                            0,
                            velocity.Velocity.Z
                        );

                        if (velocity.Velocity.LengthSquared() > 0.0001f)
                        {
                            velocity.Velocity =
                                Vector3.Normalize(
                                    velocity.Velocity
                                ) * speed;
                        }
                    }
                }
                transform.Rotation = DirectionToRotation(velocity.Velocity);
            }
        }

        public void Unload()
        {
            ModelLibrary.UnloadModel(boidModelName);
        }

        private static Vector3 DirectionToRotation(Vector3 direction)
        {
            if (direction.LengthSquared() < 0.0001f)
                return Vector3.Zero;

            direction = Vector3.Normalize(direction);

            float yaw = MathF.Atan2(
                direction.X,
                direction.Z
            );

            float pitch = -MathF.Asin(
                Math.Clamp(direction.Y, -1f, 1f)
            );

            return new Vector3(
                pitch * 180f / MathF.PI,
                yaw * 180f / MathF.PI,
                0f
            );
        }

        private Vector3 RandomDirection()
        {
            float y =
                Random.Shared.NextSingle() * 2f - 1f;

            float angle =
                Random.Shared.NextSingle() *
                MathF.PI *
                2f;

            float horizontal =
                MathF.Sqrt(1f - y * y);

            return new Vector3(
                horizontal * MathF.Cos(angle),
                y,
                horizontal * MathF.Sin(angle)
            );
        }

        private Vector3 RandomPosition()
        {
            float x =
                RandomRange(
                    -spawnRadius,
                    spawnRadius
                );

            float z =
                RandomRange(
                    -spawnRadius,
                    spawnRadius
                );

            float terrainHeight =
                TerrainPlane.GetVertexHeight(
                    x,
                    z
                );

            float y =
                terrainHeight +
                RandomRange(
                    terrainClearance + 10f,
                    terrainClearance + spawnRadius
                );

            return new Vector3(
                x,
                y,
                z
            );
        }

        private float RandomRange(float min, float max)
        {
            return min +
                Random.Shared.NextSingle() *
                (max - min);
        }
    }
}
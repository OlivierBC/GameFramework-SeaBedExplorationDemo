using GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using GameFramework_SeaBedExplorationDemo.Project.ECS.Components;

public class CameraRangeSystem : ISystem
{
    private readonly ECSWorld world;
    private readonly Func<Vector3> getCameraPosition;
    private readonly Func<Vector3> getCameraForward;

    public CameraRangeSystem(
        ECSWorld world,
        Func<Vector3> getCameraPosition,
        Func<Vector3> getCameraForward
    )
    {
        this.world = world;
        this.getCameraPosition = getCameraPosition;
        this.getCameraForward = getCameraForward;
    }

    public void Update(float dt)
    {
        Vector3 cameraPosition = getCameraPosition();
        Vector3 cameraForward = Vector3.Normalize(getCameraForward());

        foreach (var (entity, transform, range) in world.Query<TransformComponent, CameraRangeComponent>())
        {
            Vector3 offset =
                transform.Position - cameraPosition;

            if (offset.LengthSquared() <=
                range.MaxDistance * range.MaxDistance)
                continue;

            Vector3 direction =
                RandomDirectionBehind(cameraForward);

            float distance =
                range.RepositionMinDistance +
                Random.Shared.NextSingle() *
                (range.RepositionMaxDistance -
                 range.RepositionMinDistance);

            transform.Position =
                cameraPosition +
                direction * distance;
        }
    }

    private Vector3 RandomDirectionBehind(Vector3 cameraForward)
    {
        Vector3 backward = -cameraForward;

        float maxAngle = MathF.PI / 3f;

        float cosMaxAngle = MathF.Cos(maxAngle);

        float cosTheta =
            cosMaxAngle +
            Random.Shared.NextSingle() *
            (1f - cosMaxAngle);

        float sinTheta =
            MathF.Sqrt(1f - cosTheta * cosTheta);

        float phi =
            Random.Shared.NextSingle() *
            MathF.PI *
            2f;

        Vector3 reference =
            MathF.Abs(backward.Y) < 0.99f
                ? Vector3.UnitY
                : Vector3.UnitX;

        Vector3 right =
            Vector3.Normalize(
                Vector3.Cross(reference, backward)
            );

        Vector3 up =
            Vector3.Normalize(
                Vector3.Cross(backward, right)
            );

        Vector3 direction =
            backward * cosTheta +
            right * MathF.Cos(phi) * sinTheta +
            up * MathF.Sin(phi) * sinTheta;

        return Vector3.Normalize(direction);
    }
}
using GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS;
using GameFramework_SeaBedExplorationDemo.Project.ECS.Components;

public class MovementSystem : ISystem
{
    private readonly ECSWorld world;

    public MovementSystem(ECSWorld world)
    {
        this.world = world;
    }

    public void Update(float dt)
    {
        foreach (var (entity, transform, velocity) in world.Query<TransformComponent, VelocityComponent>())
        {
            transform.Position += velocity.Velocity * dt;
        }
    }
}
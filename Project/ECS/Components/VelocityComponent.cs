using GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS;
using GameFramework_SeaBedExplorationDemo.Engine.Types;

namespace GameFramework_SeaBedExplorationDemo.Project.ECS.Components
{
    public class VelocityComponent : IComponent
    {
        public Vector3 Velocity = new();
    }
}

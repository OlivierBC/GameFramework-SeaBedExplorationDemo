using GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS;

namespace GameFramework_SeaBedExplorationDemo.Project.ECS.Components
{
    public class BoidComponent : IComponent
    {
        public float turnSpeed = 4f;
        public float detectionDistance = 50f;
    }
}

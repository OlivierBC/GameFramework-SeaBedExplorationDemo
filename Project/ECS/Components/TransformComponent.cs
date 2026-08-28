using GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS;
using GameFramework_SeaBedExplorationDemo.Engine.Types;

namespace GameFramework_SeaBedExplorationDemo.Project.ECS.Components
{
    public class TransformComponent : Transform, IComponent
    {
        public TransformComponent() { }
        public TransformComponent(Transform transform) : base(transform.Position, transform.Rotation, transform.Scale) { }
    }
}

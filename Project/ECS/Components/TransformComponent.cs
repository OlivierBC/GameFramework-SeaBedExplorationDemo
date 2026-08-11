using GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS;
using GameFramework_SeaBedExplorationDemo.Engine.Types;

namespace GameFramework_SeaBedExplorationDemo.Project.ECS.Components
{
    public class TransformComponent : IComponent
    {
        public Vector3 Position = new();
        public Quaternion Rotation = Quaternion.Identity;
    }
}

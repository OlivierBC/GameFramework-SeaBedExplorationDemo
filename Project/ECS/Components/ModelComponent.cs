using GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Project.ECS.Components
{
    public class ModelComponent : IComponent
    {
        public Model Model;

        public ModelComponent(Model model)
        {
            Model = model;
        }
    }
}

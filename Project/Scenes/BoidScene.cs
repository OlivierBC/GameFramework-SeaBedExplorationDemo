using GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Geometry.Terrain;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene;
using GameFramework_SeaBedExplorationDemo.Project.ECS.Systems;
using GameFramework_SeaBedExplorationDemo.Project.Movements;
using GameFramework_SeaBedExplorationDemo.resources.shaders;

namespace GameFramework_SeaBedExplorationDemo.Project.Scenes
{
    public class BoidScene : Scene
    {
        ECSWorld ECS = new();
        public BoidScene()
        {
            MovementSystem movementSys = new(ECS);

            BoidSystem boidSys = new(ECS);

            ModelRenderSystem modelRenderSys = new(ECS);

            CameraRangeSystem cameraRangeSys = new(ECS, Camera.GetPosition, Camera.GetForward);

            ECS.AddSystem(cameraRangeSys);
            ECS.AddSystem(boidSys);
            ECS.AddSystem(movementSys);
            ECS.AddSystem(modelRenderSys);

            registry.Add(ECS);

            TerrainShader.Load(); // this should be in Load() (from Scene parent class), but it needs to be loaded before the creation of TerrainPlane; TerrainPlane and TerrainShader will be changed in further integrations

            registry.Add(new TerrainPlane());
        }
    }
}

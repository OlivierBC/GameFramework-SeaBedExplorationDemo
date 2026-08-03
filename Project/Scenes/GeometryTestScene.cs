using GameFramework_SeaBedExplorationDemo.Engine.Systems.Geometry;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Project.Scenes
{
    public class GeometryTestScene : Scene
    {
        public GeometryTestScene()
        {
            registry.Add(new Grid(
                40,
                1.0f
            ));

            registry.Add(new GroundPlane(
                new Vector3(0.0f, -0.01f, 0.0f),
                new Vector2(40.0f, 40.0f),
                new Color(30, 70, 80, 255)
            ));

            registry.Add(new Cube(
                new Vector3(-3.0f, 1.0f, 0.0f),
                new Vector3(2.0f, 2.0f, 2.0f),
                Color.Red
            ));

            registry.Add(new Sphere(
                new Vector3(0.0f, 1.0f, 0.0f),
                1.0f,
                Color.Green
            ));

            registry.Add(new Cylinder(
                new Vector3(3.0f, 0.0f, 0.0f),
                1.0f,
                1.0f,
                2.0f,
                Color.Blue
            ));
        }
    }
}
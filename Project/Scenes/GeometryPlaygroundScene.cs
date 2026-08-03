using GameFramework_SeaBedExplorationDemo.Engine.Systems.Geometry;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Project.Scenes
{
    public class GeometryPlaygroundScene : Scene
    {
        public GeometryPlaygroundScene()
        {
            registry.Add(new Grid(
                50,
                1.0f
            ));

            registry.Add(new GroundPlane(
                new Vector3(0.0f, -0.01f, 0.0f),
                new Vector2(50.0f, 50.0f),
                new Color(35, 65, 70, 255)
            ));

            registry.Add(new Cylinder(
                new Vector3(0.0f, 0.0f, 0.0f),
                2.5f,
                2.5f,
                0.5f,
                new Color(90, 100, 110, 255)
            ));

            registry.Add(new Sphere(
                new Vector3(0.0f, 2.0f, 0.0f),
                1.5f,
                new Color(80, 180, 170, 255)
            ));

            registry.Add(new Cylinder(
                new Vector3(-6.0f, 0.0f, -6.0f),
                0.75f,
                0.75f,
                4.0f,
                Color.Red
            ));

            registry.Add(new Cylinder(
                new Vector3(6.0f, 0.0f, -6.0f),
                0.75f,
                0.75f,
                4.0f,
                Color.Green
            ));

            registry.Add(new Cylinder(
                new Vector3(-6.0f, 0.0f, 6.0f),
                0.75f,
                0.75f,
                4.0f,
                Color.Blue
            ));

            registry.Add(new Cylinder(
                new Vector3(6.0f, 0.0f, 6.0f),
                0.75f,
                0.75f,
                4.0f,
                Color.Yellow
            ));

            registry.Add(new Cube(
                new Vector3(0.0f, 1.0f, -8.0f),
                new Vector3(10.0f, 2.0f, 1.0f),
                new Color(120, 80, 60, 255)
            ));

            registry.Add(new Cube(
                new Vector3(-8.0f, 1.0f, 0.0f),
                new Vector3(1.0f, 2.0f, 10.0f),
                new Color(120, 80, 60, 255)
            ));

            registry.Add(new Cube(
                new Vector3(5.0f, 0.5f, 0.0f),
                new Vector3(2.0f, 1.0f, 3.0f),
                new Color(150, 120, 80, 255)
            ));

            registry.Add(new Cube(
                new Vector3(7.0f, 1.0f, 0.0f),
                new Vector3(2.0f, 2.0f, 3.0f),
                new Color(170, 135, 90, 255)
            ));

            registry.Add(new Cube(
                new Vector3(9.0f, 1.5f, 0.0f),
                new Vector3(2.0f, 3.0f, 3.0f),
                new Color(190, 150, 100, 255)
            ));

            registry.Add(new Sphere(
                new Vector3(-4.0f, 0.75f, 2.0f),
                0.75f,
                Color.Purple
            ));

            registry.Add(new Sphere(
                new Vector3(-2.0f, 1.0f, 4.0f),
                1.0f,
                Color.Orange
            ));

            registry.Add(new Sphere(
                new Vector3(-4.0f, 1.25f, 6.0f),
                1.25f,
                Color.Pink
            ));
        }
    }
}
using GameFramework_SeaBedExplorationDemo.Engine.Base;
using GameFramework_SeaBedExplorationDemo.Engine.Registries.Observables;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Geometry
{
    public class Sphere : GameElement, IDrawable3D
    {
        public float Radius { get; set; }
        public Color Color { get; set; }

        public Sphere(Vector3 position, float radius, Color color) : base(position)
        {
            Radius = radius;
            Color = color;
        }

        public void Draw()
        {
            Raylib.DrawSphere(
                Transform.Position,
                Radius,
                Color
            );
        }
    }
}
using GameFramework_SeaBedExplorationDemo.Engine.Base;
using GameFramework_SeaBedExplorationDemo.Engine.Registries.Observables;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Geometry
{
    public class Cylinder : GameElement, IDrawable3D
    {
        public float TopRadius { get; set; }
        public float BottomRadius { get; set; }
        public float Height { get; set; }
        public int Sides { get; set; }
        public Color Color { get; set; }

        public Cylinder(Vector3 position, float topRadius, float bottomRadius, float height, Color color, int sides = 16) : base(position)
        {
            TopRadius = topRadius;
            BottomRadius = bottomRadius;
            Height = height;
            Color = color;
            Sides = sides;
        }

        public void Draw()
        {
            Raylib.DrawCylinder(
                Transform.Position,
                TopRadius,
                BottomRadius,
                Height,
                Sides,
                Color
            );
        }
    }
}
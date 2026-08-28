using GameFramework_SeaBedExplorationDemo.Engine.Base;
using GameFramework_SeaBedExplorationDemo.Engine.Registries.Observables;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Geometry
{
    public class Cube : GameElement, IDrawable3D
    {
        public Vector3 Size { get; set; }
        public Color Color { get; set; }

        public Cube(Vector3 position, Vector3 size, Color color) : base(position)
        {
            Size = size;
            Color = color;
        }

        public void Draw()
        {
            Raylib.DrawCube(
                Transform.Position,
                Size.X,
                Size.Y,
                Size.Z,
                Color
            );
        }
    }
}
using GameFramework_SeaBedExplorationDemo.Engine.Base;
using GameFramework_SeaBedExplorationDemo.Engine.Registries.Observables;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Geometry
{
    public class GroundPlane : GameElement, IDrawable3D
    {
        public Vector2 Size { get; set; }
        public Color Color { get; set; }

        public GroundPlane(Vector3 position, Vector2 size, Color color) : base(position)
        {
            Size = size;
            Color = color;
        }

        public void Draw()
        {
            Raylib.DrawPlane(
                Transform.Position,
                Size,
                Color
            );
        }
    }
}
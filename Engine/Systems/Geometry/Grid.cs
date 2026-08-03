using GameFramework_SeaBedExplorationDemo.Engine.Base;
using GameFramework_SeaBedExplorationDemo.Engine.Observers.Observables;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Geometry
{
    public class Grid : GameObject, IDrawable3D
    {
        public int Slices { get; set; }
        public float Spacing { get; set; }

        public Grid(int slices = 20, float spacing = 1.0f)
        {
            Slices = slices;
            Spacing = spacing;
        }

        public void Draw()
        {
            Raylib.DrawGrid(Slices, Spacing);
        }
    }
}
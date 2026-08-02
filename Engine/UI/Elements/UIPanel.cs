using Raylib_cs;
using GameFramework_SeaBedExplorationDemo.Engine.Collisions;
using GameFramework_SeaBedExplorationDemo.Engine.UI.Data;

namespace GameFramework_SeaBedExplorationDemo.Engine.UI.Elements
{
    internal class UIPanel : UIElement
    {
        public Color Color;
        public UIPanel(Color color, Margin padding) : base(new UIRect(new(0 + padding.Values.Left, 0 + padding.Values.Top), new(Raylib.GetScreenWidth() - padding.Values.Right, Raylib.GetScreenHeight() - padding.Values.Bottom)))
        {
            Color = color;
        }

        public override void Draw()
        {
            if (!IsEnabled) return;

            Raylib.DrawRectangle(
                (int)Bounds.Min.X,
                (int)Bounds.Min.Y,
                (int)Bounds.Width,
                (int)Bounds.Height,
                Color
            );

            base.Draw();
        }
    }
}

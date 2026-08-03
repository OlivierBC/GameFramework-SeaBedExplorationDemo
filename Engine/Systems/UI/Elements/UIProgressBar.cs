using Raylib_cs;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.UI;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.UI.Elements
{
    public class UIProgressBar : UIElement
    {
        public Color BackColor;
        public Color FillColor;
        public float Value { get; private set; } = 0f;
        public float MaxValue;
        public Direction FillDirection;

        float FillWidth => Value / MaxValue * Bounds.Width;

        public UIProgressBar(Vector2 min, Vector2 max, float maxValue, Color backColor, Color fillColor, Direction fillDirection = Direction.Right) : base(new(min, max))
        {
            MaxValue = maxValue;
            FillDirection = fillDirection;
            BackColor = backColor;
            FillColor = fillColor;
        }

        public override void Draw()
        {
            if (!IsEnabled) return;

            int fillWidth = (int)FillWidth;

            Raylib.DrawRectangle(
                (int)(Bounds.Min.X) + fillWidth,
                (int)Bounds.Min.Y,
                (int)(Bounds.Width) - fillWidth,
                (int)Bounds.Height,
                BackColor
            );

            Raylib.DrawRectangle(
                (int)Bounds.Min.X,
                (int)Bounds.Min.Y,
                fillWidth,
                (int)Bounds.Height,
                FillColor
            );

            base.Draw();
        }

        public void SetValue(float newVal)
        {
            if (newVal < 0)
                Value = 0;
            else if (newVal > MaxValue)
                Value = MaxValue;
            else
                Value = newVal;
        }
    }
}

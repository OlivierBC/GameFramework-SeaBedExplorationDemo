using Raylib_cs;
using UiIntegration.Engine.GameObjects;
using UiIntegration.Engine.Types;

namespace UiIntegration.Engine.UI.Elements.Interactable
{
    internal class UISlider : UIElement
    {
        public float Value { get; private set; } = 0f;
        public float MaxValue;
        public Direction FillDirection;

        float FillWidth => Value / MaxValue * Bounds.Width;

        public UISlider(Vector2 min, Vector2 max, float maxValue, Direction fillDirection = Direction.Right) : base(new(min, max))
        {
            MaxValue = maxValue;
            FillDirection = fillDirection;
        }

        public override void Draw()
        {
            if (!IsEnabled) return;

            int fillWidth = (int)FillWidth;

            Color backColor = IsCaptured ? Color.DarkGray :
                          IsHovered ? Color.Gray :
                          Color.LightGray;

            Raylib.DrawRectangle(
                (int)(Bounds.Min.X) + fillWidth,
                (int)Bounds.Min.Y,
                (int)(Bounds.Width) - fillWidth,
                (int)Bounds.Height,
                backColor
            );

            Raylib.DrawRectangle(
                (int)Bounds.Min.X,
                (int)Bounds.Min.Y,
                fillWidth,
                (int)Bounds.Height,
                Color.Yellow
            );

            base.Draw();
        }

        private void SetValueOnMousePos()
        {
            SetValue((Mouse.Position.X - Bounds.Min.X) / Bounds.Width * MaxValue);
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

        public override void Tick(float dt) => SetValueOnMousePos();
    }
}

using GameFramework_SeaBedExplorationDemo.Engine.Systems.UI.Structure;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.UI.Elements.Interactable
{
    public class UIButton : UIElement
    {
        public readonly string Text;
        public TextAlign TextAlign;
        public Action? OnClick;

        float distanceFromLeftInPx;

        public UIButton(Vector2 min, Vector2 max, string text, Action? onClick, TextAlign textAlign = TextAlign.Center) : base(new(min, max))
        {
            Text = text;
            TextAlign = textAlign;
            OnClick = onClick;

            float maxDistanceFromLeft = Bounds.Width - Raylib.MeasureText(Text, 20);
            distanceFromLeftInPx = (TextAlign == TextAlign.Left ? 3f : TextAlign == TextAlign.Center ? maxDistanceFromLeft * 0.5f : maxDistanceFromLeft);
        }

        public override void Draw()
        {
            if (!IsEnabled) return;

            Color color = IsPressed ? Color.DarkGray :
                          IsHovered ? Color.Gray :
                          Color.LightGray;

            Raylib.DrawRectangle(
                (int)Bounds.Min.X,
                (int)Bounds.Min.Y,
                (int)Bounds.Width,
                (int)Bounds.Height,
                color
            );

            Raylib.DrawText(Text, (int)(Bounds.Min.X + distanceFromLeftInPx), (int)(Bounds.Min.Y + 3f), 20, Color.Black); // 3f are really hardcoded here TODO

            base.Draw();
        }

        public override void Click() => OnClick?.Invoke();
    }
}
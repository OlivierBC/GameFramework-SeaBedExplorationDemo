using Raylib_cs;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using GameFramework_SeaBedExplorationDemo.Engine.UI.Data;

namespace GameFramework_SeaBedExplorationDemo.Engine.UI.Elements.Interactable
{
    public class UIButton : UIElement
    {
        public string Text;
        public TextAlign TextAlign;
        public Action? OnClick;

        public UIButton(Vector2 min, Vector2 max, string text, Action? onClick, TextAlign textAlign = TextAlign.Center) : base(new(min, max))
        {
            Text = text;
            TextAlign = textAlign;
            OnClick = onClick;
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

            float distanceFromLeft = Bounds.Width - Raylib.MeasureText(Text, 20);

            distanceFromLeft = distanceFromLeft * (TextAlign == TextAlign.Left ? 0.05f : TextAlign == TextAlign.Center ? 0.5f : 0.95f);

            Raylib.DrawText(Text, (int)(Bounds.Min.X + distanceFromLeft), (int)(Bounds.Min.Y * 1.05f), 20, Color.Black);

            base.Draw();
        }

        public override void Click() => OnClick?.Invoke();
    }
}
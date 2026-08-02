using Raylib_cs;
using UiIntegration.Engine.Types;
using UiIntegration.Engine.UI.Data;

namespace UiIntegration.Engine.UI.Elements
{
    internal class UILabel : UIElement
    {
        public string Text;
        public Color BackColor;
        public TextAlign TextAlign;

        public UILabel(Vector2 min, Vector2 max, Color backColor, string text, TextAlign textAlign = TextAlign.Left) : base(new(min, max))
        {
            Text = text;
            BackColor = backColor;
            TextAlign = textAlign;
        }

        public override void Draw()
        {
            if (!IsEnabled) return;

            Raylib.DrawRectangle(
                (int)Bounds.Min.X,
                (int)Bounds.Min.Y,
                (int)Bounds.Width,
                (int)Bounds.Height,
                BackColor
            );

            float distanceFromLeft = Bounds.Width - Raylib.MeasureText(Text, 20);

            distanceFromLeft = distanceFromLeft * (TextAlign == TextAlign.Left ? 0.05f : TextAlign == TextAlign.Center ? 0.5f : 0.95f);

            Raylib.DrawText(Text, (int)(Bounds.Min.X + distanceFromLeft), (int)(Bounds.Min.Y + Bounds.Height * 0.05f), 20, Color.Black);

            base.Draw();
        }
    }
}

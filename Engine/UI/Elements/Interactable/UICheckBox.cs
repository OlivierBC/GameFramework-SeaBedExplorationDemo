using Raylib_cs;
using UiIntegration.Engine.Types;

namespace UiIntegration.Engine.UI.Elements.Interactable
{
    internal class UICheckBox : UIElement
    {
        public bool isChecked;

        public UICheckBox(Vector2 min, Vector2 max, bool startChecked = false) : base(new(min, max))
        {
            isChecked = startChecked;
        }

        public override void Draw()
        {
            if (!IsEnabled) return;

            Color color = IsPressed ? Color.DarkGray :
                          IsHovered ? Color.Gray :
                          Color.LightGray;

            if (isChecked)
                Raylib.DrawRectangle(
                    (int)Bounds.Min.X,
                    (int)Bounds.Min.Y,
                    (int)Bounds.Width,
                    (int)Bounds.Height,
                    color
                );
            else
                Raylib.DrawRectangleLines(
                    (int)Bounds.Min.X,
                    (int)Bounds.Min.Y,
                    (int)Bounds.Width,
                    (int)Bounds.Height,
                    color
                );

            base.Draw();
        }

        public void ToggleCheck()
        {
            isChecked = !isChecked;
        }

        public override void Click() => ToggleCheck();
    }
}

using GameFramework_SeaBedExplorationDemo.Engine.Collisions;
using GameFramework_SeaBedExplorationDemo.Engine.Types;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.UI
{
    public abstract class UIElement
    {
        public UIRect Bounds;

        public bool IsEnabled = true;

        public bool IsHovered = false;
        public bool IsCaptured = false;
        public bool IsPressed = false;

        public List<UIElement> Children = new();

        protected UIElement(UIRect bounds)
        {
            Bounds = bounds;
        }

        public virtual void Draw()
        {
            if (!IsEnabled) return;
            foreach (UIElement child in Children)
                child.Draw();
        }

        public bool Contains(Vector2 point)
        {
            return Bounds.Contains(point);
        }

        public virtual void Click() { }
        public virtual void Tick(float dt) { }

        public void Toggle()
        {
            IsEnabled = !IsEnabled;

            if (!IsEnabled)
            {
                Reset();
            }
        }

        public void Reset()
        {
            IsHovered = false;
            IsPressed = false;
            IsCaptured = false;
        }
    }
}

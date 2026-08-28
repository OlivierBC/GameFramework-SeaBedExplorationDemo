using GameFramework_SeaBedExplorationDemo.Engine.Collisions;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using GameFramework_SeaBedExplorationDemo.Engine.Types.Interface;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.UI
{
    public abstract class UIElement : IToggleable
    {
        public UIRect Bounds;

        public bool IsHovered = false;
        public bool IsCaptured = false;
        public bool IsPressed = false;

        public List<UIElement> Children = new();

        public bool IsEnabled { get; private set; }

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

        public virtual bool Contains(Vector2 point)
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

            foreach (UIElement child in Children)
                child.Toggle();
        }

        public void Reset()
        {
            IsHovered = false;
            IsPressed = false;
            IsCaptured = false;
        }
    }
}

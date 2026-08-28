using GameFramework_SeaBedExplorationDemo.Engine.Types;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.UI.Elements.Containers
{
    public class UIEmptyContainer : UIElement
    {
        public UIEmptyContainer() : base(new(Vector2.Zero, Vector2.Zero)) { }

        public override bool Contains(Vector2 point)
        {
            foreach (UIElement child in Children)
                if (child.IsEnabled && child.Contains(point))
                    return true;
            return false;
        }
    }
}

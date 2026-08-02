using Raylib_cs;
using GameFramework_SeaBedExplorationDemo.Engine.GameObjects;
using GameFramework_SeaBedExplorationDemo.Engine.Observers.Observables;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using GameFramework_SeaBedExplorationDemo.Engine.Observers.Observables;

namespace GameFramework_SeaBedExplorationDemo.Engine.UI
{
    public class UIManager : IUpdatable, IDrawableUI
    {
        private readonly List<UIElement> elements = new();

        UIElement? HoveredElement; // element currently under mouse
        //UIElement? FocusedElement; // element that can intake keyboard inputs
        UIElement? CapturedElement; // element receiving drag/tick until release
        UIElement? PressedElement; // element currently being pressed, throws Click() upon first press then isPressed == true until release

        public UIManager()
        {
            Observers.Observers.Subscribe(this);
        }

        public UIManager(List<UIElement> elements) : this()
        {
            this.elements = elements;
        }

        public void Add(UIElement element)
        {
            elements.Add(element);
        }

        public void Update(float dt)
        {
            UIElement? hitElement = GetTopmostElementAt(Mouse.Position);

            UpdateHover(hitElement);
            UpdatePressed();
            UpdateMouseDown(dt);
            UpdateMouseRelease();
        }

        public void DrawUI()
        {
            foreach (UIElement element in elements)
                element.Draw();
        }

        UIElement? GetTopmostElementAt(Vector2 mousePos)
        {
            List<UIElement> currentList = elements;
            for (int i = 0; i < currentList.Count; i++)
            {
                if (!currentList[i].IsEnabled)
                    continue;

                if (currentList[i].Contains(mousePos))
                {
                    if (currentList[i].Children.Count == 0)
                        return currentList[i];

                    currentList = currentList[i].Children;
                }
            }
            return null;
        }
        private void UpdateHover(UIElement? hitElement)
        {
            if (HoveredElement != hitElement)
            {
                if (HoveredElement != null)
                    HoveredElement.IsHovered = false;
                HoveredElement = hitElement;
            }

            if (HoveredElement != null)
                HoveredElement.IsHovered = true;
        }
        private void UpdatePressed()
        {
            if (PressedElement != null && PressedElement != HoveredElement)
            {
                PressedElement.IsPressed = false;
                PressedElement = null;
            }

            if (HoveredElement != null && Mouse.IsBtnPressed(MouseButton.Left))
            {
                PressedElement = HoveredElement;
                PressedElement.IsPressed = true;

                CapturedElement = HoveredElement;
                CapturedElement.IsCaptured = true;
            }
        }
        private void UpdateMouseDown(float dt)
        {
            if (CapturedElement != null && Mouse.IsBtnDown(MouseButton.Left))
                CapturedElement.Tick(dt);
        }
        private void UpdateMouseRelease()
        {
            if (Mouse.IsBtnReleased(MouseButton.Left))
            {
                if (PressedElement != null)
                {
                    PressedElement.Click();
                    PressedElement.IsPressed = false;
                }

                if (CapturedElement != null)
                    CapturedElement.IsCaptured = false;


                PressedElement = null;
                CapturedElement = null;
            }
        }
    }
}
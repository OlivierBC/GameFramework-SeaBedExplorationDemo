using GameFramework_SeaBedExplorationDemo.Engine.Base;
using GameFramework_SeaBedExplorationDemo.Engine.Registries.Observables;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Inputs;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.UI
{
    public class UIManager : Element, IUpdatable, IDrawableUI, IUnloadable
    {
        private readonly List<UIElement> elements = new();

        UIElement? HoveredElement; // element currently under mouse
        UIElement? CapturedElement; // element receiving drag/tick until release
        UIElement? PressedElement; // element currently being pressed, throws Click() upon first press then isPressed == true until release

        public UIManager() { }

        public void Add(UIElement element)
        {
            elements.Add(element);
        }

        public void Update(float dt)
        {
            UIElement? hitElement = GetTopmostElementAt(MouseInputManager.Position);

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
            for (int i = elements.Count - 1; i >= 0; i--)
            {
                UIElement? hitElement = GetTopmostElementAt(elements[i], mousePos);

                if (hitElement != null)
                    return hitElement;
            }

            return null;
        }

        UIElement? GetTopmostElementAt(UIElement element, Vector2 mousePos)
        {
            if (!element.IsEnabled || !element.Contains(mousePos))
                return null;

            for (int i = element.Children.Count - 1; i >= 0; i--)
            {
                UIElement? hitElement = GetTopmostElementAt(element.Children[i], mousePos);

                if (hitElement != null)
                    return hitElement;
            }

            return element;
        }
        private void UpdateHover(UIElement? hitElement)
        {
            bool wasHittingUi = HoveredElement != null;

            if (HoveredElement != hitElement)
            {
                if (HoveredElement != null)
                    HoveredElement.IsHovered = false;
                HoveredElement = hitElement;
            }

            if (HoveredElement != null)
                HoveredElement.IsHovered = true;

            bool isHittingUi = HoveredElement != null;

            if (!wasHittingUi && isHittingUi)
                MouseInputManager.AddUiHit(this);
            else if (wasHittingUi && !isHittingUi)
                MouseInputManager.RemoveUiHit(this);
        }
        private void UpdatePressed()
        {
            if (PressedElement != null && PressedElement != HoveredElement)
            {
                PressedElement.IsPressed = false;
                PressedElement = null;
            }

            if (HoveredElement != null && MouseInputManager.IsBtnPressed(MouseButton.Left))
            {
                PressedElement = HoveredElement;
                PressedElement.IsPressed = true;

                CapturedElement = HoveredElement;
                CapturedElement.IsCaptured = true;
            }
        }
        private void UpdateMouseDown(float dt)
        {
            if (CapturedElement != null && MouseInputManager.IsBtnDown(MouseButton.Left))
                CapturedElement.Tick(dt);
        }
        private void UpdateMouseRelease()
        {
            if (MouseInputManager.IsBtnReleased(MouseButton.Left))
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

        public void Unload()
        {
            MouseInputManager.RemoveUiHit(this);

            if (HoveredElement != null)
                HoveredElement.IsHovered = false;

            if (PressedElement != null)
                PressedElement.IsPressed = false;

            if (CapturedElement != null)
                CapturedElement.IsCaptured = false;

            HoveredElement = null;
            PressedElement = null;
            CapturedElement = null;
        }
    }
}
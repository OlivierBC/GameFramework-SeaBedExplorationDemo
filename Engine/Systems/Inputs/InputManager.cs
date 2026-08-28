using GameFramework_SeaBedExplorationDemo.Engine.Registries.Observables;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Inputs
{
    public class InputManager : IUpdatable
    {
        public static InputManager MainInstance = new();

        List<InputAction> inputActions = new List<InputAction>();

        public void Add(InputAction action)
        {
            if (!inputActions.Contains(action))
                inputActions.Add(action);
        }

        public void Remove(string actionName)
        {
            string? actionNameToRemove = string.Empty;
            foreach (InputAction action in inputActions)
            {
                if (action.name == actionName)
                {
                    actionNameToRemove = action.name;
                    break;
                }
            }
            inputActions.Remove(new InputAction(actionNameToRemove));
        }

        public void Update(float deltaTime)
        {
            foreach (InputAction action in inputActions)
            {
                if (action.callbackMode == InputAction.InputCallbackMode.OnPressed && Raylib.IsKeyPressed(action.key))
                    action.action();
                else if (action.callbackMode == InputAction.InputCallbackMode.OnDown && Raylib.IsKeyDown(action.key))
                    action.action();
                else if (action.callbackMode == InputAction.InputCallbackMode.OnReleased && Raylib.IsKeyReleased(action.key))
                    action.action();
                else if (action.callbackMode == InputAction.InputCallbackMode.OnUp && Raylib.IsKeyUp(action.key))
                    action.action();
            }
        }
    }
}
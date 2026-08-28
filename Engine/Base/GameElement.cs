using GameFramework_SeaBedExplorationDemo.Engine.Types;

namespace GameFramework_SeaBedExplorationDemo.Engine.Base
{
    public class GameElement : Element
    {
        public Transform Transform = new();
        public GameElement() { }
        public GameElement(Vector3 position) : this()
        {
            Transform.Position = position;
        }
        public GameElement(Transform transform) : this()
        {
            Transform = transform;
        }
    }
}
using GameFramework_SeaBedExplorationDemo.Engine.Types;

namespace GameFramework_SeaBedExplorationDemo.Engine.Base
{
    public class GameObject
    {
        protected Vector3 position = new();
        public Vector3 Position => position;
        public GameObject() { }

        public GameObject(Vector3 position) : this()
        {
            this.position = position;
        }
    }
}
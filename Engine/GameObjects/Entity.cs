using GameFramework_SeaBedExplorationDemo.Engine.Types;

namespace GameFramework_SeaBedExplorationDemo.Engine.GameObjects
{
    public class Entity
    {
        protected Vector3 position = new();
        public Vector3 Position => position;
        public Entity()
        {
            Observers.Observers.Subscribe(this);
        }
        public Entity(Vector3 position) : this()
        {
            this.position = position;
        }
    }
}
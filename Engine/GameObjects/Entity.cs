using UiIntegration.Engine.Types;

namespace UiIntegration.Engine.GameObjects
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
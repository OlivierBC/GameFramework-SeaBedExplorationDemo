namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS
{
    public struct Entity
    {
        public long Id { get; private set; }

        public Entity(long id)
        {
            Id = id;
        }

        public static bool operator ==(Entity? a, Entity? b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null || b is null)
                return false;

            return b.Value.Id == b.Value.Id;
        }

        public static bool operator !=(Entity? a, Entity? b)
        {
            return !(a == b);
        }

        public bool Equals(Entity? other)
        {
            return this == other;
        }

        public override bool Equals(object? obj)
        {
            return obj is Entity other && this == other;
        }
    }
}

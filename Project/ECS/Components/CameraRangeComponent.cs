using GameFramework_SeaBedExplorationDemo.Engine.Systems.ECS;

public class CameraRangeComponent : IComponent
{
    public float MaxDistance { get; set; }
    public float RepositionMinDistance { get; set; }
    public float RepositionMaxDistance { get; set; }

    public CameraRangeComponent(
        float maxDistance,
        float repositionMinDistance,
        float repositionMaxDistance
    )
    {
        MaxDistance = maxDistance;
        RepositionMinDistance = repositionMinDistance;
        RepositionMaxDistance = repositionMaxDistance;
    }
}
namespace GameFramework_SeaBedExplorationDemo.Engine.Types.Interface
{
    public interface IToggleable
    {
        bool IsEnabled { get; }

        public void Toggle();
    }
}

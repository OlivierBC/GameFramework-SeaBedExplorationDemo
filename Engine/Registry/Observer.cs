// This observer pattern is used for the interfaces:
// IUpdatable ( to call Update() every frame )
// IDrawable ( to call Draw() function )
// Etc...

// This pattern implementation is simply based on what I'm used to working with: the unityEngine

namespace GameFramework_SeaBedExplorationDemo.Engine.Observers
{
    internal class Observer<T>
    {
        List<T> observables = new();

        public void Subscribe(T observable)
        {
            if (!observables.Contains(observable))
                observables.Add(observable);
        }

        public void AddRange(params T[] observables)
        {
            foreach (var observable in observables)
                Subscribe(observable);
        }

        public void UnsubscribeAll()
        {
            observables.Clear();
        }

        public void Notify(Action<T> action)
        {
            foreach (var observable in observables)
                action(observable);
        }
    }
}

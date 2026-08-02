// This observer pattern is used for the interfaces:
// IUpdatable ( to call Update() every frame )
// IDrawable ( to call Draw() function )
// Etc...

// This pattern implementation is simply based on what I'm used to working with: the unityEngine

namespace UiIntegration.Engine.Observers
{
    internal class Observer<T>
    {
        List<T> observables = new();

        public void Add(T observable)
        {
            if (!observables.Contains(observable))
                observables.Add(observable);
        }

        public void AddRange(params T[] observables)
        {
            foreach (var observable in observables)
                Add(observable);
        }

        public void Remove(T observable)
        {
            observables.Remove(observable);
        }

        public void Notify(Action<T> action)
        {
            foreach (var observable in observables)
                action(observable);
        }
    }
}

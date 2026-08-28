// This observer pattern is used for the interfaces:
// IUpdatable ( to call Update() every frame )
// IDrawable ( to call Draw() function )
// Etc...

// This pattern implementation is simply based on what I'm used to working with: the unityEngine

namespace GameFramework_SeaBedExplorationDemo.Engine.Registries
{
    internal class Observer<T>
    {
        List<T> observables = new();

        public void Subscribe(T observable)
        {
            if (!observables.Contains(observable))
                observables.Add(observable);
        }

        public void Unsubscribe(T objToUnsub)
        {
            if (observables.Contains(objToUnsub))
                observables.Remove(objToUnsub);
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

        public void NotifyOne(T obs, Action<T> action)
        {
            action(obs);
        }
    }
}

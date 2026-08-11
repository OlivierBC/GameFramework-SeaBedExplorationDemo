using GameFramework_SeaBedExplorationDemo.Engine.Observers.Observables;

namespace GameFramework_SeaBedExplorationDemo.Engine.Observers
{
    public class Registry
    {
        Observer<IUpdatable> updateObserver = new();

        Observer<IDrawable3D> drawObserver = new();
        Observer<IDrawable2D> draw2DObserver = new();
        Observer<IDrawableUI> drawUIObserver = new();

        Observer<ILoadable> loadObserver = new();
        Observer<IUnloadable> unloadObserver = new();

        public void Add(params Object[] objects)
        {
            foreach (var o in objects)
            {
                if (o is IUpdatable updatable)
                    updateObserver.Subscribe(updatable);

                if (o is IDrawable3D drawable3D)
                    drawObserver.Subscribe(drawable3D);

                if (o is IDrawable2D drawable2D)
                    draw2DObserver.Subscribe(drawable2D);

                if (o is IDrawableUI drawableUI)
                    drawUIObserver.Subscribe(drawableUI);

                if (o is ILoadable loadable)
                    loadObserver.Subscribe(loadable);

                if (o is IUnloadable unloadable)
                    unloadObserver.Subscribe(unloadable);
            }
        }

        public void Update(float dt)
        {
            updateObserver.Notify(x => x.Update(dt));
        }

        public void Draw()
        {
            drawObserver.Notify(x => x.Draw());
        }

        public void DrawUI()
        {
            drawUIObserver.Notify(x => x.DrawUI());
        }

        public void Draw2D()
        {
            draw2DObserver.Notify(x => x.Draw2D());
        }

        public void Load()
        {
            loadObserver.Notify(x => x.Load());
        }

        public void Unload()
        {
            unloadObserver.Notify(x => x.Unload());

            updateObserver.UnsubscribeAll();
            drawObserver.UnsubscribeAll();
            draw2DObserver.UnsubscribeAll();
            drawUIObserver.UnsubscribeAll();
            loadObserver.UnsubscribeAll();
            unloadObserver.UnsubscribeAll();
        }
    }
}

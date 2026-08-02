using UiIntegration.Engine.Observers.Observables;

namespace UiIntegration.Engine.Observers
{
    public static class Observers
    {
        static Observer<IUpdatable> updateObserver = new();

        static Observer<IDrawable3D> drawObserver = new();
        static Observer<IDrawable2D> draw2DObserver = new();
        static Observer<IDrawableUI> drawUIObserver = new();

        static Observer<IUnloadable> unloadObserver = new();

        static public void Subscribe(params Object[] objects)
        {
            foreach (var o in objects)
            {
                if (o is IUpdatable updatable)
                    updateObserver.Add(updatable);

                if (o is IDrawable3D drawable3D)
                    drawObserver.Add(drawable3D);

                if (o is IDrawable2D drawable2D)
                    draw2DObserver.Add(drawable2D);

                if (o is IDrawableUI drawableUI)
                    drawUIObserver.Add(drawableUI);

                if (o is IUnloadable unloadable)
                    unloadObserver.Add(unloadable);
            }
        }

        public static void Update(float dt)
        {
            updateObserver.Notify(x => x.Update(dt));
        }

        public static void Draw()
        {
            drawObserver.Notify(x => x.Draw());
        }

        public static void DrawUI()
        {
            drawUIObserver.Notify(x => x.DrawUI());
        }

        public static void Draw2D()
        {
            draw2DObserver.Notify(x => x.Draw2D());
        }

        public static void Unload()
        {
            unloadObserver.Notify(x => x.Unload());
        }
    }
}

using GameFramework_SeaBedExplorationDemo.Engine.Base;
using GameFramework_SeaBedExplorationDemo.Engine.Registries.Observables;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Resource;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene.SceneEditor
{
    public class SceneStaticGeometry : GameElement, IDrawable3D
    {
        public readonly Model Model;
        public string ModelName { get; private set; }

        public SceneStaticGeometry(string modelName)
        {
            ModelName = modelName;
            Model = ModelLibrary.GetModel(modelName);
        }

        public SceneStaticGeometry(string modelName, Types.Transform transform) : base(transform)
        {
            ModelName = modelName;
            Model = ModelLibrary.GetModel(modelName);
        }

        public void Draw()
        {
            Quaternion rotation = Transform.ToQuaternion();

            rotation.ToAxisAngle(
                out Vector3 axis,
                out float angle
            );

            float angleDegrees = angle * 180f / MathF.PI;

            Raylib.DrawModelEx(
                Model,
                Transform.Position,
                axis,
                angleDegrees,
                Vector3.One * Transform.Scale,
                Color.RayWhite
            );
        }
    }
}

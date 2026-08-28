using GameFramework_SeaBedExplorationDemo.Engine.Base;
using GameFramework_SeaBedExplorationDemo.Engine.Registries.Observables;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Inputs;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Project.Movements
{
    internal class Camera : GameElement, IUpdatable
    {
        float moveSpeed;
        float rotationSpeed;

        float yaw;
        float pitch;

        public static bool IsRotationUnlocked { get; private set; } = false;

        public static Vector3 GetPosition()
        {
            return Instance.Position;
        }

        public static Vector3 GetForward()
        {
            return Vector3.Normalize(
                Instance.Target - Instance.Position
            );
        }

        public static Camera3D Instance = new()
        {
            Position = new Vector3(0, 6, 0),
            Target = new Vector3(0, 6, 1),
            Up = Vector3.UnitY,
            FovY = 45,
            Projection = CameraProjection.Perspective
        };

        public Camera(float moveSpeed = 10f, float rotationSpeed = 0.003f)
        {
            this.moveSpeed = moveSpeed;
            this.rotationSpeed = rotationSpeed;

            Transform.Position = Instance.Position;
        }

        public void Update(float dt)
        {
            CameraRotation();

            Vector3 forward = CalculateForward();

            Vector3 right = Vector3.Normalize(Vector3.Cross(forward, Vector3.UnitY));

            Vector3 flatForward = new(forward.X, 0, forward.Z);

            if (flatForward.LengthSquared() > 0)
                flatForward = Vector3.Normalize(flatForward);

            Vector2 m = Movement();

            if (m.LengthSquared() > 0)
                m = Vector2.Normalize(m);

            Transform.Position += moveSpeed * dt * ((flatForward * m.Y) - (right * m.X));

            if (Raylib.IsKeyDown(KeyboardKey.Space))
                Transform.Position += Vector3.UnitY * moveSpeed * dt;

            if (Raylib.IsKeyDown(KeyboardKey.LeftShift))
                Transform.Position -= Vector3.UnitY * moveSpeed * dt;

            Instance.Position = Transform.Position;
            Instance.Target = Transform.Position + forward;
            Instance.Up = Vector3.UnitY;
        }

        Vector2 Movement()
        {
            Vector2 m = new();
            m.X += Raylib.IsKeyDown(KeyboardKey.A);
            m.X -= Raylib.IsKeyDown(KeyboardKey.D);
            m.Y += Raylib.IsKeyDown(KeyboardKey.W);
            m.Y -= Raylib.IsKeyDown(KeyboardKey.S);
            return m;
        }

        void CameraRotation()
        {
            SetCursorState();

            if (!CursorManager.IsLockedBy(this))
                return;

            Vector2 mouseDelta = MouseInputManager.Delta;

            yaw -= mouseDelta.X * rotationSpeed;
            pitch -= mouseDelta.Y * rotationSpeed;

            float limit = MathF.PI / 2f - 0.01f;

            pitch = Math.Clamp(pitch, -limit, limit);
        }

        void SetCursorState()
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Right))
                CursorManager.TryLock(this);

            if (Raylib.IsMouseButtonReleased(MouseButton.Right))
                CursorManager.Unlock(this);

            IsRotationUnlocked =
                CursorManager.IsLockedBy(this);
        }

        Vector3 CalculateForward()
        {
            return Vector3.Normalize(
                new Vector3(
                    MathF.Sin(yaw) * MathF.Cos(pitch),
                    MathF.Sin(pitch),
                    MathF.Cos(yaw) * MathF.Cos(pitch)
                )
            );
        }
    }
}
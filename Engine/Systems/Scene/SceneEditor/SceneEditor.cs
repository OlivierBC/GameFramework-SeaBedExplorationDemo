using GameFramework_SeaBedExplorationDemo.Engine.Base;
using GameFramework_SeaBedExplorationDemo.Engine.Registries.Observables;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Inputs;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.Resource;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.UI.Elements.Containers;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.UI.Elements.Interactable;
using GameFramework_SeaBedExplorationDemo.Engine.Systems.UI.Structure;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using GameFramework_SeaBedExplorationDemo.Engine.Types.Interface;
using GameFramework_SeaBedExplorationDemo.Project.Movements;
using Raylib_cs;
using Matrix4x4 = System.Numerics.Matrix4x4;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene.SceneEditor
{
    public class SceneEditor : Element, IUpdatable, IDrawable3D, IUnloadable, IToggleable
    {
        public bool IsEnabled { get; private set; }
        public EditorMode editorMode = EditorMode.Create;

        public GizmoMode gizmoMode = GizmoMode.Translation;

        public UIEmptyContainer UIContainer { get; private set; }
        public UIEmptyContainer UIContainerCreateMenu { get; private set; }
        public UIEmptyContainer UIContainerGizmoModeMenu { get; private set; }

        public string? selectedModelNameToCreate;
        public SceneStaticGeometry? selectedStaticGeometry;

        readonly List<SceneStaticGeometry> AddedObjects = new();
        readonly List<SceneStaticGeometry> RemovedObjects = new();
        readonly List<SceneStaticGeometry> baseStaticGeometrys;
        readonly List<SceneStaticGeometry> bakedStaticGeometrys = new();

        static readonly string gizmoArrowModelName = "gizmo_arrow";
        static readonly string gizmoRotateModelName = "gizmo_rotate";

        static readonly (Vector3 axis, Vector3 rotation, Color color)[] gizmoDirections =
        {
            (Vector3.UnitX, Vector3.Zero, Color.Red),
            (Vector3.UnitY, new Vector3(0f, 0f, 90f), Color.Green),
            (Vector3.UnitZ, new Vector3(0f, -90f, 0f), Color.Blue)
        };

        Model gizmoArrowModel;
        Model gizmoRotateModel;
        Model gizmoSphereModel;

        float gizmoDistanceScale = 0.08f;

        bool isDragging;
        int draggedGizmoIndex = -1;

        Vector3 dragAxis;
        RayPlane? dragPlane;

        Vector3 dragStartHit;
        Vector3 dragStartPosition;

        Vector2 rotationDragScreenDirection;
        Quaternion dragObjectRotation;

        float rotationSpeed = 0.15f;

        public SceneEditor(List<SceneStaticGeometry> baseStaticGeometrys)
        {
            this.baseStaticGeometrys = baseStaticGeometrys;

            gizmoArrowModel = ModelLibrary.GetModel(gizmoArrowModelName);
            gizmoRotateModel = ModelLibrary.GetModel(gizmoRotateModelName);
            gizmoSphereModel = Raylib.LoadModelFromMesh(Raylib.GenMeshSphere(.1f, 8, 8));

            RefreshBakedStaticGeometrys();

            // UI init
            UIContainer = new();
            UIContainerCreateMenu = new();
            UIContainerGizmoModeMenu = new();
            float h = 40, gap = h * .1f, currentH = h + gap, w = 100, currentW = gap;

            foreach (var name in Enum.GetNames(typeof(EditorMode)))
            {
                UIContainer.Children.Add(new UIButton(new(currentW, gap), new(currentW + w, gap + h), name, () => SetEditorMode((EditorMode)Enum.GetNames(typeof(EditorMode)).IndexOf<string>(name))));
                currentW += w + gap;
            }

            currentH += gap * 2;
            float subMenuStartH = currentH;

            foreach (string s in SceneFileManager.GetStaticGeometryModelNames())
            {
                UIContainerCreateMenu.Children.Add(new UIButton(new(gap, currentH), new(gap + w, currentH + h), s, () => selectedModelNameToCreate = "ssg_" + s, TextAlign.Left));
                currentH += h + gap;
            }

            currentH = subMenuStartH;

            foreach (var name in Enum.GetNames(typeof(GizmoMode)))
            {
                UIContainerGizmoModeMenu.Children.Add(new UIButton(new(gap, currentH), new(gap + w, currentH + h), name, () => SetGizmoMode((GizmoMode)Enum.GetNames(typeof(GizmoMode)).IndexOf<string>(name))));
                currentH += h + gap;
            }

            UIContainer.Children.Add(UIContainerCreateMenu);
            UIContainer.Children.Add(UIContainerGizmoModeMenu);
        }

        public void AddObject(SceneStaticGeometry staticGeometry)
        {
            if (AddedObjects.Contains(staticGeometry))
                return;

            AddedObjects.Add(staticGeometry);

            RefreshBakedStaticGeometrys();
        }

        public void RemoveSelectedObject(SceneStaticGeometry staticGeometry)
        {
            if (staticGeometry == null)
                return;

            if (AddedObjects.Contains(staticGeometry))
            {
                AddedObjects.Remove(staticGeometry);
            }
            else if (baseStaticGeometrys.Contains(staticGeometry) && !RemovedObjects.Contains(staticGeometry))
            {
                RemovedObjects.Add(staticGeometry);
            }

            EndDrag();

            RefreshBakedStaticGeometrys();
        }

        private void RefreshBakedStaticGeometrys()
        {
            bakedStaticGeometrys.Clear();

            foreach (SceneStaticGeometry staticGeometry in baseStaticGeometrys)
            {
                if (!RemovedObjects.Contains(staticGeometry))
                    bakedStaticGeometrys.Add(staticGeometry);
            }

            bakedStaticGeometrys.AddRange(AddedObjects);
        }

        public void BakeSceneFile(string sceneFileName)
        {
            SceneFileManager.SaveSceneElements(sceneFileName, bakedStaticGeometrys);
        }

        public List<SceneStaticGeometry> GetBakedStaticGeometryList()
        {
            return new(bakedStaticGeometrys);
        }

        public void Update(float deltaTime)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.E))
                Toggle();

            if (!IsEnabled)
                return;

            if (!isDragging)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.T))
                    SetGizmoMode(GizmoMode.Translation);

                if (Raylib.IsKeyPressed(KeyboardKey.R))
                    SetGizmoMode(GizmoMode.Rotation);

                if (Raylib.IsKeyPressed(KeyboardKey.Z))
                    SetEditorMode(EditorMode.Create);

                if (Raylib.IsKeyPressed(KeyboardKey.X))
                    SetEditorMode(EditorMode.Delete);

                if (Raylib.IsKeyPressed(KeyboardKey.C))
                    SetEditorMode(EditorMode.Modify);
            }

            Ray ray = Raylib.GetScreenToWorldRay(MouseInputManager.Position, Camera.Instance);

            if (isDragging)
            {
                if (Raylib.IsMouseButtonDown(MouseButton.Left))
                {
                    if (gizmoMode == GizmoMode.Translation)
                        UpdateTranslationDrag(ray);
                    else
                        UpdateRotationDrag();
                }

                if (Raylib.IsMouseButtonReleased(MouseButton.Left))
                    EndDrag();
            }
            else if (!MouseInputManager.IsHittingUi && MouseInputManager.IsBtnPressed(MouseButton.Left))
            {
                if (!TryBeginGizmoDrag(ray))
                {
                    SceneStaticGeometry? closestObject = SelectStaticGeometry(ray, out RayCollision collision);

                    if (closestObject != null)
                    {
                        if (editorMode == EditorMode.Create && selectedModelNameToCreate != null)
                        {
                            SceneStaticGeometry newObj = new SceneStaticGeometry(
                                selectedModelNameToCreate,
                                new Types.Transform(collision.Point, Vector3.Zero, 1f)
                            );

                            AddObject(newObj);

                            selectedStaticGeometry = newObj;
                        }
                        else if (editorMode == EditorMode.Delete)
                        {
                            RemoveSelectedObject(closestObject);

                            selectedStaticGeometry = null;
                        }
                        else if (editorMode == EditorMode.Modify)
                        {
                            selectedStaticGeometry = closestObject;
                        }
                    }
                }
            }
        }

        private bool TryBeginGizmoDrag(Ray ray)
        {
            if (selectedStaticGeometry == null)
                return false;

            if (gizmoMode == GizmoMode.Translation)
            {
                if (!TryGetGizmoHit(ray, gizmoArrowModel, out int gizmoIndex))
                    return false;

                BeginTranslationDrag(ray, gizmoIndex);

                return true;
            }

            if (!TryGetGizmoHit(ray, gizmoRotateModel, out int rotationIndex))
                return false;

            return BeginRotationDrag(ray, rotationIndex);
        }

        private void BeginTranslationDrag(Ray ray, int gizmoIndex)
        {
            if (selectedStaticGeometry == null)
                return;

            draggedGizmoIndex = gizmoIndex;

            dragAxis = GetWorldGizmoAxis(gizmoDirections[gizmoIndex].axis);

            Vector3 planeNormal = GetTranslationDragPlaneNormal(dragAxis);
            dragPlane = new RayPlane(selectedStaticGeometry.Transform.Position, planeNormal);

            if (!dragPlane.Raycast(ray, out dragStartHit))
                return;

            dragStartPosition = selectedStaticGeometry.Transform.Position;

            isDragging = true;
        }

        private void UpdateTranslationDrag(Ray ray)
        {
            if (selectedStaticGeometry == null || dragPlane == null)
                return;

            if (!dragPlane.Raycast(ray, out Vector3 currentHit))
                return;

            Vector3 delta = currentHit - dragStartHit;

            float movement = Vector3.Dot(delta, dragAxis);

            selectedStaticGeometry.Transform.Position = dragStartPosition + dragAxis * movement;
        }

        private bool BeginRotationDrag(Ray ray, int gizmoIndex)
        {
            if (selectedStaticGeometry == null)
                return false;

            Vector3 axis = GetWorldGizmoAxis(gizmoDirections[gizmoIndex].axis);
            Vector3 center = selectedStaticGeometry.Transform.Position;

            RayPlane rotationPlane = new(center, axis);

            if (!rotationPlane.Raycast(ray, out Vector3 hit))
                return false;

            Vector3 radial = hit - center;

            if (radial.LengthSquared() < 0.0001f)
                return false;

            radial = Vector3.Normalize(radial);

            Vector3 tangent = Vector3.Cross(axis, radial);

            if (tangent.LengthSquared() < 0.0001f)
                return false;

            tangent = Vector3.Normalize(tangent);

            Vector2 hitScreen = Raylib.GetWorldToScreen(hit, Camera.Instance);
            Vector2 tangentScreenPoint = Raylib.GetWorldToScreen(hit + tangent, Camera.Instance);
            Vector2 screenDirection = tangentScreenPoint - hitScreen;

            if (screenDirection.LengthSquared() < 0.0001f)
                return false;

            if (!CursorManager.TryLock(this))
                return false;

            draggedGizmoIndex = gizmoIndex;
            dragAxis = axis;
            dragObjectRotation = selectedStaticGeometry.Transform.ToQuaternion();
            rotationDragScreenDirection = Vector2.Normalize(screenDirection);

            isDragging = true;

            return true;
        }

        private void UpdateRotationDrag()
        {
            if (selectedStaticGeometry == null)
                return;

            Vector2 mouseDelta = MouseInputManager.Delta;

            float movement = Vector2.Dot(mouseDelta, rotationDragScreenDirection);
            float angle = movement * rotationSpeed;

            if (MathF.Abs(angle) < 0.0001f)
                return;

            Quaternion deltaRotation = Quaternion.CreateFromAxisAngle(dragAxis, angle * MathF.PI / 180f);

            dragObjectRotation = Quaternion.Concatenate(dragObjectRotation, deltaRotation).Normalized();

            selectedStaticGeometry.Transform.Rotation = dragObjectRotation.ToEulerDegrees();
        }

        private void EndDrag()
        {
            if (CursorManager.IsLockedBy(this))
                CursorManager.Unlock(this);

            isDragging = false;
            draggedGizmoIndex = -1;
            dragPlane = null;
        }

        private Vector3 GetTranslationDragPlaneNormal(Vector3 axis)
        {
            Vector3 cameraForward = Camera.Instance.Target - Camera.Instance.Position;

            if (cameraForward.LengthSquared() > 0.0001f)
                cameraForward = Vector3.Normalize(cameraForward);

            Vector3 normal = cameraForward - axis * Vector3.Dot(cameraForward, axis);

            if (normal.LengthSquared() < 0.0001f)
            {
                Vector3 cameraUp = Camera.Instance.Up;

                normal = cameraUp - axis * Vector3.Dot(cameraUp, axis);
            }

            if (normal.LengthSquared() < 0.0001f)
            {
                Vector3 fallback = MathF.Abs(Vector3.Dot(axis, Vector3.UnitY)) < 0.99f
                    ? Vector3.UnitY : Vector3.UnitX;

                normal = fallback - axis * Vector3.Dot(fallback, axis);
            }

            return Vector3.Normalize(normal);
        }

        private Vector3 GetWorldGizmoAxis(Vector3 localAxis)
        {
            if (selectedStaticGeometry == null)
                return localAxis;

            Quaternion rotation = selectedStaticGeometry.Transform.ToQuaternion();

            Vector3 axis = System.Numerics.Vector3.Transform(localAxis, rotation.AsSystemNumerics);

            if (axis.LengthSquared() < 0.0001f)
                return localAxis;

            return Vector3.Normalize(axis);
        }

        private SceneStaticGeometry? SelectStaticGeometry(Ray ray, out RayCollision closestCollision)
        {
            if (!Raycast.TryStaticGeometrys(ray, bakedStaticGeometrys, out SceneStaticGeometry? closestObject, out closestCollision))
                return null;

            return closestObject;
        }

        private bool TryGetGizmoHit(Ray ray, Model model, out int gizmoIndex)
        {
            gizmoIndex = -1;

            if (selectedStaticGeometry == null)
                return false;

            float closestDistance = float.MaxValue;

            for (int gizmo = 0; gizmo < gizmoDirections.Length; gizmo++)
            {
                Matrix4x4 matrix = GetGizmoMatrix(gizmo);

                if (!Raycast.TryModel(ray, model, matrix, out RayCollision collision))
                    continue;

                if (collision.Distance >= closestDistance)
                    continue;

                closestDistance = collision.Distance;
                gizmoIndex = gizmo;
            }

            return gizmoIndex != -1;
        }

        private Quaternion GetGizmoRotation(int gizmoIndex)
        {
            var gizmo = gizmoDirections[gizmoIndex];

            Quaternion modelRotation = Quaternion.Identity;

            if (gizmoMode == GizmoMode.Rotation)
            {
                float angle = gizmoIndex == 2
                    ? 360f : (gizmoIndex + 1) * 90f;

                modelRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, angle * MathF.PI / 180f);
            }

            Quaternion directionRotation = Quaternion.CreateFromEulerDegrees(gizmo.rotation);
            Quaternion gizmoRotation = Quaternion.Concatenate(modelRotation, directionRotation);

            if (selectedStaticGeometry == null)
                return gizmoRotation;

            return Quaternion.Concatenate(gizmoRotation, selectedStaticGeometry.Transform.ToQuaternion());
        }

        private float GetGizmoScale(Vector3 position)
        {
            float distance = Vector3.Distance(Camera.Instance.Position, position);

            return distance * gizmoDistanceScale;
        }

        private Matrix4x4 GetGizmoMatrix(int gizmoIndex)
        {
            if (selectedStaticGeometry == null)
                return Matrix4x4.Identity;

            Quaternion rotation = GetGizmoRotation(gizmoIndex);
            float scale = GetGizmoScale(selectedStaticGeometry.Transform.Position);

            Matrix4x4 matrix =
                Matrix4x4.CreateScale(scale)
                    * Matrix4x4.CreateFromQuaternion(rotation.AsSystemNumerics)
                    * Matrix4x4.CreateTranslation(selectedStaticGeometry.Transform.Position);

            return Matrix4x4.Transpose(matrix);
        }

        private void SetEditorMode(EditorMode newMode)
        {
            if ((newMode == EditorMode.Create && !UIContainerCreateMenu.IsEnabled) || (newMode != EditorMode.Create && UIContainerCreateMenu.IsEnabled))
                UIContainerCreateMenu.Toggle();
            if ((newMode == EditorMode.Modify && !UIContainerGizmoModeMenu.IsEnabled) || (newMode != EditorMode.Modify && UIContainerGizmoModeMenu.IsEnabled))
                UIContainerGizmoModeMenu.Toggle();

            editorMode = newMode;
        }

        private void SetGizmoMode(GizmoMode newMode)
        {
            gizmoMode = newMode;
        }

        public void Draw()
        {
            // TODO Currently staticGeometrys are rendered by the sceneEditor, so if this foreach is after the IsEnabled check then not even the objects loaded from the scene .json files are rendered...
            foreach (SceneStaticGeometry staticGeometry in bakedStaticGeometrys)
                staticGeometry.Draw();

            if (!IsEnabled) return;

            if (editorMode == EditorMode.Modify && selectedStaticGeometry != null)
                DrawEditGizmo(selectedStaticGeometry.Transform.Position);
        }

        public void DrawEditGizmo(Vector3 position)
        {
            Rlgl.DisableDepthTest();

            float scale = GetGizmoScale(position);

            Model model = gizmoMode == GizmoMode.Translation
                ? gizmoArrowModel : gizmoRotateModel;

            for (int i = 0; i < gizmoDirections.Length; i++)
            {
                if (isDragging && i != draggedGizmoIndex)
                    continue;

                var gizmo = gizmoDirections[i];

                Quaternion rotation = GetGizmoRotation(i);

                rotation.ToAxisAngle(out Vector3 axis, out float angle);

                Raylib.DrawModelEx(
                    model,
                    position,
                    axis,
                    angle * 180f / MathF.PI,
                    Vector3.One * scale,
                    gizmo.color
                );
            }

            if (!isDragging)
                Raylib.DrawModelEx(gizmoSphereModel, position, Vector3.UnitY, 0f, Vector3.One * scale, Color.Gray);

            Rlgl.EnableDepthTest();
        }

        public void Unload()
        {
            CursorManager.Unlock(this);

            Raylib.UnloadModel(gizmoSphereModel);
        }

        public void Toggle()
        {
            IsEnabled = !IsEnabled;

            if (UIContainer.IsEnabled != IsEnabled)
                UIContainer.Toggle();

            SetEditorMode(editorMode);
        }

        public enum GizmoMode
        {
            Translation,
            Rotation
        }
        public enum EditorMode
        {
            Create,
            Delete,
            Modify
        }
    }
}
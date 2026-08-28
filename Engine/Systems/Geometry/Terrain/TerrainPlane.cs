using GameFramework_SeaBedExplorationDemo.Engine.Base;
using GameFramework_SeaBedExplorationDemo.Engine.Registries.Observables;
using GameFramework_SeaBedExplorationDemo.Engine.Types;
using GameFramework_SeaBedExplorationDemo.resources.shaders;
using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Geometry.Terrain
{
    /// <summary>
    /// This class was AI generated for testing purposes and will deleted later on for a simpler solution that I can implement by myself.
    /// </summary>
    public class TerrainPlane :
        GameElement,
        IDrawable3D,
        IUnloadable
    {
        Mesh mesh;
        Material mat;

        public TerrainPlane()
        {
            mesh = Raylib.GenMeshPlane(
                10000,
                10000,
                50,
                50
            );

            mat = Raylib.LoadMaterialDefault();

            ApplyDisplacement(ref mesh);
            RecalculateNormals(ref mesh);

            unsafe
            {
                Raylib.UpdateMeshBuffer(
                    mesh,
                    0,
                    mesh.Vertices,
                    mesh.VertexCount * 3 * sizeof(float),
                    0
                );

                Raylib.UpdateMeshBuffer(
                    mesh,
                    2,
                    mesh.Normals,
                    mesh.VertexCount * 3 * sizeof(float),
                    0
                );

                mat.Maps[0].Color = Color.Beige;
            }

            mat.Shader = TerrainShader.Shader;

            Console.WriteLine(
                $"Material shader ID: {mat.Shader.Id}"
            );
        }
        public void Draw()
        {
            Raylib.DrawMesh(
                mesh,
                mat,
                System.Numerics.Matrix4x4.Identity
            );
        }

        public void Unload()
        {
            Raylib.UnloadMesh(mesh);
            Raylib.UnloadMaterial(mat);
        }

        private static unsafe void ApplyDisplacement(
            ref Mesh mesh
        )
        {
            for (int i = 0; i < mesh.VertexCount; i++)
            {
                int index = i * 3;

                float x =
                    mesh.Vertices[index];

                float z =
                    mesh.Vertices[index + 2];

                mesh.Vertices[index + 1] =
                    GetVertexHeight(x, z);
            }
        }

        public static float GetVertexHeight(
            float x,
            float z
        )
        {
            float height = 0;

            height +=
                MathF.Sin(x * 0.008f) *
                12f;

            height +=
                MathF.Cos(z * 0.006f) *
                10f;

            height +=
                MathF.Sin(
                    (x + z) * 0.012f
                ) *
                6f;

            height +=
                MathF.Sin(x * 0.025f) *
                MathF.Cos(z * 0.02f) *
                3f;

            return height;
        }

        private static unsafe void RecalculateNormals(
            ref Mesh mesh
        )
        {
            float sampleDistance = 1f;

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                int index = i * 3;

                float x =
                    mesh.Vertices[index];

                float z =
                    mesh.Vertices[index + 2];

                float left =
                    GetVertexHeight(
                        x - sampleDistance,
                        z
                    );

                float right =
                    GetVertexHeight(
                        x + sampleDistance,
                        z
                    );

                float back =
                    GetVertexHeight(
                        x,
                        z - sampleDistance
                    );

                float front =
                    GetVertexHeight(
                        x,
                        z + sampleDistance
                    );

                Vector3 normal =
                    Vector3.Normalize(
                        new Vector3(
                            left - right,
                            sampleDistance * 2f,
                            back - front
                        )
                    );

                mesh.Normals[index] =
                    normal.X;

                mesh.Normals[index + 1] =
                    normal.Y;

                mesh.Normals[index + 2] =
                    normal.Z;
            }
        }
    }
}
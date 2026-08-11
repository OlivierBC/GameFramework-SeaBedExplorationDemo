using Raylib_cs;
using System.Numerics;

namespace GameFramework_SeaBedExplorationDemo.resources.shaders
{
    /// <summary>
    /// This class was AI generated for testing purposes and will deleted later on for a simpler solution that I can implement by myself.
    /// This is also the case for the frag and vert shaders for terrain
    /// </summary>
    public static class TerrainShader
    {
        private static Shader shader;

        private static int lightDirLoc;
        private static int ambientLoc;
        private static int shadowStrengthLoc;
        private static int terrainColorLoc;

        public static Shader Shader => shader;

        public static void Load()
        {
            shader = Raylib.LoadShader(
                "resources/shaders/terrain.vs",
                "resources/shaders/terrain.fs"
            );

            lightDirLoc = Raylib.GetShaderLocation(
                shader,
                "lightDir"
            );

            ambientLoc = Raylib.GetShaderLocation(
                shader,
                "ambient"
            );

            shadowStrengthLoc = Raylib.GetShaderLocation(
                shader,
                "shadowStrength"
            );

            terrainColorLoc = Raylib.GetShaderLocation(
                shader,
                "terrainColor"
            );

            SetDefaults();
        }

        public static void SetLightDirection(Vector3 direction)
        {
            SetVec3(
                lightDirLoc,
                Vector3.Normalize(direction)
            );
        }

        public static void SetAmbient(float ambient)
        {
            SetFloat(ambientLoc, ambient);
        }

        public static void SetShadowStrength(float strength)
        {
            SetFloat(shadowStrengthLoc, strength);
        }

        public static void Unload()
        {
            Raylib.UnloadShader(shader);
        }

        private static unsafe void SetFloat(
            int location,
            float value
        )
        {
            Raylib.SetShaderValue(
                shader,
                location,
                &value,
                ShaderUniformDataType.Float
            );
        }

        private static unsafe void SetVec3(
            int location,
            Vector3 value
        )
        {
            Raylib.SetShaderValue(
                shader,
                location,
                &value,
                ShaderUniformDataType.Vec3
            );
        }

        private static void SetDefaults()
        {
            SetVec3(
                lightDirLoc,
                Vector3.Normalize(
                    new Vector3(
                        0.4f,
                        1.0f,
                        0.25f
                    )
                )
            );

            SetFloat(
                ambientLoc,
                0.25f
            );

            SetFloat(
                shadowStrengthLoc,
                3.0f
            );

            SetVec3(
                terrainColorLoc,
                new Vector3(
                    Color.Beige.R / 255f,
                    Color.Beige.G / 255f,
                    Color.Beige.B / 255f
                )
            );
        }
    }
}
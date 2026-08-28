using System.Text.Json;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Scene.SceneEditor
{
    public static class SceneFileManager
    {
        private static readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IncludeFields = true
        };

        public static List<SceneStaticGeometry> LoadSceneElements(string sceneFileName)
        {
            string path = GetScenePath(sceneFileName);

            if (!File.Exists(path))
                return new List<SceneStaticGeometry>();

            string json = File.ReadAllText(path);

            List<StaticGeometryData> data =
                JsonSerializer.Deserialize<List<StaticGeometryData>>(json, options)
                ?? new List<StaticGeometryData>();

            List<SceneStaticGeometry> elements = new();

            foreach (StaticGeometryData staticGeometry in data)
            {
                elements.Add(
                    new SceneStaticGeometry(
                        staticGeometry.ModelName,
                        staticGeometry.Transform
                    )
                );
            }

            return elements;
        }

        public static void SaveSceneElements(string sceneFileName, IEnumerable<SceneStaticGeometry> elements)
        {
            string path = GetScenePath(sceneFileName);

            string? directory = Path.GetDirectoryName(path);

            if (directory != null)
                Directory.CreateDirectory(directory);

            List<StaticGeometryData> data = new();

            foreach (SceneStaticGeometry element in elements)
            {
                data.Add(new StaticGeometryData
                {
                    ModelName = element.ModelName,
                    Transform = element.Transform
                });
            }

            string json = JsonSerializer.Serialize(data, options);

            File.WriteAllText(path, json);
        }

        public static List<string> GetStaticGeometryModelNames()
        {
            string modelsPath = Path.Combine(AppContext.BaseDirectory, "resources", "models");

            if (!Directory.Exists(modelsPath))
                return new List<string>();

            return Directory.GetFiles(modelsPath, "ssg_*.glb")
                .Select(Path.GetFileNameWithoutExtension)
                .Select(name => name![4..])
                .ToList();
        }

        public static bool SceneFileExists(string sceneFileName)
        {
            return File.Exists(GetScenePath(sceneFileName));
        }

        public static void DeleteSceneFile(string sceneFileName)
        {
            string path = GetScenePath(sceneFileName);

            if (File.Exists(path))
                File.Delete(path);
        }

        private static string GetScenePath(string sceneFileName)
        {
            if (!sceneFileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                sceneFileName += ".json";

            return Path.Combine(
                AppContext.BaseDirectory,
                "resources",
                "scenes",
                sceneFileName
            );
        }
    }
}
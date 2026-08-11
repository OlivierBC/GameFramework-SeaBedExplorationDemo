using Raylib_cs;

namespace GameFramework_SeaBedExplorationDemo.Engine.Systems.Resource
{
    public static class ModelLibrary
    {
        static Dictionary<string, Model> Models = new();

        public static void LoadModel(string modelName, out Model model)
        {
            model = Raylib.LoadModel("resources/models/" + modelName + ".glb");
            Models.Add(modelName, model);
        }

        public static void UnloadModel(string modelName)
        {
            if (Models.TryGetValue(modelName, out Model model))
            {
                Raylib.UnloadModel(model);
                Models.Remove(modelName);
            }
        }

        public static Model GetModel(string modelName)
        {
            Model model;
            if (Models.TryGetValue(modelName, out model))
                return model;
            else
            {
                LoadModel(modelName, out model);
                return model;
            }
        }
    }
}

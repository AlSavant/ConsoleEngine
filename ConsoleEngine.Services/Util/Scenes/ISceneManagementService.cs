using DataModel.Rendering;

namespace ConsoleEngine.Services.Util.Scenes
{
    public interface ISceneManagementService : IService
    {
        void LoadScene(Scene scene);
        void UnloadScene(Scene scene);
        void UnloadAllScenes();
        Scene GetActiveScene();
    }
}

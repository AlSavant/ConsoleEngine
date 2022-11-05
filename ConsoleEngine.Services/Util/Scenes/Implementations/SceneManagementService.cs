using DataModel.Rendering;
using System.Collections.Generic;

namespace ConsoleEngine.Services.Util.Scenes.Implementations
{
    internal sealed class SceneManagementService : ISceneManagementService
    {
        private Scene activeScene;

        public readonly List<Scene> scenes;

        public SceneManagementService()
        {
            scenes = new List<Scene>();
        }

        public Scene GetActiveScene()
        {
            return activeScene;
        }

        public void LoadScene(Scene scene)
        {
            if (scene == null)
                return;
            scenes.Add(scene);
            activeScene = scene;            
        }

        public void UnloadAllScenes()
        {
            activeScene = null;
            scenes.Clear();
        }

        public void UnloadScene(Scene scene)
        {
            scenes.Remove(scene);
            if(activeScene == scene)
            {
                if(scenes.Count > 0)
                {
                    activeScene = scenes[scenes.Count - 1];
                }
                else
                {
                    activeScene = null;
                }
            }
        }

        public void ActivateScene(Scene scene)
        {
            if(!scenes.Contains(scene))
            {
                LoadScene(scene);
            }
            else
            {
                activeScene = scene;
            }
        }
    }
}

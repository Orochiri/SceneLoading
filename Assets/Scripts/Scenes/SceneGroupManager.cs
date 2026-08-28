using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace System.SceneManagement
{
    public class SceneGroupManager
    {
        public event Action<string> OnSceneLoaded = delegate { };
        public event Action<string> OnSceneUnloaded = delegate { };
        public event Action OnSceneGroupLoaded = delegate { };

        private SceneGroup ActiveSceneGroup;

        public async Task LoadScenes(SceneGroup group, IProgress<float> progress, bool reloadDupScene = false)
        {
            ActiveSceneGroup = group;
            var loadedScenes = new List<string>();

            await UnLoadScenes();

            int sceneCount = SceneManager.sceneCount;

            for (int i = 0; i < sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i).name);
            }

            var totalScenesToLoad = ActiveSceneGroup.Scenes.Count;

            var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

            for (int i = 0; i < totalScenesToLoad; i++)
            {
                var sceneData = group.Scenes[i];
                if (reloadDupScene == false && loadedScenes.Contains(sceneData.Name)) continue;

                var operation = SceneManager.LoadSceneAsync(sceneData.Reference.Path, LoadSceneMode.Additive);
                
                operationGroup.Operations.Add(operation);
                
                OnSceneLoaded.Invoke(sceneData.Name);
            }
            
            // Wait until all Scenes in the AsyncOperations are loaded
            while (!operationGroup.IsDone)
            {
                progress?.Report(operationGroup.Progress);
                await Task.Delay(100);
            }

            Scene activeScene = SceneManager.GetSceneByName(ActiveSceneGroup.FindSceneNameByType(SceneType.ActiveScene));

            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene);
            }
            
            OnSceneGroupLoaded.Invoke();
        }

        public async Task UnLoadScenes()
        {
            var scenes = new List<string>();
            var activeScene = SceneManager.GetActiveScene().name;

            int sceneCount = SceneManager.sceneCount;

            for (int i = sceneCount - 1; i > 0; i--)
            {
                var sceneAt = SceneManager.GetSceneAt(i);
                if (!sceneAt.isLoaded) continue;

                var sceneName = sceneAt.name;
                if (sceneName.Equals(activeScene) || sceneName == "Bootstrapper") continue;
                scenes.Add(sceneName);
            }
            
            // Create AsyncOperationGroup
            var operationGroup = new AsyncOperationGroup(scenes.Count);

            foreach (var scene in scenes)
            {
                var operation = SceneManager.UnloadSceneAsync(scene);
                if (operation == null) continue;
                
                operationGroup.Operations.Add(operation);
                
                OnSceneUnloaded.Invoke(scene);
            }
            
            // Wait for all AsynOperations in the group to be done
            while (!operationGroup.IsDone)
            {
                await Task.Delay(100);
            }

            //this is optional, it is expensive and can mess with future assets. But it cleans the memory :)
            await UnityEngine.Resources.UnloadUnusedAssets();
        }
        

        public readonly struct AsyncOperationGroup
        {
            public readonly List<AsyncOperation> Operations;

            public float Progress => Operations.Count == 0 ? 0 : Operations.Average(o => o.progress);
            public bool IsDone => Operations.All(o => o.isDone);

            public AsyncOperationGroup(int initialCapacity)
            {
                Operations = new List<AsyncOperation>(initialCapacity);
            }
        }
        
    }
    
}

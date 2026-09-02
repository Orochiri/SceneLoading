using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace System.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Image loadingBar;
        [SerializeField] private float fillSpeed = 0.5f;
        [SerializeField] private Canvas loadingCanvas;
        [SerializeField] private Camera loadingCamera;
        [SerializeField] private SceneGroup[] sceneGroups;

        private float targetProgress;
        private bool isLoading;

        public readonly SceneGroupManager manager = new SceneGroupManager();
        
        public static SceneLoader Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            manager.OnSceneLoaded += sceneName => Debug.Log("Loaded: " + sceneName);
            manager.OnSceneUnloaded += sceneName => Debug.Log("Unloaded: " + sceneName);
            manager.OnSceneGroupLoaded += () => Debug.Log("Scene group loaded");
        }

        private async void Start()
        {
            await LoadSceneGroup(0, true);
        }

        private void Update()
        {
            if (!isLoading) return;

            float currentFillAmount = loadingBar.fillAmount;
            float progressDifference = Mathf.Abs(currentFillAmount - targetProgress);

            float dynamicFillSpeed = progressDifference * fillSpeed;

            loadingBar.fillAmount = Mathf.Lerp(currentFillAmount, targetProgress, Time.deltaTime * dynamicFillSpeed);
            
        }
        
        public async void LoadScene(int startScene) 
        {
            await LoadSceneGroup(startScene, true);
        }

        public async Task LoadSceneGroup(int index, bool loadingScreen)
        {
            loadingBar.fillAmount = 0f;
            targetProgress = 1f;

            if (index < 0 || index >= sceneGroups.Length)
            {
                Debug.LogError("Invalid scene group index: " + index);
                return;
            }

            LoadingProgress progress = new LoadingProgress();
            progress.Progressed += target => targetProgress = Mathf.Max(target, targetProgress);

            if (loadingScreen)
            {
                EnableLoadingCanvas();
                await manager.LoadScenes(sceneGroups[index], progress, loadingScreen);
                EnableLoadingCanvas(false);
            }
            else
            {
                await manager.LoadScenes(sceneGroups[index], progress, loadingScreen);
            }
        }

        void EnableLoadingCanvas(bool enable = true)
        {
            isLoading = enable;
            loadingCanvas.gameObject.SetActive(enable);
            loadingCamera.gameObject.SetActive(enable);
        }
    }

    public class LoadingProgress : IProgress<float>
    {
        public event Action<float> Progressed;

        private const float ratio = 1f;
        
        public void Report(float value)
        {
            Progressed?.Invoke(value / ratio);
        }
    }
    
}

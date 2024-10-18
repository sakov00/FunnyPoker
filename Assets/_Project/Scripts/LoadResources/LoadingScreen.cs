using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.LoadResources
{
    public class LoadingScreen : MonoBehaviour
    {
        [Inject] private NetworkForTesting NetworkForTesting;
        [Inject] private LoadingResources LoadingResources;

        private bool IsNetworkReady;
        private bool IsAllResourcesLoaded;

        private void Start()
        {
            NetworkForTesting.CallBackConnection += LoadedNetwork;
            LoadingResources.CallBackAllResourcesLoaded += LoadedResources;
        }
        private void LoadedNetwork()
        {
            IsNetworkReady = true;
            StartGame();
        }

        private void LoadedResources()
        {
            IsAllResourcesLoaded = true;
            StartGame();
        }

        private void StartGame()
        {
            if (IsNetworkReady && IsAllResourcesLoaded)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainGameScene");
            }
        }
    }
}

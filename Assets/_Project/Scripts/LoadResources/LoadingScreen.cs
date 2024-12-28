using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Project.Scripts.LoadResources
{
    public class LoadingScreen : MonoBehaviour
    {
        private bool IsAllResourcesLoaded;

        private bool IsNetworkReady;
        [Inject] private LoadingResources LoadingResources;
        [Inject] private NetworkForTesting NetworkForTesting;

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
            if (IsNetworkReady && IsAllResourcesLoaded) SceneManager.LoadScene("MainGameScene");
        }
    }
}
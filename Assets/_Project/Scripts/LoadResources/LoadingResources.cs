using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.LoadResources
{
    public class LoadingResources : MonoBehaviour
    {
        [Inject] private AddressablesPrefabPool AddressablesPrefabPool;

        [SerializeField] private List<string> labelsGroupGame;

        private int loadedPrefabsCount = 0;

        public Action CallBackAllResourcesLoaded;

        private void Awake()
        {
            PhotonNetwork.PrefabPool = AddressablesPrefabPool;

            foreach (string label in labelsGroupGame)
            {
                AddressablesPrefabPool.PreLoadGroup(label, OnPrefabLoaded);
            }
        }

        private void OnPrefabLoaded()
        {
            loadedPrefabsCount++;

            if (loadedPrefabsCount >= labelsGroupGame.Count)
            {
                OnAllResourcesLoaded();
            }
        }

        private void OnAllResourcesLoaded()
        {
            CallBackAllResourcesLoaded.Invoke();
        }
    }
}

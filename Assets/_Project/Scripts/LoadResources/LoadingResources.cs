using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.LoadResources
{
    public class LoadingResources : MonoBehaviour
    {
        [SerializeField] private List<string> labelsGroupGame;
        [Inject] private AddressablesPrefabPool AddressablesPrefabPool;

        public Action CallBackAllResourcesLoaded;

        private int loadedPrefabsCount;

        private void Awake()
        {
            PhotonNetwork.PrefabPool = AddressablesPrefabPool;

            foreach (var label in labelsGroupGame) AddressablesPrefabPool.PreLoadGroup(label, OnPrefabLoaded);
        }

        private void OnPrefabLoaded()
        {
            loadedPrefabsCount++;

            if (loadedPrefabsCount >= labelsGroupGame.Count) OnAllResourcesLoaded();
        }

        private void OnAllResourcesLoaded()
        {
            CallBackAllResourcesLoaded.Invoke();
        }
    }
}
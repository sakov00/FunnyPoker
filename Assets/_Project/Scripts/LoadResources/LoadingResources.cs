using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets._Project.Scripts.LoadResources
{
    public class LoadingResources : MonoBehaviour
    {
        [Inject] private AddressablesPrefabPool AddressablesPrefabPool;

        [SerializeField] private List<AssetReference> prefabAssetRef;

        private int loadedPrefabsCount = 0;

        public Action CallBackAllResourcesLoaded;

        private void Awake()
        {
            PhotonNetwork.PrefabPool = AddressablesPrefabPool;

            foreach (AssetReference asset in prefabAssetRef)
            {
                PreLoadPrefab(asset);
            }
        }

        private void PreLoadPrefab(AssetReference prefabAssetRef)
        {
            AddressablesPrefabPool.PreLoad(prefabAssetRef.AssetGUID, OnPrefabLoaded);
        }

        private void OnPrefabLoaded(string prefabId, GameObject prefab)
        {
            loadedPrefabsCount++;

            if (loadedPrefabsCount >= prefabAssetRef.Count)
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

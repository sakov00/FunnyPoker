using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets._Project.Scripts.LoadResources
{
    public class LoadingResources : MonoBehaviour
    {
        [Inject] private AddressablesPrefabPool AddressablesPrefabPool;

        [SerializeField] private AssetReference playerPrefabAssetRef;

        public Action CallBackAllResourcesLoaded;

        private void Awake()
        {
            PhotonNetwork.PrefabPool = AddressablesPrefabPool;
            AddressablesPrefabPool.PreLoad(playerPrefabAssetRef.AssetGUID, OnPlayerPrefabLoaded);
        }

        private void OnPlayerPrefabLoaded(string prefabId, GameObject prefab)
        {
            CallBackAllResourcesLoaded.Invoke();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.LoadResources
{
    public class AddressablesPrefabPool : IPunPrefabPool
    {
        private readonly Dictionary<string, GameObject> loadedPrefabs = new();

        public Action<string, GameObject> CallBackPreLoaded;

        public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
        {
            if (loadedPrefabs.TryGetValue(prefabId, out var prefab))
                return GameObject.Instantiate(prefab, position, rotation);

            GameObject instance = null;
            var assetRef = new AssetReference(prefabId);
            assetRef.InstantiateAsync(position, rotation).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    prefab = handle.Result;
                    loadedPrefabs[prefabId] = prefab;
                    instance = prefab;
                }
                else
                {
                    Debug.LogError($"Failed to load prefab with id {prefabId}");
                }
            };

            return instance;
        }

        public void Destroy(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }

        public void PreLoadGroup(string labelGroup, Action callBackPreLoaded)
        {
            var loadOperation = Addressables.LoadAssetsAsync<GameObject>(labelGroup, null);
            loadOperation.Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (var prefab in handle.Result)
                    {
                        var prefabId = prefab.name;
                        loadedPrefabs[prefabId] = prefab;
                    }

                    callBackPreLoaded.Invoke();
                }
            };
        }
    }
}
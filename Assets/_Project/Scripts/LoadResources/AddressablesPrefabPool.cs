using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets._Project.Scripts.LoadResources
{
    public class AddressablesPrefabPool : IPunPrefabPool
    {
        private Dictionary<string, GameObject> loadedPrefabs = new Dictionary<string, GameObject>();

        public Action<string, GameObject> CallBackPreLoaded;

        public void PreLoad(string prefabId, Action<string, GameObject> callBackPreLoaded)
        {
            if (loadedPrefabs.TryGetValue(prefabId, out var prefab))
            {
                callBackPreLoaded.Invoke(prefabId, prefab);
            }
            else
            {
                var assetRef = new AssetReference(prefabId);
                assetRef.LoadAssetAsync<GameObject>().Completed += handle =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        prefab = handle.Result;
                        loadedPrefabs[prefabId] = prefab;
                        callBackPreLoaded.Invoke(prefabId, prefab);
                    }
                };
            }
        }

        public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
        {
            // Проверяем, есть ли уже загруженный префаб.
            if (loadedPrefabs.TryGetValue(prefabId, out var prefab))
            {
                return GameObject.Instantiate(prefab, position, rotation);
            }

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
    }
}

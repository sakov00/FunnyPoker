using Assets._Project.Scripts.Components;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets._Project.Scripts.Factories
{
    public class PlayerFactory
    {
        private DiContainer container;
        private AssetReference playerPrefabAssetRef;
        private AssetReference cameraPrefabAssetRef;

        public PlayerFactory(DiContainer container, AssetReference playerPrefabAssetRef, AssetReference cameraPrefabAssetRef)
        {
            this.container = container;
            this.playerPrefabAssetRef = playerPrefabAssetRef;
            this.cameraPrefabAssetRef = cameraPrefabAssetRef;
        }

        public GameObject CreatePlayer(Vector3 position, Quaternion rotation)
        {
            if (PhotonNetwork.LocalPlayer.IsLocal)
            {
                var player = PhotonNetwork.Instantiate(playerPrefabAssetRef.AssetGUID, position, rotation);
                InjectObject(player);

                if (player.GetComponent<PhotonView>().IsMine)
                {
                    var cameraProvider = player.GetComponent<CameraProvider>();
                    var cameraPosition = cameraProvider.value.Transform.position;
                    var cameraRotation = cameraProvider.value.Transform.rotation;
                    var mainCamera = PhotonNetwork.PrefabPool.Instantiate(cameraPrefabAssetRef.AssetGUID, cameraPosition, cameraRotation);
                    InjectObject(mainCamera);
                }
                return player;
            }
            return null;
        }

        private void InjectObject(GameObject gameObject)
        {
            container.Inject(gameObject);
            foreach (var component in gameObject.GetComponentsInChildren<MonoBehaviour>(true))
            {
                container.Inject(component);
            }
        }
    }
}

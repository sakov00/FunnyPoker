using Assets._Project.Scripts.Components;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.Factories
{
    public class PlayerFactory
    {
        private DiContainer container;
        private Object playerPrefab;
        private Object cameraPrefab;

        public PlayerFactory(DiContainer container, Object playerPrefab, Object cameraPrefab)
        {
            this.container = container;
            this.playerPrefab = playerPrefab;
            this.cameraPrefab = cameraPrefab;
        }

        public GameObject CreatePlayer(Vector3 position, Quaternion rotation)
        {
            if (PhotonNetwork.LocalPlayer.IsLocal)
            {
                var player = PhotonNetwork.Instantiate(playerPrefab.name, position, rotation);
                InjectObject(player);

                if (player.GetComponent<PhotonView>().IsMine)
                {
                    var cameraProvider = player.GetComponent<CameraPositionProvider>();
                    var cameraPosition = cameraProvider.value.CameraPosition.position;
                    var mainCamera = PhotonNetwork.PrefabPool.Instantiate(cameraPrefab.name, cameraPosition, rotation);
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

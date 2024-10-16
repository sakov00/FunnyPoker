using Photon.Pun;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.Factories
{
    public class PlayerFactory
    {
        private DiContainer _container;
        private Object _playerPrefab;
        private Object _playerCameraPrefab;

        public PlayerFactory(DiContainer container)
        {
            _container = container;
            LoadResources();
        }

        public void LoadResources()
        {
            _playerPrefab = Resources.Load("Player");
            _playerCameraPrefab = Resources.Load("PlayerCamera");
        }

        public void CreatePlayer(Vector3 position)
        {
            var player = PhotonNetwork.Instantiate(_playerPrefab.name, position, Quaternion.identity);

            var playerPhotonView = player.GetComponent<PhotonView>();
            if (playerPhotonView.Owner.IsLocal)
            {
                var playerCamera = (GameObject)GameObject.Instantiate(_playerCameraPrefab, position, Quaternion.identity);
            }
        }
    }
}

using _Project.Scripts.Data;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.Factories
{
    public class PlayerFactory
    {
        private GameObject _playerPrefab;
        private GameObject _cameraPrefab;

        public PlayerFactory(GameObject playerPrefab, GameObject cameraPrefab)
        {
            _playerPrefab = playerPrefab;
            _cameraPrefab = cameraPrefab;
        }

        public GameObject CreatePlayer(Vector3 position, Quaternion rotation)
        {
            if (!PhotonNetwork.LocalPlayer.IsLocal)
                return null;
            
            var player = PhotonNetwork.Instantiate(_playerPrefab.name, position, rotation);
            if (player.GetComponent<PhotonView>().IsMine)
            {
                var posForCamera = position;
                posForCamera.y += 0.7f;
                CreateCamera(posForCamera, rotation, player.transform);
            }

            return player;
        }
        
        private GameObject CreateCamera(Vector3 position, Quaternion rotation, Transform parent)
        {
            var camera = Object.Instantiate(_cameraPrefab, position, rotation, parent);
            return camera;
        }
    }
}

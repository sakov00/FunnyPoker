using _Project.Scripts.Data;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.Factories
{
    public class PlayerFactory
    {
        private Object playerPrefab;

        public PlayerFactory(Object playerPrefab)
        {
            this.playerPrefab = playerPrefab;
        }

        public PlayerData CreatePlayer(Vector3 position, Quaternion rotation)
        {
            if (PhotonNetwork.LocalPlayer.IsLocal)
            {
                var player = PhotonNetwork.Instantiate(playerPrefab.name, position, rotation);
                return player.GetComponent<PlayerData>();
            }
            return null;
        }
    }
}

using Assets._Project.Scripts.Factories;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.Network
{
    public class NetworkCallBacks : MonoBehaviourPunCallbacks, IInitializable
    {
        [Inject] PlayerFactory playerFactory;

        [SerializeField] private Transform[] spawnPlayerPoint;

        public void Initialize()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 3 });
        }

        public override void OnJoinedRoom()
        {
            var firstSpawnPoint = spawnPlayerPoint[PhotonNetwork.CountOfPlayers - 1];
            playerFactory.CreatePlayer(firstSpawnPoint.position, firstSpawnPoint.rotation);
            Destroy(firstSpawnPoint.gameObject);
        }
    }
}
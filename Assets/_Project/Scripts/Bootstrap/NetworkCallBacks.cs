using Assets._Project.Scripts.Factories;
using Assets._Project.Scripts.MonoBehLogic;
using Leopotam.EcsLite;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.Bootstrap
{
    public class NetworkCallBacks : MonoBehaviourPunCallbacks, IInitializable
    {
        [Inject] EcsWorld world;
        [Inject] PlayerFactory playerFactory;
        [Inject] EcsGameStartUp ecsGameStartUp;
        [Inject] PlayersTurnService playersTurnService;

        [SerializeField] private Transform[] spawnPlayerPoint;

        public void Initialize()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 1 });
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
                StartGame();
        }

        public override void OnJoinedRoom()
        {
            var firstSpawnPoint = spawnPlayerPoint[PhotonNetwork.CurrentRoom.PlayerCount - 1];
            var player = playerFactory.CreatePlayer(firstSpawnPoint.position, firstSpawnPoint.rotation);
            Destroy(firstSpawnPoint.gameObject);

            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
                StartGame();
        }

        private void StartGame()
        {
            playersTurnService.Initialize();
            ecsGameStartUp.Initialize();
        }
    }
}
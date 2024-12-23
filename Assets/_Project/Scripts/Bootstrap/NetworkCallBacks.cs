using _Project.Scripts.Services.Network;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.Bootstrap
{
    public class NetworkCallBacks : MonoBehaviourPunCallbacks, IInitializable
    {
        [Inject] private PlayersInRoomService _playersInRoomService;
        [Inject] private GameStartUp _gameStartUp;  

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
            _playersInRoomService.PlayerJoinedToRoom();
            
            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
                _gameStartUp.StartGame();
        }

        public override void OnJoinedRoom()
        {
            _playersInRoomService.PlayerJoinedToRoom();
            
            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
                _gameStartUp.StartGame();
        }
    }
}
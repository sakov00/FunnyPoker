using _Project.Scripts.Services.Network;
using Photon.Pun;
using Photon.Realtime;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class NetworkCallBacks : MonoBehaviourPunCallbacks, IInitializable
    {
        [Inject] private GameStartUp _gameStartUp;
        [Inject] private PlayersInfoInRoomService _playersInfoInRoomService;

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
            _playersInfoInRoomService.PlayerEnteredToRoom(newPlayer);

            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
                _gameStartUp.StartGame();
        }

        public override void OnJoinedRoom()
        {
            _playersInfoInRoomService.PlayerJoinedToRoom();

            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
                _gameStartUp.StartGame();
        }
    }
}
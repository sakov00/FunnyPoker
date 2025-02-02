using System;
using Photon.Pun;
using Photon.Realtime;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class NetworkCallBacks : MonoBehaviourPunCallbacks, IInitializable
    {
        public Action PlayerJoinedToRoom;
        public Action<Player> PlayerEnteredToRoom;
        public Action<Player> PlayerLeftRoom;

        public void Initialize()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            PlayerEnteredToRoom?.Invoke(player);
        }

        public override void OnJoinedRoom()
        { 
            PlayerJoinedToRoom?.Invoke();
        }

        public override void OnPlayerLeftRoom(Player player)
        {
            PlayerLeftRoom?.Invoke(player);
        }
    }
}
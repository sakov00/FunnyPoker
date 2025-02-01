using System;
using Photon.Pun;
using Photon.Realtime;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class NetworkCallBacks : MonoBehaviourPunCallbacks, IInitializable
    {
        public Action PlayerJoined;
        public Action<Player> PlayerEntered;
        public Action<Player> PlayerLeft;

        public void Initialize()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 3 });
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            PlayerEntered?.Invoke(player);
        }

        public override void OnJoinedRoom()
        { 
            PlayerJoined?.Invoke();
        }

        public override void OnPlayerLeftRoom(Player player)
        {
            PlayerLeft?.Invoke(player);
        }
    }
}
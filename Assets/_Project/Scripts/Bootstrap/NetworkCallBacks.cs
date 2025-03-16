using System;
using _Project.Scripts.Managers;
using Photon.Pun;
using Photon.Realtime;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class NetworkCallBacks : MonoBehaviourPunCallbacks, IInitializable
    {
        [Inject] private GameStateManager gameStateManager;
        
        public Action CallbackOnJoinedRoom;
        public Action CallbackOnLeftRoom;
        public Action<Player> CallbackOnPlayerEnteredRoom;
        
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
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 3 });
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            CallbackOnPlayerEnteredRoom?.Invoke(player);
        }

        public override void OnJoinedRoom()
        { 
            gameStateManager.SetState(0);
            CallbackOnJoinedRoom?.Invoke();
        }

        public override void OnPlayerLeftRoom(Player player)
        {
            CallbackOnLeftRoom?.Invoke();
        }
    }
}
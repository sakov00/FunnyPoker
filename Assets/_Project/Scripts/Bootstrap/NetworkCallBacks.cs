using System;
using _Project.Scripts.GameLogic.InputHandlers;
using _Project.Scripts.Managers;
using Photon.Pun;
using Photon.Realtime;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class NetworkCallBacks : MonoBehaviourPunCallbacks, IInitializable
    {
        [Inject] private GameStateManager gameStateManager;
        [Inject] private PlacesManager placesManager;
        [Inject] private InputHandler inputHandler;
        
        public Action CallbackOnJoinedRoom;
        public Action<Player> CallbackOnLeftRoom;
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
            placesManager.OnPlayerEnteredRoom(player);
            CallbackOnPlayerEnteredRoom?.Invoke(player);
        }

        public override void OnJoinedRoom()
        { 
            gameStateManager.SetState(0);
            placesManager.OnJoinedRoom();
            inputHandler.Initialize();
            
            CallbackOnJoinedRoom?.Invoke();
        }

        public override void OnPlayerLeftRoom(Player player)
        {
            placesManager.OnPlayerLeftRoom(player);
            CallbackOnLeftRoom?.Invoke(player);
        }
    }
}
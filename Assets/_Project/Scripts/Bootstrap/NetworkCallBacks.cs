using System;
using _Project.Scripts.GameLogic.PlayerInput;
using _Project.Scripts.Managers;
using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class NetworkCallBacks : MonoBehaviourPunCallbacks, IInitializable
    {
        [Inject] private GameStateManager gameStateManager;
        
        public Action Joined;
        public Action<Player> Left;
        public Action<Player> Entered;
        public Action<Hashtable> PropertiesUpdated;
        
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

        public async override void OnJoinedRoom()
        { 
            await UniTask.Yield();
            Joined?.Invoke();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Left?.Invoke(otherPlayer);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Entered?.Invoke(newPlayer);
        }
        
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            PropertiesUpdated?.Invoke(propertiesThatChanged);
        }
    }
}
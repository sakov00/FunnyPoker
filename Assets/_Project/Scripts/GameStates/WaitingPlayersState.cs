using _Project.Scripts.Bootstrap;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameStates
{
    public class WaitingPlayersState : IGameState
    {
        [Inject] private NetworkCallBacks _networkCallBacks;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private PlacesManager _placesManager;
        
        public void EnterState()
        {
            Debug.Log("Ожидание игроков...");
            _networkCallBacks.CallbackOnJoinedRoom += OnJoinedRoom;
            _networkCallBacks.CallbackOnPlayerEnteredRoom += OnPlayerEnteredRoom;
        }

        private void OnJoinedRoom()
        {
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            if (PhotonNetwork.CurrentRoom == null ||
                PhotonNetwork.CurrentRoom.PlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers)
                return;
            
            _gameStateManager.Next();
        }

        private void OnPlayerEnteredRoom(Player newPlayer)
        {
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            if (PhotonNetwork.CurrentRoom == null ||
                PhotonNetwork.CurrentRoom.PlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers)
                return;
            
            _gameStateManager.Next();
        }

        public void ExitState()
        {
            _networkCallBacks.CallbackOnJoinedRoom -= OnJoinedRoom;
            _networkCallBacks.CallbackOnPlayerEnteredRoom -= OnPlayerEnteredRoom;
            Debug.Log("Все игроки подключены. Начинаем игру.");
        }
    }
}
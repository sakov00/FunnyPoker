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
        [Inject] private NetworkCallBacks networkCallBacks;
        [Inject] private GameStateManager gameStateManager;
        
        public void EnterState()
        {
            Debug.Log("Ожидание игроков...");
            
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            networkCallBacks.Entered += OnPlayerEnteredRoom;
        }

        private void OnPlayerEnteredRoom(Player newPlayer)
        {
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            if (PhotonNetwork.CurrentRoom == null ||
                PhotonNetwork.CurrentRoom.PlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers)
                return;

            gameStateManager.Next();
        }

        public void ExitState()
        {
            Debug.Log("Все игроки подключены. Начинаем игру.");
            
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            networkCallBacks.Entered -= OnPlayerEnteredRoom;
        }
    }
}
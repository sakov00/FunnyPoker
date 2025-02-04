using System.Linq;
using _Project.Scripts.Bootstrap;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using _Project.Scripts.Services;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.GameStates
{
    public class WaitingForPlayersState : IGameState, IFixedTickable
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private PlacesManager _placesManager;
        
        public bool IsCompleted { get; set; }
        
        public void EnterState()
        {
            Debug.Log("Ожидание игроков...");
            IsCompleted = false;
        }
        
        public void FixedTick()
        {
            if (IsCompleted)
                return;

            if (PhotonNetwork.CurrentRoom == null ||
                PhotonNetwork.CurrentRoom.PlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers)
                return;
            
            _placesManager.ActivateRandomPlace();
            _gameStateManager.SetState<DealingCardsState>();
        }

        public void ExitState()
        {
            IsCompleted = true;
            Debug.Log("Все игроки подключены. Начинаем игру.");
        }
    }
}
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.GameStates
{
    public class WaitingForPlayersState : IGameState
    {
        [Inject] private ServicePlaces _servicePlaces;
        
        public bool Completed { get; set; }
        
        public void EnterState()
        {
            Debug.Log("Ожидание игроков...");
        }

        public void UpdateState()
        {
            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                _servicePlaces.ActivateRandomPlace();
                Completed = true; 
            }
        }

        public void ExitState()
        {
            Debug.Log("Все игроки подключены. Начинаем игру.");
        }
    }
}
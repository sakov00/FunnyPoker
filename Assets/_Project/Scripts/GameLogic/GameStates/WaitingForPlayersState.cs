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
    public class WaitingForPlayersState : IGameState
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private NetworkCallBacks _networkCallBacks;
        [Inject] private PlacesManager _placesManager;
        
        public void EnterState()
        {
            Debug.Log("Ожидание игроков...");
            _networkCallBacks.PlayerEnteredToRoom += CheckPlayersCount;
        }
        
        private void CheckPlayersCount(Player player)
        {
            var connectedPlayers = _placesManager.AllPlayerPlaces.Count(place => !place.IsFreeSync);
            if (connectedPlayers == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                _placesManager.ActivateRandomPlace();
                _gameStateManager.SetState<DealingCardsState>();
            }
        }

        public void ExitState()
        {
            _networkCallBacks.PlayerEnteredToRoom -= CheckPlayersCount;
            Debug.Log("Все игроки подключены. Начинаем игру.");
        }
    }
}
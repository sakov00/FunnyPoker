using System.Threading.Tasks;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using _Project.Scripts.Services;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.GameStates
{
    public class DealingCardsState : IGameState
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private CardsService _cardsService;
        
        public async void EnterState()
        {
            Debug.Log("Раздача карт...");
            
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            _cardsService.DealTwoCardsToPlayers();
            _gameStateManager.SetState<BettingState>();
        }
        
        public void FixedUpdate()
        {
            
        }

        public void ExitState()
        {
            Debug.Log("Карты разданы.");
        }
    }
}
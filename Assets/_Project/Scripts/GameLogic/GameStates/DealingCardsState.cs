using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using _Project.Scripts.Services;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.GameStates
{
    public class DealingCardsState : IGameState
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private DeskService _deskService;
        
        public bool IsCompleted { get; set; }
        
        public void EnterState()
        {
            Debug.Log("Раздача карт...");
            _deskService.DealCards(2);
            _gameStateManager.SetState<BettingState>();
        }

        public void ExitState()
        {
            Debug.Log("Карты разданы.");
        }
    }
}
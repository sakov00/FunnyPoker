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
        [Inject] private DeskService _deskService;
        
        public async void EnterState()
        {
            Debug.Log("Раздача карт...");
            _deskService.DealCards(2);
            await Task.Delay(3000);
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
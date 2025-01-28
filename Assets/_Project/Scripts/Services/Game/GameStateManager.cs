using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.GameLogic.GameStates;
using _Project.Scripts.Interfaces;
using Photon.Pun;
using Zenject;

namespace _Project.Scripts.Services.Game
{
    public class GameStateManager : MonoBehaviourPunCallbacks
    {
        [Inject] private List<IGameState> _gameStates = new ();
        private IGameState _currentState;

        private void Update()
        {
            _currentState?.UpdateState();
            if (_currentState?.Completed == true)
            {
                NextState();
            }
        }
        
        public void SetState(GameStates newState)
        {
            _currentState?.ExitState();
            
            switch (newState)
            {
                case GameStates.WaitingForPlayersState:
                    _currentState = _gameStates.OfType<WaitingForPlayersState>().FirstOrDefault();
                    break;
                case GameStates.DealingCardsState:
                    _currentState = _gameStates.OfType<DealingCardsState>().FirstOrDefault();
                    break;
            }
            
            _currentState?.EnterState();
        }

        public void NextState()
        {
            var nextStateIndex = _gameStates.IndexOf(_currentState) + 1;
            _currentState?.ExitState();
            _currentState = _gameStates[nextStateIndex];
            _currentState?.EnterState();
        }
    }
}
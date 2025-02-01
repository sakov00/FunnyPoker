using System;
using System.Collections.Generic;
using _Project.Scripts.Interfaces;
using ExitGames.Client.Photon;
using Photon.Pun;
using Zenject;

namespace _Project.Scripts.Managers
{
    public class GameStateManager : MonoBehaviourPunCallbacks
    {
        private const string GameStateKey = "GameState";
        private Dictionary<Type, IGameState> _gameStates;
        private IGameState _currentState;

        [Inject]
        public void Initialize(List<IGameState> states)
        {
            _gameStates = new Dictionary<Type, IGameState>();
            foreach (var state in states)
            {
                _gameStates[state.GetType()] = state;
            }
        }
        
        public void SetState<T>() where T : IGameState
        {
            _currentState?.ExitState();
            _currentState = _gameStates[typeof(T)];
            
            var property = new Hashtable { { GameStateKey, _currentState.GetType().AssemblyQualifiedName } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(property);
            
            _currentState?.EnterState();
        }
        
        public override void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            _currentState = (IGameState)Type.GetType((string)changedProps[GameStateKey]);
        }
    }
}
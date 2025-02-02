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
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            _currentState?.ExitState();
            _currentState = _gameStates[typeof(T)];
            
            var property = new Hashtable { { GameStateKey, _currentState.GetType().AssemblyQualifiedName } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(property);
            
            _currentState?.EnterState();
        }
        
        public void LoadInfoFromPhoton()
        {
            var currentRoom = PhotonNetwork.CurrentRoom;

            if (currentRoom.CustomProperties.ContainsKey(GameStateKey))
            {
                var currentType = Type.GetType((string)currentRoom.CustomProperties[GameStateKey]);
                _gameStates.TryGetValue(currentType, out _currentState);
            }
        }
        
        public override void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            if (changedProps.ContainsKey(GameStateKey))
            {
                var currentType = Type.GetType((string)changedProps[GameStateKey]);
                _gameStates.TryGetValue(currentType, out _currentState);
            }
        }
    }
}
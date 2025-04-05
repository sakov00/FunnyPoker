using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Bootstrap;
using _Project.Scripts.GameStates;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Zenject;

namespace _Project.Scripts.Managers
{
    public class GameStateManager : IInitializable, IDisposable
    {
        [Inject] private NetworkCallBacks networkCallBacks;
        [Inject] private List<IGameState> gameStates;
        
        private const string GameStateKey = "GameState";
        private IGameState currentState;
        
        public void Initialize()
        {
            networkCallBacks.PropertiesUpdated += PropertiesUpdate;
            networkCallBacks.Joined += PlayerJoined;
        }

        private void PlayerJoined()
        {
            SetState<WaitingPlayersState>();
        }
        
        public void Next()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            int index = gameStates.IndexOf(currentState);
            if (index < 0 || index + 1 >= gameStates.Count)
                return;

            var nextState = gameStates[index + 1];
            SetState(nextState.GetType());
        }
        
        public void SetState<T>() where T : IGameState
        {
            SetState(typeof(T));
        }
        
        private void SetState(Type stateType)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            var newState = gameStates.FirstOrDefault(state => state.GetType() == stateType);
            if (newState == null || newState == currentState)
                return;
            
            var oldProps = new Hashtable {{ GameStateKey, currentState?.GetType().AssemblyQualifiedName }};
            var props = new Hashtable {{ GameStateKey, stateType.AssemblyQualifiedName }};
            PhotonNetwork.CurrentRoom.SetCustomProperties(props, oldProps);
        }
        
        private void ChangeState(IGameState newState)
        {
            currentState?.ExitState();
            currentState = newState;
            currentState?.EnterState();
        }

        private void PropertiesUpdate(Hashtable changedProps)
        {
            if (!changedProps.TryGetValue(GameStateKey, out var stateTypeNameObj) ||
                stateTypeNameObj is not string stateTypeName)
                return;
            
            var type = Type.GetType(stateTypeName);
            if (type == null)
                return;

            var newState = gameStates.FirstOrDefault(state => state.GetType() == type);
            if (newState == null || newState == currentState)
                return;
            
            ChangeState(newState);
        }

        public void Dispose()
        {
            networkCallBacks.PropertiesUpdated -= PropertiesUpdate;
            networkCallBacks.Joined -= PlayerJoined;
        }
    }
}
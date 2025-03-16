using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Zenject;

namespace _Project.Scripts.Managers
{
    public class GameStateManager : IInRoomCallbacks
    {
        private const string GameStateKey = "GameState";
        private Dictionary<int, IGameState> gameStates;
        private IGameState currentState;

        [Inject]
        public void Initialize(List<IGameState> states)
        {
            PhotonNetwork.AddCallbackTarget(this);
            gameStates = new Dictionary<int, IGameState>();
            for (int i = 0; i < states.Count; i++)
            {
                gameStates[i] = states[i];
            }
        }
        
        public void Next()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            
            int currentKey = gameStates.FirstOrDefault(x => x.Value == currentState).Key;

            SetState(currentKey + 1);
        }

        public void SetState(int stateKey)
        {
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            if (!gameStates.ContainsKey(stateKey))
                return;
            
            currentState?.ExitState();
            currentState = gameStates[stateKey];
            
            var property = new Hashtable { { GameStateKey, stateKey } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(property);

            currentState?.EnterState();
        }

        public void OnPlayerEnteredRoom(Player newPlayer) { }
        public void OnPlayerLeftRoom(Player otherPlayer) { }
        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) { }
        public void OnRoomPropertiesUpdate(Hashtable changedProps) { LoadFromPhoton(); }
        public void OnMasterClientSwitched(Player newMasterClient) { }

        private void LoadFromPhoton()
        {
            var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

            if (roomProps.TryGetValue(GameStateKey, out var gameStateKey))
            {
                gameStates.TryGetValue((int)gameStateKey, out var photonState);
                if (photonState != currentState)
                {
                    currentState?.ExitState();
                    currentState = photonState;
                    currentState?.EnterState();
                }
            }
        }
    }
}
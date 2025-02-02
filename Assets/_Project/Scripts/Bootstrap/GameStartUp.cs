using System;
using _Project.Scripts.GameLogic.GameStates;
using _Project.Scripts.Managers;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class GameStartUp : MonoBehaviour
    {
        [Inject] private NetworkCallBacks _networkCallBacks;
        [Inject] private GameStateManager _gameStateManager;

        public void Start()
        {
            _networkCallBacks.PlayerJoinedToRoom += PlayerJoinedToRoom;
        }

        private void PlayerJoinedToRoom()
        {
            _gameStateManager.SetState<WaitingForPlayersState>();
            _gameStateManager.LoadInfoFromPhoton();
        }

        private void OnDestroy()
        {
            _networkCallBacks.PlayerJoinedToRoom -= PlayerJoinedToRoom;
        }
    }
}
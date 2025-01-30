using System;
using _Project.Scripts.Enums;
using _Project.Scripts.Managers;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class GameStartUp : MonoBehaviour
    {
        [Inject] private NetworkCallBacks _networkCallBacks;
        [Inject] private GameStateManager _gameStateManager;

        private void Start()
        {
            _networkCallBacks.PlayerEntered += StartGame;
        }

        private void StartGame(Player player)
        {
            _gameStateManager.SetState(GameStates.WaitingForPlayersState);
        }
    }
}
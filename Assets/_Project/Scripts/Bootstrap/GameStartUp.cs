using _Project.Scripts.GameLogic.GameStates;
using _Project.Scripts.Managers;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class GameStartUp : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;

        private void Start()
        {
            _gameStateManager.SetState<WaitingForPlayersState>();
        }
    }
}
using _Project.Scripts.Bootstrap;
using _Project.Scripts.Enums;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Services.Game
{
    public class GameManager :MonoBehaviour
    {
        [Inject] private NetworkCallBacks _networkCallBacks;
        [Inject] private GameStateManager _stateManager;

        private void Awake()
        {
            _networkCallBacks.PlayerJoined += Initialize;
        }

        private void Initialize()
        {
            _stateManager.SetState(GameStates.WaitingForPlayersState);
        }

        private void OnDestroy()
        {
            _networkCallBacks.PlayerJoined -= Initialize;
        }
    }
}
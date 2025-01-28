using _Project.Scripts.Bootstrap;
using _Project.Scripts.Factories;
using _Project.Scripts.GameLogic.GameStates;
using _Project.Scripts.Services.Game;
using _Project.Scripts.Services.Network;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.InjectInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject cameraPrefab;
        [SerializeField] private NetworkCallBacks networkCallBacks;
        [SerializeField] private GameStartUp gameStartUp;
        [SerializeField] private PlayersInfoService playersInfoService;
        [SerializeField] private ServicePlaces servicePlaces;
        
        [Header("Managers")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameStateManager gameStateManager;
        
        public override void InstallBindings()
        {
            BindNetwork();
            BindGame();
            BindGameStates();
            BindManagers();
            BindServices();
            BindFactories();
        }

        private void BindNetwork()
        {
            Container.BindInterfacesAndSelfTo<NetworkCallBacks>().FromInstance(networkCallBacks).AsSingle().NonLazy();
        }

        private void BindGame()
        {
            Container.BindInstance(gameStartUp).AsSingle();
        }
        
        private void BindGameStates()
        {
            Container.BindInterfacesAndSelfTo<WaitingForPlayersState>().AsSingle();
            Container.BindInterfacesAndSelfTo<DealingCardsState>().AsSingle();
        }

        private void BindManagers()
        {
            Container.BindInstance(gameStateManager).AsSingle();
            Container.BindInstance(gameManager).AsSingle();
            
        }

        private void BindServices()
        {
            Container.BindInstance(servicePlaces).AsSingle();
            Container.BindInstance(playersInfoService).AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerPrefab, cameraPrefab);
        }
    }
}
using _Project.Scripts.Bootstrap;
using _Project.Scripts.Factories;
using _Project.Scripts.GameLogic.GameStates;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using _Project.Scripts.Services;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.InjectInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject cameraPrefab;
        [SerializeField] private NetworkCallBacks networkCallBacks;
        [SerializeField] private GameStartUp gameStartUp;
        
        
        [Header("Managers")]
        [SerializeField] private PlacesManager placesManager;
        [SerializeField] private GameStateManager gameStateManager;
        
        [Header("Services")]
        [SerializeField] private CardsService cardsService;
        
        [Header("GameStates")]
        [SerializeField] private WaitingPlayersState waitingPlayersState;
        
        public override void InstallBindings()
        {
            BindNetwork();
            BindGame();
            BindFactories();
            BindServices();
            BindGameStates();
            BindManagers();
        }

        private void BindNetwork()
        {
            Container.BindInterfacesAndSelfTo<NetworkCallBacks>().FromInstance(networkCallBacks).AsSingle().NonLazy();
        }

        private void BindGame()
        {
            Container.BindInstance(gameStartUp).AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerPrefab, cameraPrefab);
        }

        private void BindServices()
        {
            Container.BindInstance(cardsService).AsSingle();
        }

        private void BindGameStates()
        {
            Container.BindInterfacesAndSelfTo<WaitingPlayersState>().FromInstance(waitingPlayersState).AsSingle();
            Container.BindInterfacesAndSelfTo<DealingCardsState>().AsSingle();
            Container.BindInterfacesAndSelfTo<BettingState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShowdownState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ResultState>().AsSingle();
        }

        private void BindManagers()
        {
            Container.BindInstance(placesManager).AsSingle();
            Container.BindInstance(gameStateManager).AsSingle()
                .WithArguments(Container.ResolveAll<IGameState>());
        }
    }
}
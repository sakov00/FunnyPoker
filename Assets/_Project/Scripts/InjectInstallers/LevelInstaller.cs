using _Project.Scripts.Bootstrap;
using _Project.Scripts.Factories;
using _Project.Scripts.GameLogic.GameStates;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using _Project.Scripts.MVP.Table;
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
        
        [Header("Game Objects")]
        [SerializeField] private TablePresenter tablePresenter;
        
        [Header("Managers")]
        [SerializeField] private PlacesManager placesManager;
        [SerializeField] private GameStateManager gameStateManager;
        
        [Header("Services")]
        [SerializeField] private CardsService cardsService;
        
        public override void InstallBindings()
        {
            BindNetwork();
            BindGameObjects();
            BindFactories();
            BindServices();
            BindGameStates();
            BindManagers();
        }

        private void BindNetwork()
        {
            Container.BindInterfacesAndSelfTo<NetworkCallBacks>().FromInstance(networkCallBacks).AsSingle().NonLazy();
        }

        private void BindGameObjects()
        {
            Container.BindInstance(tablePresenter).AsSingle();
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
            Container.BindInterfacesAndSelfTo<WaitingPlayersState>().AsSingle();
            Container.BindInterfacesAndSelfTo<DealingCardsState>().AsSingle();
            Container.BindInterfacesAndSelfTo<PreflopState>().AsSingle();
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
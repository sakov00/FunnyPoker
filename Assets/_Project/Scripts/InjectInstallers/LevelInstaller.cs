using _Project.Scripts.Bootstrap;
using _Project.Scripts.Factories;
using _Project.Scripts.GameLogic.InputHandlers;
using _Project.Scripts.GameLogic.PlayerCanvases;
using _Project.Scripts.GameLogic.PlayerInput;
using _Project.Scripts.GameStates;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using _Project.Scripts.MVP.Table;
using _Project.Scripts.Services;
using UnityEngine;
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
        [SerializeField] private InputHandler inputHandler;
        
        [Header("Managers")]
        [SerializeField] private CardsManager cardsManager;
        [SerializeField] private PlacesManager placesManager;
        [SerializeField] private PlayerInputManager playerInputManager;
        
        [Header("Canvases")]
        [SerializeField] private StartGameCanvas startGameCanvas;    
        [SerializeField] private MainGameCanvas mainGameCanvas; 
        [SerializeField] private EndGameCanvas endGameCanvas; 
        
        public override void InstallBindings()
        {
            BindNetwork();
            BindGameObjects();
            BindManagers();
            BindServices();
            BindGameStates();
            BindFactories();
            BindCanvases();
        }

        private void BindNetwork()
        {
            Container.BindInterfacesAndSelfTo<NetworkCallBacks>().FromInstance(networkCallBacks).AsSingle().NonLazy();
        }

        private void BindGameObjects()
        {
            Container.BindInstance(inputHandler).AsSingle();
            Container.BindInstance(tablePresenter).AsSingle();
        }
        
        private void BindManagers()
        {
            Container.Bind<GameStateManager>().AsSingle()
                .WithArguments(Container.ResolveAll<IGameState>());
            Container.BindInstance(placesManager).AsSingle();
            Container.BindInstance(cardsManager).AsSingle();
            Container.BindInstance(playerInputManager).AsSingle();
            Container.Bind<CanvasesManager>().AsSingle();
        }
        
        private void BindServices()
        {
            Container.Bind<RoundService>().AsSingle();
        }
        
        private void BindGameStates()
        {
            Container.BindInterfacesAndSelfTo<WaitingPlayersState>().AsSingle();
            Container.BindInterfacesAndSelfTo<DealingCardsState>().AsSingle();
            Container.BindInterfacesAndSelfTo<PreflopState>().AsSingle();
            Container.BindInterfacesAndSelfTo<FlopState>().AsSingle();
            Container.BindInterfacesAndSelfTo<RiverState>().AsSingle();
            Container.BindInterfacesAndSelfTo<TurnState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShowdownState>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerPrefab, cameraPrefab);
        }

        private void BindCanvases()
        {
            Container.BindInstance(startGameCanvas).AsSingle();
            Container.BindInstance(mainGameCanvas).AsSingle();
            Container.BindInstance(endGameCanvas).AsSingle();
        }
    }
}
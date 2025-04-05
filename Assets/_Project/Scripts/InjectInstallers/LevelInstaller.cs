using _Project.Scripts.Bootstrap;
using _Project.Scripts.Factories;
using _Project.Scripts.GameLogic.Data;
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
        [SerializeField] private GameData gameData;
        
        [Header("Canvases")]
        [SerializeField] private StartGameCanvas startGameCanvas;    
        [SerializeField] private MainGameCanvas mainGameCanvas; 
        [SerializeField] private EndGameCanvas endGameCanvas; 
        
        public override void InstallBindings()
        {
            BindNetwork();
            BindGameObjects();
            BindFactories();
            BindManagers();
            BindServices();
            BindGameStates();
            BindCanvases();
            BindGameData();
        }

        private void BindNetwork()
        {
            Container.BindInterfacesAndSelfTo<NetworkCallBacks>().FromInstance(networkCallBacks).AsSingle().NonLazy();
        }

        private void BindGameObjects()
        {
            
        }
        
        private void BindFactories()
        {
            Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerPrefab, cameraPrefab);
        }
        
        private void BindManagers()
        {
            Container.BindInterfacesAndSelfTo<PlayerInputHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle();
            Container.Bind<CardsManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
            Container.Bind<CanvasesManager>().AsSingle();
        }
        
        private void BindServices()
        {
            Container.Bind<RoundService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlaceSync>().AsSingle();
            Container.BindInterfacesAndSelfTo<DataSync>().AsSingle();
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

        private void BindCanvases()
        {
            Container.BindInstance(startGameCanvas).AsSingle();
            Container.BindInstance(mainGameCanvas).AsSingle();
            Container.BindInstance(endGameCanvas).AsSingle();
        }

        private void BindGameData()
        {
            Container.BindInstance(gameData).AsSingle();
        }
    }
}
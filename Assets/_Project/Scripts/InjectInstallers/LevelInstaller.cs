using _Project.Scripts.Bootstrap;
using _Project.Scripts.Factories;
using _Project.Scripts.GameLogic.GameStates;
using _Project.Scripts.Managers;
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
        [SerializeField] private GameStartUp gameStartUp;
        [SerializeField] private ServicePlaces servicePlaces;
        
        [Header("Managers")]
        [SerializeField] private GameStateManager gameStateManager;
        
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
            Container.BindInstance(servicePlaces).AsSingle();
        }

        private void BindGameStates()
        {
            Container.BindInterfacesAndSelfTo<WaitingForPlayersState>().AsSingle();
            Container.BindInterfacesAndSelfTo<DealingCardsState>().AsSingle();
        }

        private void BindManagers()
        {
            Container.BindInstance(gameStateManager).AsSingle();
        }
    }
}
using _Project.Scripts.Bootstrap;
using _Project.Scripts.Factories;
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
        
        public override void InstallBindings()
        {
            BindNetwork();
            BindGame();
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
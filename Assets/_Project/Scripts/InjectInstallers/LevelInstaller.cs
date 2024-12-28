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
        [SerializeField] private GameStartUp gameStartUp;
        [SerializeField] private PlayersInfoInRoomService playersInfoInRoomService;
        [SerializeField] private ServicePlaces servicePlaces;

        public override void InstallBindings()
        {
            BindGame();
            BindServices();
            BindFactories();
            BindNetwork();
        }

        private void BindGame()
        {
            Container.BindInstance(gameStartUp).AsSingle();
        }

        private void BindServices()
        {
            Container.BindInstance(servicePlaces).AsSingle();
            Container.BindInstance(playersInfoInRoomService).AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerPrefab, cameraPrefab);
        }

        private void BindNetwork()
        {
            Container.BindInterfacesAndSelfTo<NetworkCallBacks>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}
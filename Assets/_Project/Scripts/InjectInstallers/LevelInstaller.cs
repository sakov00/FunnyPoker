using _Project.Scripts.Services.Game;
using _Project.Scripts.Services.Network;
using Assets._Project.Scripts.Bootstrap;
using Assets._Project.Scripts.Factories;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.InjectInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject cameraPrefab;
        [SerializeField] private GameStartUp gameStartUp;
        [SerializeField] private PlayersInfoInRoomService playersInfoInRoomService;
        [SerializeField] private QueuePlayerController queuePlayerController;

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
            Container.BindInstance(playersInfoInRoomService).AsSingle();
            Container.BindInstance(queuePlayerController).AsSingle();
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

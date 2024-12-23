using _Project.Scripts.Services.Network;
using Assets._Project.Scripts.Bootstrap;
using Assets._Project.Scripts.Factories;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.InjectInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private Object playerPrefab;
        [SerializeField] private GameStartUp gameStartUp;
        [SerializeField] private PlayersInRoomService playersInRoomService;

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
            Container.BindInstance(playersInRoomService).AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerPrefab);
        }

        private void BindNetwork()
        {
            Container.BindInterfacesAndSelfTo<NetworkCallBacks>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}

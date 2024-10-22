using Assets._Project.Scripts.Bootstrap;
using Assets._Project.Scripts.Factories;
using Assets._Project.Scripts.Interfaces;
using Assets._Project.Scripts.MonoBehLogic;
using Assets._Project.Scripts.Systems;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.InjectInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private Object playerPrefab;
        [SerializeField] private Object cameraPrefab;

        public override void InstallBindings()
        {
            BindServices();
            BindEcs();
            BindFactories();
            BindNetwork();
        }

        private void BindServices()
        {
            Container.Bind<PlayersTurnService>().FromComponentInHierarchy().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerPrefab, cameraPrefab);
        }

        private void BindEcs()
        {
            Container.Bind<EcsWorld>().FromInstance(new EcsWorld()).AsSingle();

            Container.Bind<IEcsFixedUpdateSystem>().To<PlayerCanvasActiveSystem>().AsSingle();

            Container.Bind<IEcsUpdateSystem>().To<CameraSystem>().AsSingle();

            Container.Bind<EcsGameStartUp>().FromComponentInHierarchy().AsSingle().NonLazy();
        }

        private void BindNetwork()
        {
            Container.BindInterfacesAndSelfTo<NetworkCallBacks>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}

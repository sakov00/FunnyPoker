using Assets._Project.Scripts.Bootstrap;
using Assets._Project.Scripts.Factories;
using UnityEngine.AddressableAssets;
using UnityEngine;
using Zenject;
using Leopotam.EcsLite;
using Assets._Project.Scripts.Interfaces;
using Assets._Project.Scripts.Systems;
using Assets._Project.Scripts.Network;
using Assets._Project.Scripts.MonoBehLogic;

namespace Assets._Project.Scripts.InjectInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private AssetReference playerAsset;
        [SerializeField] private AssetReference cameraAsset;

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
            Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerAsset, cameraAsset);
        }

        private void BindEcs()
        {
            Container.Bind<EcsWorld>().FromInstance(new EcsWorld()).AsSingle();

            Container.Bind<IEcsFixedUpdateSystem>().To<PlayerCanvasActiveSystem>().AsSingle();

            Container.Bind<EcsGameStartUp>().FromComponentInHierarchy().AsSingle().NonLazy();
        }

        private void BindNetwork()
        {
            Container.BindInterfacesAndSelfTo<NetworkCallBacks>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}

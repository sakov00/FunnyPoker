using Assets._Project.Scripts.Bootstrap;
using Assets._Project.Scripts.Factories;
using UnityEngine.AddressableAssets;
using UnityEngine;
using Zenject;
using Leopotam.EcsLite;
using Assets._Project.Scripts.Interfaces;
using Assets._Project.Scripts.Systems;
using Assets._Project.Scripts.Network;
using Assets._Project.Scripts.LoadResources;

namespace Assets._Project.Scripts.InjectInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private AssetReference playerAsset;

        public override void InstallBindings() 
        {
            Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerAsset);

            var world = new EcsWorld();
            Container.Bind<EcsWorld>().FromInstance(world).AsSingle();

            //Container.Bind<IEcsFixedUpdateSystem>().To<PlayerSpawnSystem>().AsSingle();

            Container.BindInterfacesAndSelfTo<EcsGameStartUp>().FromComponentInHierarchy().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<NetworkCallBacks>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}

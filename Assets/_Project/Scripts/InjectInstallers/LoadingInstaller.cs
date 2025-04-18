﻿using _Project.Scripts.LoadResources;
using Zenject;

namespace _Project.Scripts.InjectInstallers
{
    internal class LoadingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AddressablesPrefabPool>().AsSingle();
            Container.Bind<LoadingResources>().FromComponentInHierarchy().AsSingle();
            Container.Bind<NetworkForTesting>().FromComponentInHierarchy().AsSingle();
        }
    }
}
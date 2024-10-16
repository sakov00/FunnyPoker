using Assets._Project.Scripts.Bootstrap;
using Zenject;

namespace Assets._Project.Scripts.InjectInstallers
{
    internal class LevelInstaller : MonoInstaller
    {
        public override void InstallBindings() 
        {
            Container.BindInterfacesAndSelfTo<EcsGameStartUp>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}

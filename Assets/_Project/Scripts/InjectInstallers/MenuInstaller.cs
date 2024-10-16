using Assets._Project.Scripts.Menu.CurrentNetworkRoom;
using Assets._Project.Scripts.Menu.ManagmentPanels;
using Assets._Project.Scripts.Menu.Network;
using Zenject;

namespace Assets._Project.Scripts.InjectInstallers
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PanelModel>().AsSingle();
            Container.Bind<PanelView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PanelPresenter>().FromComponentInHierarchy().AsSingle();

            Container.Bind<NetworkModel>().AsSingle();
            Container.Bind<NetworkView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<NetworkPresenter>().FromComponentInHierarchy().AsSingle();

            Container.Bind<CurrentRoomModel>().AsSingle();
            Container.Bind<CurrentRoomView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CurrentRoomPresenter>().FromComponentInHierarchy().AsSingle();
        }
    }
}
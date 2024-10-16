using UniRx;

namespace Assets._Project.Scripts.Menu.CurrentNetworkRoom
{
    public class CurrentRoomModel
    {
        public ReactiveCollection<PlayerElement> PlayerElements { get; set; }

        public CurrentRoomModel()
        {
            PlayerElements = new ReactiveCollection<PlayerElement>();
        }
    }
}

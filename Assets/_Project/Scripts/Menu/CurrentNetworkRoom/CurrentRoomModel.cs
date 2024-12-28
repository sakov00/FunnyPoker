using UniRx;

namespace _Project.Scripts.Menu.CurrentNetworkRoom
{
    public class CurrentRoomModel
    {
        public CurrentRoomModel()
        {
            PlayerElements = new ReactiveCollection<PlayerElement>();
        }

        public ReactiveCollection<PlayerElement> PlayerElements { get; set; }
    }
}
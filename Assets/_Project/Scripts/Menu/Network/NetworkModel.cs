using Photon.Realtime;
using UniRx;

namespace _Project.Scripts.Menu.Network
{
    public class NetworkModel
    {
        public NetworkModel()
        {
            RoomInfos = new ReactiveCollection<RoomInfo>();
            RoomElements = new ReactiveCollection<RoomElement>();
        }

        public ReactiveCollection<RoomInfo> RoomInfos { get; set; }

        public ReactiveCollection<RoomElement> RoomElements { get; set; }
    }
}
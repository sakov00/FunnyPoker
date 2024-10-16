using Photon.Realtime;
using UniRx;

namespace Assets._Project.Scripts.Menu.Network
{
    public class NetworkModel
    {
        public ReactiveCollection<RoomInfo> RoomInfos { get; set; }

        public ReactiveCollection<RoomElement> RoomElements { get; set; }

        public NetworkModel()
        {
            RoomInfos = new ReactiveCollection<RoomInfo>();
            RoomElements = new ReactiveCollection<RoomElement>();
        }
    }
}

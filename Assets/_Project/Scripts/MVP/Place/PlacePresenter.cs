using _Project.Scripts.MVP.Views;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.MVP.Place
{
    public class PlacePresenter : MonoBehaviourPunCallbacks
    {
        private readonly CompositeDisposable _disposables = new ();
        
        [field: SerializeField] public PlaceData Data { get; set; }
        [field: SerializeField] public PlaceSync Sync { get; set; }
        [field: SerializeField] private PlaceView View { get; set; }

        private void Start()
        {
            Sync.Init(Data.Number); 
            Sync.IsEnabledReactive.Subscribe(value => View.UpdateView(value)).AddTo(_disposables);
        }

        public void LoadFromPhoton() => Sync.LoadFromPhoton();

        public override void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            Sync.LoadFromPhoton();
        }
        
        private void OnDestroy()
        {
            _disposables.Dispose();
            Sync.Dispose();
        }
    }
}
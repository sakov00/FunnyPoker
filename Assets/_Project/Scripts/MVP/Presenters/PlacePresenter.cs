using _Project.Scripts.MVP.Models.Data;
using _Project.Scripts.MVP.Models.DataSync;
using _Project.Scripts.MVP.Views;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.MVP.Presenters
{
    public class PlacePresenter : MonoBehaviourPunCallbacks
    {
        private readonly CompositeDisposable _disposables = new ();
        
        [field: SerializeField] public PlaceData Data { get; set; }
        [field: SerializeField] private PlaceSync Sync { get; set; }
        [field: SerializeField] private PlaceView View { get; set; }

        public bool IsFree { get; set; }
        public bool IsEnabled { get; set; }
        public int PlayerActorNumber { get; set; }
        public bool IsSmallBlind { get; set; }
        public bool IsBigBlind { get; set; }

        private void Start()
        {
            Sync.IsFree.Subscribe(value => IsFree = value).AddTo(_disposables);
            Sync.IsEnabled.Subscribe(value => IsEnabled = value).AddTo(_disposables);
            Sync.PlayerActorNumber.Subscribe(value => PlayerActorNumber = value).AddTo(_disposables);
            Sync.IsSmallBlind.Subscribe(value => IsSmallBlind = value).AddTo(_disposables);
            Sync.IsBigBlind.Subscribe(value => IsBigBlind = value).AddTo(_disposables);
            
            Sync.Init(Data.Number); 
            Sync.IsEnabled.Subscribe(value => View.UpdateView(value)).AddTo(_disposables);
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
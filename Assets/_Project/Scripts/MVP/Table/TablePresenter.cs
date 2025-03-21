using System;
using System.Linq;
using _Project.Scripts.Managers;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MVP.Table
{
    public class TablePresenter : MonoBehaviourPunCallbacks
    {
        [Inject] private PlacesManager placesManager;
        
        private readonly CompositeDisposable disposable = new ();

        [SerializeField] private TableData data;
        [SerializeField] private TableSync sync;
        [SerializeField] private TableView view;
        
        public int Id => photonView.ViewID;
        public int MaxPlayerBet => placesManager.AllPlayerPlaces.Max(p => p.BettingMoney);
        
        public int Bank
        {
            get => sync.bank.Value;
            set => sync.bank.Value = value;
        }

        private void OnValidate()
        {
            view ??= GetComponent<TableView>();
        }

        private void Start()
        {
            sync.bank.Skip(1).Subscribe(value => SyncProperty(nameof(sync.bank), value)).AddTo(disposable);
        }

        private void SyncProperty(string propertyName, object value)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            
            Hashtable property = new() { { "Table" + propertyName, value } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(property);
        }
        
        public override void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            if (PhotonNetwork.IsMasterClient)
                return;
            
            if (changedProps.TryGetValue("Table" + nameof(sync.bank), out var owner))
                sync.bank.Value = (int)owner;
        }

        public void LoadFromPhoton()
        {
            var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
            if (roomProps.TryGetValue("Table" + nameof(sync.bank), out var owner))
                sync.bank.Value = (int)owner;
        }

        private void OnDestroy()
        {
            disposable.Dispose();
        }
    }
}
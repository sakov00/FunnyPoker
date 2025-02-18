using System;
using _Project.Scripts.Enums;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.MVP.Cards
{
    public class CardSync : MonoBehaviourPunCallbacks
    {
        [SerializeField] private CardData data;
        
        [SerializeField] private IntReactiveProperty ownerPlaceIdReactive = new ();
        
        private readonly CompositeDisposable _disposableProperties = new ();
        
        public IObservable<int> OwnerPlaceIdReactive  => ownerPlaceIdReactive;
        
        public int OwnerPlaceId
        {
            get => ownerPlaceIdReactive.Value;
            set => ownerPlaceIdReactive.Value = value;
        }
        
        public void Start()
        {
            OwnerPlaceIdReactive.Skip(1).Subscribe(value => SyncProperty(nameof(ownerPlaceIdReactive), value)).AddTo(_disposableProperties);
        }
        
        private void SyncProperty(string propertyName, object value)
        {
            Hashtable property = new() { { propertyName + data.Id, value } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(property);
        }
        
        public override void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            LoadFromPhoton();
        }

        public void LoadFromPhoton()
        {
            var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

            if (roomProps.TryGetValue(nameof(OwnerPlaceIdReactive) + data.Id, out var owner))
                ownerPlaceIdReactive.Value = (int)owner;
        }

        private void OnDestroy()
        {
            _disposableProperties.Dispose();
        }
    }
}
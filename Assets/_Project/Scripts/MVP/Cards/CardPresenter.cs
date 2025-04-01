using System;
using System.Linq;
using _Project.Scripts.Enums;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MVP.Cards
{
    public class CardPresenter : MonoBehaviourPunCallbacks
    {
        private readonly CompositeDisposable disposable = new ();

        [SerializeField] private CardData data;
        [SerializeField] private CardSync sync;
        [SerializeField] private CardView view;

        public int Id => photonView.ViewID;
        public PlayingCardRank Rank => data.rank;
        public PlayingCardSuit Suit => data.suit;
        
        public bool IsFree
        {
            get => sync.isFreeReactive.Value;
            set => sync.isFreeReactive.Value = value;
        }

        private void OnValidate()
        {
            data ??= GetComponent<CardData>();
            sync ??= GetComponent<CardSync>();
            view ??= GetComponent<CardView>();
        }
        
        private void Start()
        {
            sync.isFreeReactive.Skip(1).Subscribe(value => SyncProperty(nameof(sync.isFreeReactive), value)).AddTo(disposable);
        }

        public void UpdateCardPosition(Transform parent, Transform point)
            => view.UpdateCardPosition(parent, point);
        
        private void SyncProperty(string propertyName, object value)
        {
            if(!sync.isSyncData)
                return;
            
            Hashtable property = new() { { propertyName + Id, value } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(property);
        }
        
        public override void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            LoadFromPhoton(changedProps);
        }

        public void LoadFromPhoton(Hashtable properties = null)
        {
            properties ??= PhotonNetwork.CurrentRoom.CustomProperties;

            sync.isSyncData = false;
            
            if (properties.TryGetValue(nameof(sync.isFreeReactive) + Id, out var isFree))
                IsFree = (bool)isFree;
            
            sync.isSyncData = true;
        }

        private void OnDestroy()
        {
            disposable.Dispose();
        }
    }
}
using System.Collections.Generic;
using _Project.Scripts.MVP.Views;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.MVP.Place
{
    public class PlacePresenter : MonoBehaviourPunCallbacks
    {
        private readonly CompositeDisposable _disposable = new ();

        [SerializeField] private PlaceData data;
        [SerializeField] private PlaceSync sync;
        [SerializeField] private PlaceView view;
        
        public int Id => data.id;
        public PlacePresenter Previous => data.previous;
        public PlacePresenter Next => data.next;
        public Transform PlayerPoint => data.playerPoint;
        public Transform ParentCards => data.parentCards;
        public List<Transform> CardPoints => data.cardPoints;
        
        public bool IsFree
        {
            get => sync.isFreeReactive.Value;
            set => sync.isFreeReactive.Value = value;
        }
        
        public bool IsEnabled
        {
            get => sync.isEnabledReactive.Value;
            set => sync.isEnabledReactive.Value = value;
        }
        
        public int PlayerActorNumber
        {
            get => sync.playerActorNumberReactive.Value;
            set => sync.playerActorNumberReactive.Value = value;
        }
        
        public bool IsSmallBlind
        {
            get => sync.isSmallBlindReactive.Value;
            set => sync.isSmallBlindReactive.Value = value;
        }
        
        public bool IsBigBlind
        {
            get => sync.isBigBlindReactive.Value;
            set => sync.isBigBlindReactive.Value = value;
        }
        
        public IReactiveCollection<int> HandPlayingCards => sync.handPlayingCards;

        private void OnValidate()
        {
            data ??= GetComponent<PlaceData>();
            sync ??= GetComponent<PlaceSync>();
            view ??= GetComponent<PlaceView>();
        }

        private void Start()
        {            
            sync.isFreeReactive.Subscribe(value => SyncProperty(nameof(sync.isFreeReactive), value)).AddTo(_disposable);
            sync.isEnabledReactive.Subscribe(value =>
            {
                SyncProperty(nameof(sync.isEnabledReactive), value);
                view.UpdateButton(value);
            }).AddTo(_disposable);
            sync.playerActorNumberReactive.Subscribe(value => SyncProperty(nameof(sync.playerActorNumberReactive), value)).AddTo(_disposable);
            sync.isSmallBlindReactive.Subscribe(value => SyncProperty(nameof(sync.isSmallBlindReactive), value)).AddTo(_disposable);
            sync.isBigBlindReactive.Subscribe(value => SyncProperty(nameof(sync.isBigBlindReactive), value)).AddTo(_disposable);
            
            sync.handPlayingCards.ObserveAdd().Subscribe(addEvent => AddHandPlayingCard(addEvent.Value)).AddTo(_disposable);
            sync.handPlayingCards.ObserveRemove().Subscribe(removeEvent => RemoveHandPlayingCard(removeEvent.Value)).AddTo(_disposable);
        }
        
        private void SyncProperty(string propertyName, object value)
        {
            Hashtable property = new() { { propertyName + data.id, value } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(property);
        }
        
        public override void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            LoadFromPhoton();
        }

        public void LoadFromPhoton()
        {
            var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

            if (roomProps.TryGetValue(nameof(sync.isFreeReactive) + data.id, out var isFree))
                sync.isFreeReactive.Value = (bool)isFree;

            if (roomProps.TryGetValue(nameof(sync.isEnabledReactive) + data.id, out var isEnabled))
                sync.isEnabledReactive.Value = (bool)isEnabled;

            if (roomProps.TryGetValue(nameof(sync.playerActorNumberReactive) + data.id, out var actorNumber))
                sync.playerActorNumberReactive.Value = (int)actorNumber;

            if (roomProps.TryGetValue(nameof(sync.isSmallBlindReactive) + data.id, out var isSmallBlind))
                sync.isSmallBlindReactive.Value = (bool)isSmallBlind;

            if (roomProps.TryGetValue(nameof(sync.isBigBlindReactive) + data.id, out var isBigBlind))
                sync.isBigBlindReactive.Value = (bool)isBigBlind;
            
            //TODO sync HandPlayingCards when player join to room for reconnect
        }
        
        private void AddHandPlayingCard(int value)
        {
            if(PhotonNetwork.IsMasterClient)
                photonView?.RPC("SyncAddHandPlayingCardRPC", RpcTarget.Others, value);
        }
        
        private void RemoveHandPlayingCard(int value)
        {
            if(PhotonNetwork.IsMasterClient)
                photonView?.RPC("SyncRemoveHandPlayingCardRPC", RpcTarget.Others, value);
        }
        
        [PunRPC]
        private void SyncAddHandPlayingCardRPC(int addedCardId)
        {
            sync.handPlayingCards.Add(addedCardId);
        }
        
        [PunRPC]
        private void SyncRemoveHandPlayingCardRPC(int removedCardId)
        {
            sync.handPlayingCards.Remove(removedCardId);
        }
        
        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}
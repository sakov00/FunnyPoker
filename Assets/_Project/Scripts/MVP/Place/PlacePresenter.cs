using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.MVP.Table;
using _Project.Scripts.MVP.Views;
using _Project.Scripts.Services;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MVP.Place
{
    public class PlacePresenter : MonoBehaviourPunCallbacks
    {
        [Inject] private CardsService cardsService;
        [Inject] private TablePresenter tablePresenter;
        
        private readonly CompositeDisposable disposable = new ();

        [SerializeField] private PlaceData data;
        [SerializeField] private PlaceSync sync;
        [SerializeField] private PlaceView view;
        
        public int Id => data.id;
        public PlacePresenter Previous => data.previous;
        public PlacePresenter Next => data.next;
        public Transform PlayerPoint => data.playerPoint;
        public Transform PlayerCardsParent => data.playerCardsParent;
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
        
        public int Money
        {
            get => sync.moneyReactive.Value;
            set => sync.moneyReactive.Value = value;
        }
        
        public int BettingMoney
        {
            get => sync.bettingMoneyReactive.Value;
            set
            {
                Money -= value - sync.bettingMoneyReactive.Value;
                tablePresenter.Bank += value - sync.bettingMoneyReactive.Value;
                sync.bettingMoneyReactive.Value = value;
            }
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
            sync.isFreeReactive.Subscribe(value => SyncProperty(nameof(sync.isFreeReactive), value)).AddTo(disposable);
            sync.isEnabledReactive.Subscribe(value =>
            {
                SyncProperty(nameof(sync.isEnabledReactive), value);
                view.UpdateButton(value);
            }).AddTo(disposable);
            sync.playerActorNumberReactive.Subscribe(value => SyncProperty(nameof(sync.playerActorNumberReactive), value)).AddTo(disposable);
            
            sync.moneyReactive.Subscribe(value => SyncProperty(nameof(sync.moneyReactive), value)).AddTo(disposable);
            sync.bettingMoneyReactive.Subscribe(value => SyncProperty(nameof(sync.bettingMoneyReactive), value)).AddTo(disposable);
            
            sync.isSmallBlindReactive.Subscribe(value => SyncProperty(nameof(sync.isSmallBlindReactive), value)).AddTo(disposable);
            sync.isBigBlindReactive.Subscribe(value => SyncProperty(nameof(sync.isBigBlindReactive), value)).AddTo(disposable);
            
            sync.handPlayingCards.ObserveAdd().Subscribe(addEvent => AddHandPlayingCard(addEvent.Value)).AddTo(disposable);
            sync.handPlayingCards.ObserveRemove().Subscribe(removeEvent => RemoveHandPlayingCard(removeEvent.Value)).AddTo(disposable);
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
            
            if (roomProps.TryGetValue(nameof(sync.moneyReactive) + data.id, out var money))
                sync.moneyReactive.Value = (int)money;
            
            if (roomProps.TryGetValue(nameof(sync.bettingMoneyReactive) + data.id, out var bettingMoney))
                sync.bettingMoneyReactive.Value = (int)bettingMoney;

            if (roomProps.TryGetValue(nameof(sync.isSmallBlindReactive) + data.id, out var isSmallBlind))
                sync.isSmallBlindReactive.Value = (bool)isSmallBlind;

            if (roomProps.TryGetValue(nameof(sync.isBigBlindReactive) + data.id, out var isBigBlind))
                sync.isBigBlindReactive.Value = (bool)isBigBlind;
            
            //TODO sync HandPlayingCards when player join to room for reconnect
        }
        
        private void AddHandPlayingCard(int value)
        {
            if(PhotonNetwork.IsMasterClient)
                photonView?.RPC("SyncAddHandPlayingCardRPC", RpcTarget.All, value);
        }
        
        private void RemoveHandPlayingCard(int value)
        {
            if(PhotonNetwork.IsMasterClient)
                photonView?.RPC("SyncRemoveHandPlayingCardRPC", RpcTarget.All, value);
        }
        
        [PunRPC]
        private void SyncAddHandPlayingCardRPC(int addedCardId)
        {
            sync.handPlayingCards.Add(addedCardId);
            int cardPlaceIndex = HandPlayingCards.IndexOf(addedCardId);
            var movedCard = cardsService.PlayingCards.First(card => card.Id == addedCardId);
            movedCard.UpdateCardPosition(PlayerCardsParent, CardPoints[cardPlaceIndex]);
        }
        
        [PunRPC]
        private void SyncRemoveHandPlayingCardRPC(int removedCardId)
        {
            sync.handPlayingCards.Remove(removedCardId);
            int cardPlaceIndex = HandPlayingCards.IndexOf(removedCardId);
            var movedCard = cardsService.PlayingCards.First(card => card.Id == removedCardId);
            movedCard.UpdateCardPosition(cardsService.DealerCardsParent, CardPoints[cardPlaceIndex]);
        }
        
        private void OnDestroy()
        {
            disposable.Dispose();
        }
    }
}
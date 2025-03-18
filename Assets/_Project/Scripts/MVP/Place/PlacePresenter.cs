using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Managers;
using _Project.Scripts.MVP.Table;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MVP.Place
{
    public class PlacePresenter : MonoBehaviourPunCallbacks
    {
        [Inject] private CardsManager cardsManager;
        [Inject] private TablePresenter tablePresenter;
        
        private readonly CompositeDisposable disposable = new ();

        [SerializeField] private PlaceData data;
        [SerializeField] private PlaceSync sync;
        [SerializeField] private PlaceView view;
        
        public int Id => photonView.ViewID;
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
            set
            {
                if (IsFolded)
                {
                    Next.IsEnabled = value;
                }
                else
                {
                    sync.isEnabledReactive.Value = value;
                }
            }
        }

        public bool IsFolded
        {
            get => sync.isFoldedReactive.Value;
            set => sync.isFoldedReactive.Value = value;
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
            sync.isFreeReactive.Skip(1).Subscribe(value => SyncProperty(nameof(sync.isFreeReactive), value)).AddTo(disposable);
            sync.isEnabledReactive.Skip(1).Subscribe(value =>
            {
                SyncProperty(nameof(sync.isEnabledReactive), value);
                view.UpdateButton(value);
            }).AddTo(disposable);
            sync.isFoldedReactive.Skip(1).Subscribe(value => SyncProperty(nameof(sync.isFoldedReactive), value)).AddTo(disposable);
            
            sync.playerActorNumberReactive.Skip(1).Subscribe(value => SyncProperty(nameof(sync.playerActorNumberReactive), value)).AddTo(disposable);
            
            sync.moneyReactive.Skip(1).Subscribe(value => SyncProperty(nameof(sync.moneyReactive), value)).AddTo(disposable);
            sync.bettingMoneyReactive.Skip(1).Subscribe(value => SyncProperty(nameof(sync.bettingMoneyReactive), value)).AddTo(disposable);
            
            sync.isSmallBlindReactive.Skip(1).Subscribe(value => SyncProperty(nameof(sync.isSmallBlindReactive), value)).AddTo(disposable);
            sync.isBigBlindReactive.Skip(1).Subscribe(value => SyncProperty(nameof(sync.isBigBlindReactive), value)).AddTo(disposable);
            
            sync.handPlayingCards.ObserveAdd().Skip(1).Subscribe(addEvent => AddHandPlayingCard(addEvent.Value)).AddTo(disposable);
            sync.handPlayingCards.ObserveRemove().Skip(1).Subscribe(removeEvent => RemoveHandPlayingCard(removeEvent.Value)).AddTo(disposable);
        }
        
        private void SyncProperty(string propertyName, object value)
        {
            if(!sync.isSyncData)
                return;
            
            Hashtable property = new() { { propertyName + Id, value } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(property);
        }
        
        public override void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            LoadFromPhoton();
        }

        public void LoadFromPhoton()
        {
            var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

            sync.isSyncData = false;
            
            if (roomProps.TryGetValue(nameof(sync.isFreeReactive) + Id, out var isFree))
                sync.isFreeReactive.Value = (bool)isFree;

            if (roomProps.TryGetValue(nameof(sync.isEnabledReactive) + Id, out var isEnabled))
                sync.isEnabledReactive.Value = (bool)isEnabled;
            
            if (roomProps.TryGetValue(nameof(sync.isFoldedReactive) + Id, out var isFolded))
                sync.isFoldedReactive.Value = (bool)isFolded;

            if (roomProps.TryGetValue(nameof(sync.playerActorNumberReactive) + Id, out var actorNumber))
                sync.playerActorNumberReactive.Value = (int)actorNumber;
            
            if (roomProps.TryGetValue(nameof(sync.moneyReactive) + Id, out var money))
                sync.moneyReactive.Value = (int)money;
            
            if (roomProps.TryGetValue(nameof(sync.bettingMoneyReactive) + Id, out var bettingMoney))
                sync.bettingMoneyReactive.Value = (int)bettingMoney;

            if (roomProps.TryGetValue(nameof(sync.isSmallBlindReactive) + Id, out var isSmallBlind))
                sync.isSmallBlindReactive.Value = (bool)isSmallBlind;

            if (roomProps.TryGetValue(nameof(sync.isBigBlindReactive) + Id, out var isBigBlind))
                sync.isBigBlindReactive.Value = (bool)isBigBlind;
            
            sync.isSyncData = true;
            
            //TODO sync HandPlayingCards when player join to room for reconnect
        }
        
        private void AddHandPlayingCard(int value)
        {
            photonView?.RPC("SyncAddHandPlayingCardRPC", RpcTarget.All, value);
        }
        
        private void RemoveHandPlayingCard(int value)
        {
            photonView?.RPC("SyncRemoveHandPlayingCardRPC", RpcTarget.All, value);
        }
        
        [PunRPC]
        private void SyncAddHandPlayingCardRPC(int addedCardId)
        {
            if (!PhotonNetwork.IsMasterClient)
                sync.handPlayingCards.Add(addedCardId);
            
            int cardPlaceIndex = HandPlayingCards.IndexOf(addedCardId);
            var movedCard = cardsManager.AllPlayingCards.First(card => card.Id == addedCardId);
            movedCard.UpdateCardPosition(PlayerCardsParent, CardPoints[cardPlaceIndex]);
        }
        
        [PunRPC]
        private void SyncRemoveHandPlayingCardRPC(int removedCardId)
        {
            if (!PhotonNetwork.IsMasterClient)
                sync.handPlayingCards.Remove(removedCardId);
            
            int cardPlaceIndex = HandPlayingCards.IndexOf(removedCardId);
            var movedCard = cardsManager.AllPlayingCards.First(card => card.Id == removedCardId);
            movedCard.UpdateCardPosition(cardsManager.DealerCardsParent, CardPoints[cardPlaceIndex]);
        }
        
        private void OnDestroy()
        {
            disposable.Dispose();
        }
    }
}
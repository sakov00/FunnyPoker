using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.Managers;
using _Project.Scripts.MVP.Table;
using _Project.Scripts.Services;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MVP.Place
{
    public class PlacePresenter : MonoBehaviourPun
    {
        [Inject] private GameData gameData;
        [Inject] private RoundService roundService;
        [Inject] private SyncData syncData;
        
        private readonly CompositeDisposable disposable = new ();

        [SerializeField] private PlaceData data;
        [SerializeField] private PlaceSync sync;
        [SerializeField] private PlaceView view;
        
        public int Id => photonView.ViewID;
        public string ObjectName => nameof(PlacePresenter) + Id;
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
                    if (value)
                        roundService.CheckRoundEnd(this);
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
                gameData.TablePresenter.Bank += value - sync.bettingMoneyReactive.Value;
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
            var syncPropertiesBool = new Dictionary<ReactiveProperty<bool>, string>
            {
                { sync.isFreeReactive, nameof(IsFree) },
                { sync.isEnabledReactive, nameof(IsEnabled) },
                { sync.isFoldedReactive, nameof(IsFolded) },
                { sync.isSmallBlindReactive, nameof(IsSmallBlind) },
                { sync.isBigBlindReactive, nameof(IsBigBlind) }
            };
            var syncPropertiesInt = new Dictionary<ReactiveProperty<int>, string>
            {
                { sync.playerActorNumberReactive, nameof(PlayerActorNumber) },
                { sync.moneyReactive, nameof(Money) },
                { sync.bettingMoneyReactive, nameof(BettingMoney) },
            };

            BindSyncProperties(syncPropertiesBool);
            BindSyncProperties(syncPropertiesInt);
            
            sync.isEnabledReactive
                .Subscribe(value => view.UpdateButton(value))
                .AddTo(disposable);
            
            sync.handPlayingCards.ObserveAdd().Subscribe(addEvent => AddHandPlayingCard(addEvent.Value)).AddTo(disposable);
            sync.handPlayingCards.ObserveRemove().Subscribe(removeEvent => RemoveHandPlayingCard(removeEvent.Value)).AddTo(disposable);
        }
        
        private void BindSyncProperties<T>(Dictionary<ReactiveProperty<T>, string> properties)
        {
            foreach (var pair in properties)
            {
                pair.Key
                    .Skip(1)
                    .Subscribe(value => syncData.SyncProperty(ObjectName, pair.Value, value))
                    .AddTo(disposable);
            }
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
            if(!sync.handPlayingCards.Contains(addedCardId))
                sync.handPlayingCards.Add(addedCardId);
            
            int cardPlaceIndex = HandPlayingCards.IndexOf(addedCardId);
            var movedCard = gameData.AllPlayingCards.First(card => card.Id == addedCardId);
            movedCard.UpdateCardPosition(PlayerCardsParent, CardPoints[cardPlaceIndex]);
        }
        
        [PunRPC]
        private void SyncRemoveHandPlayingCardRPC(int removedCardId)
        {
            if(!sync.handPlayingCards.Contains(removedCardId))
                sync.handPlayingCards.Remove(removedCardId);
            
            int cardPlaceIndex = HandPlayingCards.IndexOf(removedCardId);
            var movedCard = gameData.AllPlayingCards.First(card => card.Id == removedCardId);
            movedCard.UpdateCardPosition(gameData.DealerCardsParent, CardPoints[cardPlaceIndex]);
        }
        
        private void OnDestroy()
        {
            disposable.Dispose();
        }
    }
}
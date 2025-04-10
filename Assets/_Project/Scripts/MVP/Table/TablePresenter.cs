using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.Managers;
using _Project.Scripts.Services;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MVP.Table
{
    public class TablePresenter : MonoBehaviourPun
    {
        [Inject] private GameData gameData;
        [Inject] private DataSync dataSync;
        
        private readonly CompositeDisposable disposable = new ();

        [SerializeField] private TableData data;
        [SerializeField] private TableSync sync;
        [SerializeField] private TableView view;
        
        public int Id => photonView.ViewID;
        public string ObjectName => nameof(TablePresenter) + Id;
        public Transform CardsParent => data.cardsParent;
        public List<Transform> CardPoints => data.cardPoints;
        public int MaxPlayerBet => gameData.AllPlayerPlaces.Max(p => p.BettingMoney);
        
        public int Bank
        {
            get => sync.bank.Value;
            set => sync.bank.Value = value;
        }
        
        public IReactiveCollection<int> PlayingCards => sync.playingCards;

        private void OnValidate()
        {
            data ??= GetComponent<TableData>();
            sync ??= GetComponent<TableSync>();
            view ??= GetComponent<TableView>();
        }

        public void ShowCard(int indexCard)
        {
            var cardId = PlayingCards[indexCard];
            view.ShowCard(cardId);
        }

        private void Start()
        {
            sync.bank
                .Skip(1)
                .Subscribe(value => dataSync.SyncProperty(ObjectName, nameof(Bank), value))
                .AddTo(disposable);
            
            sync.playingCards.ObserveAdd().Subscribe(addEvent => AddPlayingCard(addEvent.Value)).AddTo(disposable);
            sync.playingCards.ObserveRemove().Subscribe(removeEvent => RemovePlayingCard(removeEvent.Value)).AddTo(disposable);
            
            sync.playingCards
                .ObserveAdd()
                .Subscribe(addEvent =>
                {
                    AddPlayingCard(addEvent.Value);
                    dataSync.SyncProperty(ObjectName, nameof(PlayingCards), PlayingCards.ToArray());
                })
                .AddTo(disposable);

            sync.playingCards
                .ObserveRemove()
                .Subscribe(removeEvent =>
                {
                    RemovePlayingCard(removeEvent.Value);
                    dataSync.SyncProperty(ObjectName, nameof(PlayingCards), PlayingCards.ToArray());
                })
                .AddTo(disposable);
        }
        
        private void AddPlayingCard(int value)
        {
            int cardPlaceIndex = PlayingCards.IndexOf(value);
            var movedCard = gameData.AllPlayingCards.First(card => card.Id == value);
            movedCard.UpdateCardPosition(CardsParent, CardPoints[cardPlaceIndex]);
        }
        
        private void RemovePlayingCard(int value)
        {
            // int cardPlaceIndex = HandPlayingCards.IndexOf(value);
            // var movedCard = gameData.AllPlayingCards.First(card => card.Id == value);
            // movedCard.UpdateCardPosition(gameData.DealerCardsParent, CardPoints[cardPlaceIndex]);
        }

        private void OnDestroy()
        {
            disposable.Dispose();
        }
    }
}
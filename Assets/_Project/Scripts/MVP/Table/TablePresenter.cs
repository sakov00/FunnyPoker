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

        private void Start()
        {
            sync.bank
                .Skip(1)
                .Subscribe(value => dataSync.SyncProperty(ObjectName, nameof(Bank), value))
                .AddTo(disposable);
            
            sync.playingCards.ObserveAdd().Subscribe(addEvent => AddHandPlayingCard(addEvent.Value)).AddTo(disposable);
            sync.playingCards.ObserveRemove().Subscribe(removeEvent => RemoveHandPlayingCard(removeEvent.Value)).AddTo(disposable);
        }

        public void ThrowCards()
        {
            
        }
        
        private void AddHandPlayingCard(int value)
        {
            photonView?.RPC("SyncAddHandPlayingCardRPC", RpcTarget.Others, value);
        }
        
        private void RemoveHandPlayingCard(int value)
        {
            photonView?.RPC("SyncRemoveHandPlayingCardRPC", RpcTarget.Others, value);
        }
        
        [PunRPC]
        private void SyncAddHandPlayingCardRPC(int addedCardId)
        {
            if(!sync.playingCards.Contains(addedCardId))
                sync.playingCards.Add(addedCardId);
            
            int cardPlaceIndex = PlayingCards.IndexOf(addedCardId);
            var movedCard = gameData.AllPlayingCards.First(card => card.Id == addedCardId);
            movedCard.UpdateCardPosition(CardsParent, CardPoints[cardPlaceIndex]);
        }
        
        [PunRPC]
        private void SyncRemoveHandPlayingCardRPC(int removedCardId)
        {
            if(!sync.playingCards.Contains(removedCardId))
                sync.playingCards.Remove(removedCardId);
            
            int cardPlaceIndex = PlayingCards.IndexOf(removedCardId);
            var movedCard = gameData.AllPlayingCards.First(card => card.Id == removedCardId);
            movedCard.UpdateCardPosition(gameData.DealerCardsParent, CardPoints[cardPlaceIndex]);
        }

        private void OnDestroy()
        {
            disposable.Dispose();
        }
    }
}
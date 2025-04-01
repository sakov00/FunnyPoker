using System;
using System.Collections.Generic;
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
        [Inject] private CardsManager cardsManager;
        
        private readonly CompositeDisposable disposable = new ();

        [SerializeField] private TableData data;
        [SerializeField] private TableSync sync;
        [SerializeField] private TableView view;
        
        public int Id => photonView.ViewID;
        public Transform CardsParent => data.cardsParent;
        public List<Transform> CardPoints => data.cardPoints;
        public int MaxPlayerBet => placesManager.AllPlayerPlaces.Max(p => p.BettingMoney);
        
        public int Bank
        {
            get => sync.bank.Value;
            set => sync.bank.Value = value;
        }
        
        public IReactiveCollection<int> PlayingCards => sync.playingCards;

        private void OnValidate()
        {
            view ??= GetComponent<TableView>();
        }

        private void Start()
        {
            sync.bank.Skip(1).Subscribe(value => SyncProperty(nameof(sync.bank), value)).AddTo(disposable);
            
            sync.playingCards.ObserveAdd().Subscribe(addEvent => AddHandPlayingCard(addEvent.Value)).AddTo(disposable);
            sync.playingCards.ObserveRemove().Subscribe(removeEvent => RemoveHandPlayingCard(removeEvent.Value)).AddTo(disposable);
        }

        public void ThrowCards()
        {
            
        }

        private void SyncProperty(string propertyName, object value)
        {
            if(!sync.isSyncData)
                return;
            
            Hashtable property = new() { { "Table" + propertyName, value } };
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
            
            if (properties.TryGetValue("Table" + nameof(sync.bank), out var owner))
                sync.bank.Value = (int)owner;
            
            sync.isSyncData = true;
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
            var movedCard = cardsManager.AllPlayingCards.First(card => card.Id == addedCardId);
            movedCard.UpdateCardPosition(CardsParent, CardPoints[cardPlaceIndex]);
        }
        
        [PunRPC]
        private void SyncRemoveHandPlayingCardRPC(int removedCardId)
        {
            if(!sync.playingCards.Contains(removedCardId))
                sync.playingCards.Remove(removedCardId);
            
            int cardPlaceIndex = PlayingCards.IndexOf(removedCardId);
            var movedCard = cardsManager.AllPlayingCards.First(card => card.Id == removedCardId);
            movedCard.UpdateCardPosition(cardsManager.DealerCardsParent, CardPoints[cardPlaceIndex]);
        }

        private void OnDestroy()
        {
            disposable.Dispose();
        }
    }
}
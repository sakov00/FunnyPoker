using System;
using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MVP.Cards
{
    public class CardPresenter : MonoBehaviourPunCallbacks
    {
        [Inject] private PlacesManager _placesManager;
        [Inject] private CardsService _cardsService;
        
        private readonly CompositeDisposable _disposable = new ();

        [SerializeField] private CardData data;
        [SerializeField] private CardSync sync;
        [SerializeField] private CardView view;
        
        public int Id => data.id;
        public PlayingCardRank Rank => data.rank;
        public PlayingCardSuit Suit => data.suit;

        public int OwnerPlaceId
        {
            get => sync.ownerPlaceIdReactive.Value;
            set => sync.ownerPlaceIdReactive.Value = value;
        }

        private void OnValidate()
        {
            data ??= GetComponent<CardData>();
            sync ??= GetComponent<CardSync>();
            view ??= GetComponent<CardView>();
        }

        private void Start()
        {
            sync.ownerPlaceIdReactive.
                Subscribe(value =>
                {
                    SyncProperty(nameof(sync.ownerPlaceIdReactive), value);
                    UpdateCardOwner();
                }).AddTo(_disposable);
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

            if (roomProps.TryGetValue(nameof(sync.ownerPlaceIdReactive) + data.id, out var owner))
                sync.ownerPlaceIdReactive.Value = (int)owner;
        }

        private void UpdateCardOwner()
        {
            var ownerPlace = _placesManager.AllPlayerPlaces
                .FirstOrDefault(place => place.HandPlayingCards.Contains(data.id));
            
            if (ownerPlace == null)
                return;
            
            int cardPlaceIndex = ownerPlace.HandPlayingCards.IndexOf(data.id);
            view.UpdateCardOwner(ownerPlace.ParentCards, ownerPlace.CardPoints[cardPlaceIndex]);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}
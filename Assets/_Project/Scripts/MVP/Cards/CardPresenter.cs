using System;
using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MVP.Cards
{
    public class CardPresenter : MonoBehaviourPun
    {
        [Inject] private PlacesManager _placesManager;
        [Inject] private CardsService _cardsService;
        
        private readonly CompositeDisposable _disposables = new ();
        
        [field: SerializeField] public CardData Data { get; private set; }
        [field: SerializeField] public CardSync Sync { get; private set; }
        [field: SerializeField] private CardView View { get; set; }

        private void OnValidate()
        {
            if (Data == null)
                Data = GetComponent<CardData>();
            if (Sync == null)
                Sync = GetComponent<CardSync>();
            if (View == null)
                View = GetComponent<CardView>();
        }

        private void Start()
        {
            Sync.OwnerPlaceIdReactive.Subscribe(value => UpdateCardOwner()).AddTo(_disposables);
        }

        private void UpdateCardOwner()
        {
            var ownerPlace = _placesManager.AllPlayerPlaces
                .FirstOrDefault(place => place.Sync.PlayingCardIdsInHand.Contains(Data.Id));
            
            if (ownerPlace == null)
                return;
            
            int cardPlaceIndex = ownerPlace.Sync.PlayingCardIdsInHand.IndexOf(Data.Id);
            View.UpdateCardOwner(ownerPlace.Data.ParentCards, ownerPlace.Data.CardPoints[cardPlaceIndex]);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.MVP.Cards;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.Services
{
    public class CardsService : MonoBehaviourPun
    {
        [Inject] private PlacesManager _placesManager;
        
        [field: SerializeField] public List<CardPresenter> PlayingCards { get; private set; }
        
        public void DealTwoCardsToPlayers()
        {
            DealCardToPlayers();
            DealCardToPlayers();
        }
        
        private void DealCardToPlayers()
        {
            foreach (var place in _placesManager.AllPlayerPlaces)
            {
                var card = GetRandomPlayingCard();
                card.gameObject.SetActive(true);
                place.HandPlayingCards.Add(card.Id);
                card.OwnerPlaceId = place.Id;
            }
        }
        
        private CardPresenter GetRandomPlayingCard()
        {
            if (PlayingCards.Count == 0)
                return null;

            var index = Random.Range(0, PlayingCards.Count);
            var card = PlayingCards[index];
            PlayingCards.RemoveAt(index);
            return card;
        }
    }
}
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
        
        [SerializeField] private List<CardPresenter> playingCards = new ();
        
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
                place.Sync.PlayingCardIdsInHand.Add(card.Data.Id);
                card.Sync.OwnerPlaceId = place.Data.Id;
            }
        }
        
        private CardPresenter GetRandomPlayingCard()
        {
            if (playingCards.Count == 0)
                return null;

            var index = Random.Range(0, playingCards.Count);
            var card = playingCards[index];
            playingCards.RemoveAt(index);
            return card;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Data;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.Services
{
    public class DeskService : MonoBehaviour
    {
        [Inject] private PlacesManager _placesManager;
        
        [SerializeField] private List<PlayingCard> playingCards = new ();
        [SerializeField] private Transform playingCardsParent;
        
        public void DealCards()
        {
            foreach (var place in _placesManager.AllPlayerPlaces)
            {
                var card = GetRandomPlayingCard();
                place.PlayingCards.Add(card);
                // card.transform.SetParent(place.CardsPoint);
                card.transform.position = place.CardsPoint.position;
            }
        }

        private PlayingCard GetRandomPlayingCard()
        {
            return playingCards.FirstOrDefault();
        }
    }
}
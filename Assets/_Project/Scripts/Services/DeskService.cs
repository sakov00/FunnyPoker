using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Data;
using Photon.Pun;
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
        
        public void DealCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var place in _placesManager.AllPlayerPlaces)
                {
                    var card = GetRandomPlayingCard();
                    place.PlayingCards.Add(card);
                    card.transform.SetParent(place.CardsParent);
                    card.transform.localPosition = place.CardPoints[i].localPosition;
                    card.transform.localRotation = place.CardPoints[i].localRotation;
                }
            }
        }

        private PlayingCard GetRandomPlayingCard()
        {
            var card = playingCards.FirstOrDefault();
            playingCards.Remove(card);
            return card;
        }
    }
}
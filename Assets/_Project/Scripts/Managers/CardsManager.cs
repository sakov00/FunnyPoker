using System;
using System.Collections.Generic;
using _Project.Scripts.MVP.Cards;
using Photon.Pun;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Managers
{
    public class CardsManager : MonoBehaviour
    {
        [Inject] private PlacesManager placesManager;
        
        [field: SerializeField] public Transform DealerCardsParent { get; private set; }
        [field: SerializeField] public List<CardPresenter> AllPlayingCards { get; private set; } = new();
        [field: SerializeField] public List<CardPresenter> CurrentPlayingCards { get; private set; } = new();

        private void Start()
        {
            CurrentPlayingCards.AddRange(AllPlayingCards);
        }

        public void DealTwoCardsToPlayers()
        {
            DealCardToPlayers();
            DealCardToPlayers();
        }
        
        private void DealCardToPlayers()
        {
            foreach (var place in placesManager.AllPlayerPlaces)
            {
                var card = GetRandomPlayingCard();
                card.gameObject.SetActive(true);
                place.HandPlayingCards.Add(card.Id);
            }
        }
        
        private CardPresenter GetRandomPlayingCard()
        {
            if (CurrentPlayingCards.Count == 0)
                return null;

            var index = Random.Range(0, CurrentPlayingCards.Count);
            var card = CurrentPlayingCards[index];
            CurrentPlayingCards.RemoveAt(index);
            return card;
        }
    }
}
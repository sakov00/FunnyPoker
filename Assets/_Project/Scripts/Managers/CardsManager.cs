using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.MVP.Cards;
using Photon.Pun;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Managers
{
    public class CardsManager
    {
        [Inject] private GameData gameData;

        public void DealTwoCardsToPlayers()
        {
            DealCardToPlayers();
            DealCardToPlayers();
        }
        
        private void DealCardToPlayers()
        {
            foreach (var place in gameData.AllPlayerPlaces)
            {
                var card = GetRandomPlayingCard();
                card.gameObject.SetActive(true);
                place.HandPlayingCards.Add(card.Id);
            }
        }
        
        private CardPresenter GetRandomPlayingCard()
        {
            var freePlayingCards = gameData.AllPlayingCards.Where(x => x.IsFree).ToList();
            if (!freePlayingCards.Any())
                return null;

            var index = Random.Range(0, freePlayingCards.Count);
            var card = freePlayingCards[index];
            card.IsFree = false;
            return card;
        }
    }
}
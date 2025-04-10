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
        
        public void DealCardToTable()
        {
            var table = gameData.TablePresenter;
            var cards = GetRandomPlayingCard(5);
            foreach (var card in cards)
                table.PlayingCards.Add(card.Id);
        }
        
        public void DealCardToPlayers()
        {
            foreach (var place in gameData.AllPlayerPlaces)
            {
                var cards = GetRandomPlayingCard(2);
                foreach (var card in cards)
                    place.HandPlayingCards.Add(card.Id);
            }
        }
        
        private List<CardPresenter> GetRandomPlayingCard(int count)
        {
            var listCards = new List<CardPresenter>();
            for (int i = 0; i < count; i++)
            {
                var freePlayingCards = gameData.AllPlayingCards.Where(x => x.IsFree).ToList();
                if (!freePlayingCards.Any())
                    return null;

                var index = Random.Range(0, freePlayingCards.Count);
                var card = freePlayingCards[index];
                card.IsFree = false;
                card.gameObject.SetActive(true);
                listCards.Add(card);
            }
            return listCards;
        }
    }
}
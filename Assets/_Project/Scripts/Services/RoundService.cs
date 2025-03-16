using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Managers;
using _Project.Scripts.MVP.Place;
using _Project.Scripts.MVP.Table;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Services
{
    public class RoundService
    {
        [Inject] private PlacesManager placesManager;
        [Inject] private TablePresenter tablePresenter;
        [Inject] private GameStateManager gameStateManager;
        
        private List<PlacePresenter> orderPlaces = new ();
        
        public void CheckRoundEnd()
        {
            var activePlayers = placesManager.AllPlayerPlaces.Where(p => !p.IsFolded).ToList();

            if (activePlayers.Count == 1)
            {
                //EndGame(activePlayers[0]); // Единственный игрок выигрывает
                return;
            }

            bool allBetsEqual = activePlayers.All(p => p.BettingMoney == tablePresenter.MaxPlayerBet);

            if (allBetsEqual)
            {
                gameStateManager.Next();
            }
        }

        public void SetOrderPlaces(PlacePresenter startPlace)
        {
            orderPlaces.Clear();

            var allPlaces = placesManager.AllPlayerPlaces;
            int startIndex = allPlaces.IndexOf(startPlace);

            if (startIndex == -1)
                return;

            for (int i = 0; i < allPlaces.Count; i++)
            {
                int index = (startIndex + i) % allPlaces.Count;
                orderPlaces.Add(allPlaces[index]);
                Debug.Log($"Добавлено место: {index}");
            }
        }
    }
}
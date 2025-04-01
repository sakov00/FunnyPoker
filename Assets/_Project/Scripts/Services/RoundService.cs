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
        
        public void CheckRoundEnd(PlacePresenter place)
        {
            if (place.IsBigBlind)
            {
                var activePlayers = placesManager.AllPlayerPlaces.Where(p => !p.IsFolded).ToList();
                var allBetsEqual = activePlayers.All(p => p.BettingMoney == tablePresenter.MaxPlayerBet);

                if (allBetsEqual)
                {
                    gameStateManager.Next();
                }
            }
        }
    }
}
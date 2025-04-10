using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.Managers;
using _Project.Scripts.MVP.Place;
using _Project.Scripts.MVP.Table;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Services
{
    public class RoundService
    {
        [Inject] private GameData gameData;
        [Inject] private GameStateManager gameStateManager;
        
        public void CheckRoundEnd()
        {
            foreach (var place in gameData.AllPlayerPlaces)
            {
                if (!place.IsBigBlind || !place.IsEnabled)
                    continue;
                
                var activePlayers = gameData.AllPlayerPlaces.Where(p => !p.IsFolded).ToList();
                var allBetsEqual = activePlayers.All(p => p.BettingMoney == gameData.TablePresenter.MaxPlayerBet);

                if (allBetsEqual)
                {
                    gameStateManager.Next();
                }
            }
        }
    }
}
using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using _Project.Scripts.MVP.Table;
using _Project.Scripts.Services;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameStates
{
    public class PreflopState : IGameState
    {
        [Inject] private GameData gameData;
        
        public void EnterState()
        {
            Debug.Log("Игроки делают ставки PreflopState");
            
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            var random = Random.Range(0, gameData.AllPlayerPlaces.Count);
            var placeInfo = gameData.AllPlayerPlaces.ElementAt(random);
            
            placeInfo.IsSmallBlind = true;
            placeInfo.GlobalBettingMoney = 5;
            
            placeInfo.Next.IsBigBlind = true;
            placeInfo.Next.GlobalBettingMoney = 10;
            
            placeInfo.Next.Next.IsEnabled = true;

            gameData.TablePresenter.ThrowCards();
        }

        public void ExitState()
        {
            gameData.AllPlayerPlaces.ForEach(place => place.IsEnabled = false);

            Debug.Log("Ставки приняты PreflopState");
        }
    }
}
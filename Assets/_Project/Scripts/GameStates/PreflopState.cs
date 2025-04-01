using System.Linq;
using _Project.Scripts.Enums;
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
        [Inject] private PlacesManager placesManager;
        [Inject] private TablePresenter tablePresenter;
        
        public void EnterState()
        {
            Debug.Log("Игроки делают ставки PreflopState");
            
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            var random = Random.Range(0, placesManager.AllPlayerPlaces.Count);
            var placeInfo = placesManager.AllPlayerPlaces.ElementAt(random);
            
            placeInfo.IsSmallBlind = true;
            placeInfo.BettingMoney = 5;
            
            placeInfo.Next.IsBigBlind = true;
            placeInfo.Next.BettingMoney = 10;
            
            placeInfo.Next.Next.IsEnabled = true;

            tablePresenter.ThrowCards();
        }

        public void ExitState()
        {
            placesManager.AllPlayerPlaces.ForEach(place => place.IsEnabled = false);

            Debug.Log("Ставки приняты PreflopState");
        }
    }
}
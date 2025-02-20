using System.Linq;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.GameStates
{
    public class BettingState : IGameState
    {
        [Inject] private PlacesManager _placesManager;
        
        public void EnterState()
        {
            Debug.Log("Игроки делают ставки");
            
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            var random = Random.Range(0, _placesManager.AllPlayerPlaces.Count);
            var placeInfo = _placesManager.AllPlayerPlaces.ElementAt(random);
            placeInfo.Sync.IsSmallBlind = true;
            placeInfo.Data.Next.Sync.IsBigBlind = true;
            placeInfo.Data.Next.Data.Next.Sync.IsEnabled = true;
        }
        
        public void FixedUpdate()
        {
            
        }

        public void ExitState()
        {
            Debug.Log("Ставки приняты");
        }
    }
}
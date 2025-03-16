using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameStates
{
    public class TurnState : IGameState
    {
        [Inject] private PlacesManager placesManager;
        [Inject] private CanvasesManager canvasesManager;
        
        public void EnterState()
        {
            Debug.Log("Игроки делают ставки TurnState");
            
            if(!PhotonNetwork.IsMasterClient)
                return;

            var bigBlindPlace = placesManager.AllPlayerPlaces.First(place => place.IsBigBlind);
            bigBlindPlace.Next.IsEnabled = true;
            
            canvasesManager.ShowCanvas(PlayerCanvas.StartGame);
        }

        public void ExitState()
        {
            placesManager.AllPlayerPlaces.ForEach(place => place.IsEnabled = false);
            
            canvasesManager.ShowCanvas(PlayerCanvas.None);

            Debug.Log("Ставки приняты TurnState");
        }
    }
}
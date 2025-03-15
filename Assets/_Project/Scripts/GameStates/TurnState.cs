using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.GameStates
{
    public class TurnState : IGameState
    {
        [Inject] private PlacesManager placesManager;
        [Inject] private CanvasesService canvasesService;
        
        public void EnterState()
        {
            Debug.Log("Игроки делают ставки TurnState");
            
            if(!PhotonNetwork.IsMasterClient)
                return;

            var bigBlindPlace = placesManager.AllPlayerPlaces.First(place => place.IsBigBlind);
            bigBlindPlace.Next.IsEnabled = true;
            
            canvasesService.ShowCanvas(PlayerCanvas.StartGame);
        }

        public void ExitState()
        {
            placesManager.AllPlayerPlaces.ForEach(place => place.IsEnabled = false);
            
            canvasesService.ShowCanvas(PlayerCanvas.None);

            Debug.Log("Ставки приняты TurnState");
        }
    }
}
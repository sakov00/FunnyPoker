using _Project.Scripts.Enums;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.GameStates
{
    public class ShowdownState : IGameState
    {
        [Inject] private PlacesManager placesManager;
        [Inject] private CanvasesService canvasesService;
        
        public void EnterState()
        {
            
            canvasesService.ShowCanvas(PlayerCanvas.EndGame);
        }

        public void ExitState()
        {
            placesManager.AllPlayerPlaces.ForEach(place => place.IsEnabled = false);
            
            canvasesService.ShowCanvas(PlayerCanvas.None);

            Debug.Log("Ставки приняты");
        }
    }
}
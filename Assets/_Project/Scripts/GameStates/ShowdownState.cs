using _Project.Scripts.Enums;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameStates
{
    public class ShowdownState : IGameState
    {
        [Inject] private PlacesManager placesManager;
        [Inject] private CanvasesManager canvasesManager;
        
        public void EnterState()
        {
            
            canvasesManager.ShowCanvas(PlayerCanvas.EndGame);
        }

        public void ExitState()
        {
            placesManager.AllPlayerPlaces.ForEach(place => place.IsEnabled = false);
            
            canvasesManager.ShowCanvas(PlayerCanvas.None);

            Debug.Log("Ставки приняты");
        }
    }
}
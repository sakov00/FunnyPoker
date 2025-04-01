using _Project.Scripts.Enums;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameStates
{
    public class ShowdownState : IGameState
    {
        [Inject] private GameData gameData;
        
        public void EnterState()
        {
            
        }

        public void ExitState()
        {
            gameData.AllPlayerPlaces.ForEach(place => place.IsEnabled = false);

            Debug.Log("Ставки приняты");
        }
    }
}
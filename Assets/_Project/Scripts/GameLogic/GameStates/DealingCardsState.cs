using _Project.Scripts.Interfaces;
using UnityEngine;

namespace _Project.Scripts.GameLogic.GameStates
{
    public class DealingCardsState : IGameState
    {
        public bool Completed { get; set; }
        
        public void EnterState()
        {
            Debug.Log("Раздача карт...");
        }

        public void UpdateState()
        {
            Completed = true;
        }

        public void ExitState()
        {
            Debug.Log("Карты разданы.");
        }
    }
}
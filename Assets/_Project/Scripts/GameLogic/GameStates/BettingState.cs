using _Project.Scripts.Interfaces;
using UnityEngine;

namespace _Project.Scripts.GameLogic.GameStates
{
    public class BettingState : IGameState
    {
        public bool IsCompleted { get; set; }
        
        public void EnterState()
        {
            Debug.Log("Игроки делают ставки");
            IsCompleted = false;
        }

        public void ExitState()
        {
            Debug.Log("Ставки приняты");
            IsCompleted = false;
        }
    }
}
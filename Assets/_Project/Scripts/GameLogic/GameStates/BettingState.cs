using _Project.Scripts.Interfaces;
using UnityEngine;

namespace _Project.Scripts.GameLogic.GameStates
{
    public class BettingState : IGameState
    {
        
        public void EnterState()
        {
            Debug.Log("Игроки делают ставки");
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
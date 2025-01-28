using System;
using Unity.VisualScripting;

namespace _Project.Scripts.Interfaces
{
    public interface IGameState
    {
        bool Completed { get; set; }
        
        void EnterState();
        void UpdateState();
        void ExitState();

        
    }
}
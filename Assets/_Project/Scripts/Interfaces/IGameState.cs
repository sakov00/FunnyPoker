using System;
using Unity.VisualScripting;

namespace _Project.Scripts.Interfaces
{
    public interface IGameState
    {
        bool IsCompleted { get; set; }
        void EnterState();
        void ExitState();
    }
}
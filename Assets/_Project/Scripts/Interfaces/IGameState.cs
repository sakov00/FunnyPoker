using System;
using Unity.VisualScripting;

namespace _Project.Scripts.Interfaces
{
    public interface IGameState
    {
        void EnterState();
        void ExitState();
    }
}
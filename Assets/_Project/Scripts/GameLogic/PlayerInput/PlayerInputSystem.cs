using UniRx;
using UnityEngine;

namespace _Project.Scripts.GameLogic.PlayerInput
{
    public class PlayerInputSystem : MonoBehaviour
    {
        public readonly ReactiveCommand OnQ = new();
        public readonly ReactiveCommand OnW = new();
        public readonly ReactiveCommand OnE = new();
        public readonly ReactiveCommand OnR = new();
        public readonly ReactiveCommand OnT = new();
        public readonly ReactiveCommand OnUp = new();
        public readonly ReactiveCommand OnDown = new();

        private void Start()
        {
            // var inputActions = new PlayerInputActions();
            // inputActions.Enable();
            //
            // inputActions.Poker.Bet.performed += _ => { OnBet.Execute(); }; // Q
            // inputActions.Poker.Check.performed += _ => { OnCheck.Execute(); }; // W
            // inputActions.Poker.Fold.performed += _ => { OnFold.Execute(); }; // E
            // inputActions.Poker.Raise.performed += _ => { OnRaise.Execute(); }; // R
            // inputActions.Poker.Fold.performed += _ => { OnIncreaseBet.Execute(); };
            // inputActions.Poker.Raise.performed += _ => { OnDecreaseBet.Execute(); };
        }
    }
}
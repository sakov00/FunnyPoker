using System.Linq;
using _Project.Scripts.MVP.Place;
using _Project.Scripts.Services;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.PlayerInGame
{
    public class PlayerInputSystem : MonoBehaviour
    {
        private PlacePresenter playerPlacePresenter;
        
        [Inject] private PlacesManager playersInfo;
        
        public readonly ReactiveCommand OnBet = new();
        public readonly ReactiveCommand OnCheck = new();
        public readonly ReactiveCommand OnFold = new();
        public readonly ReactiveCommand OnRaise = new();
        public readonly ReactiveCommand OnIncreaseBet = new();
        public readonly ReactiveCommand OnDecreaseBet = new();

        private void Start()
        {
            playerPlacePresenter = playersInfo.AllPlayerPlaces.FirstOrDefault(placePresenter =>
                placePresenter.PlayerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
            
            // var inputActions = new PlayerInputActions();
            // inputActions.Enable();
            //
            // inputActions.Poker.Bet.performed += _ => { if (playerPlacePresenter.IsEnabled) OnBet.Execute(); }; // Q
            // inputActions.Poker.Check.performed += _ => { if (playerPlacePresenter.IsEnabled) OnCheck.Execute(); }; // W
            // inputActions.Poker.Fold.performed += _ => { if (playerPlacePresenter.IsEnabled) OnFold.Execute(); }; // E
            // inputActions.Poker.Raise.performed += _ => { if (playerPlacePresenter.IsEnabled) OnRaise.Execute(); }; // R
            // inputActions.Poker.Fold.performed += _ => { if (playerPlacePresenter.IsEnabled) OnIncreaseBet.Execute(); };
            // inputActions.Poker.Raise.performed += _ => { if (playerPlacePresenter.IsEnabled) OnDecreaseBet.Execute(); };
        }
    }
}
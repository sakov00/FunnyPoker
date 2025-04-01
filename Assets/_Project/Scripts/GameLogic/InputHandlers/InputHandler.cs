using System;
using System.Linq;
using _Project.Scripts.Bootstrap;
using _Project.Scripts.Enums;
using _Project.Scripts.GameLogic.PlayerInput;
using _Project.Scripts.Managers;
using _Project.Scripts.MVP.Place;
using _Project.Scripts.MVP.Table;
using _Project.Scripts.Services;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.InputHandlers
{
    public class InputHandler : MonoBehaviour
    {
        [Inject] private PlayerInputManager playerInputManager;
        [Inject] private PlacesManager playersInfo;
        [Inject] private TablePresenter tablePresenter;
        [Inject] private RoundService roundService;
        
        private PlacePresenter playerPlacePresenter;

        public void Initialize()
        {
            playerPlacePresenter = playersInfo.AllPlayerPlaces.FirstOrDefault(placePresenter =>
                placePresenter.PlayerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
            
            playerInputManager.OnQ.Subscribe(_ => PlayerAct(PlayerAction.Fold)).AddTo(this);
            playerInputManager.OnW.Subscribe(_ => PlayerAct(PlayerAction.Call)).AddTo(this);
            playerInputManager.OnE.Subscribe(_ => PlayerAct(PlayerAction.Raise)).AddTo(this);
        }

        private void PlayerAct(PlayerAction playerAction)
        {
            if(!playerPlacePresenter.IsEnabled)
                return;
            
            switch (playerAction)
            {
                case PlayerAction.Fold : Fold(); break;
                case PlayerAction.Call : Call(); break;
                case PlayerAction.Raise : Raise(50); break;
            }
            
            playerPlacePresenter.IsEnabled = false;
            playerPlacePresenter.Next.IsEnabled = true;
            //roundService.CheckRoundEnd();
        }

        private void Check()
        {
            
        }

        private void Fold()
        {
            playerPlacePresenter.IsFolded = true;
            playerPlacePresenter.HandPlayingCards.Clear();
        }
        
        private void Call()
        {
            playerPlacePresenter.BettingMoney = tablePresenter.MaxPlayerBet;
        }
        
        private void Raise(int value)
        {
            playerPlacePresenter.BettingMoney += value;
        }
    }
}
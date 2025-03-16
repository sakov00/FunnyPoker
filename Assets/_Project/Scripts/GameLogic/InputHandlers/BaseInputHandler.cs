using System.Linq;
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
    public class BaseInputHandler : MonoBehaviour
    {
        [Inject] private PlayerInputSystem playerInputSystem;
        [Inject] private PlacesManager playersInfo;
        [Inject] private TablePresenter tablePresenter;
        [Inject] private RoundService roundService;
        
        private PlacePresenter playerPlacePresenter;

        private void Start()
        {
            playerPlacePresenter = playersInfo.AllPlayerPlaces.FirstOrDefault(placePresenter =>
                placePresenter.PlayerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
            
            playerInputSystem.OnQ.Subscribe(_ => PlayerAct(PlayerAction.Check)).AddTo(this);
            playerInputSystem.OnW.Subscribe(_ => PlayerAct(PlayerAction.Fold)).AddTo(this);
            playerInputSystem.OnR.Subscribe(_ => PlayerAct(PlayerAction.Raise)).AddTo(this);
            playerInputSystem.OnT.Subscribe(_ => PlayerAct(PlayerAction.Call)).AddTo(this);
        }

        private void PlayerAct(PlayerAction playerAction)
        {
            if(!playerPlacePresenter.IsEnabled)
                return;
            
            switch (playerAction)
            {
                case PlayerAction.Check : Check(); break;
                case PlayerAction.Fold : Fold(); break;
                case PlayerAction.Call : Call(); break;
                case PlayerAction.Raise : Raise(50); break;
            }
            
            playerPlacePresenter.IsEnabled = false;
            playerPlacePresenter.Next.IsEnabled = true;
            roundService.CheckRoundEnd();
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
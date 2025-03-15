using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.GameLogic.PlayerInGame;
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
        
        private PlacePresenter playerPlacePresenter;

        private void Start()
        {
            playerPlacePresenter = playersInfo.AllPlayerPlaces.FirstOrDefault(placePresenter =>
                placePresenter.PlayerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
            
            playerInputSystem.OnQ.Subscribe(_ => PlayerAct(PlayerAction.Check)).AddTo(this);
            playerInputSystem.OnW.Subscribe(_ => PlayerAct(PlayerAction.Fold)).AddTo(this);
            playerInputSystem.OnE.Subscribe(_ => PlayerAct(PlayerAction.Bet)).AddTo(this);
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
                case PlayerAction.Bet : Bet(50); break;
                case PlayerAction.Call : Call(); break;
                case PlayerAction.Raise : Raise(50); break;
            }
            
            playerPlacePresenter.IsEnabled = false;
            playerPlacePresenter.Next.IsEnabled = true;
        }

        private void Check()
        {
            
        }

        private void Fold()
        {
            playerPlacePresenter.HandPlayingCards.Clear();
        }
        
        private void Bet(int value)
        {
            playerPlacePresenter.BettingMoney = value;
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
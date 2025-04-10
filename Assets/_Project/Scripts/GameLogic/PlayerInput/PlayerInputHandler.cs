using System;
using System.Linq;
using _Project.Scripts.Bootstrap;
using _Project.Scripts.Enums;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.MVP.Place;
using _Project.Scripts.Services;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UniRx;
using Zenject;

namespace _Project.Scripts.GameLogic.PlayerInput
{
    public class PlayerInputHandler : IInitializable, IDisposable
    {
        [Inject] private PlayerInput playerInput;
        [Inject] private GameData gameData;
        [Inject] private RoundService roundService;
        [Inject] private NetworkCallBacks networkCallBacks;
        
        private readonly CompositeDisposable disposable = new ();
        
        private PlacePresenter playerPlacePresenter;

        public void Initialize()
        {
            networkCallBacks.Joined += PlayerJoined;
        }

        private async void PlayerJoined()
        {
            if(playerPlacePresenter != null)
                return;

            await UniTask.NextFrame();
            
            playerPlacePresenter = gameData.AllPlayerPlaces.FirstOrDefault(placePresenter =>
                placePresenter.PlayerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
            
            playerInput.OnQ.Subscribe(_ => PlayerAct(PlayerAction.Fold)).AddTo(disposable);
            playerInput.OnW.Subscribe(_ => PlayerAct(PlayerAction.Call)).AddTo(disposable);
            playerInput.OnE.Subscribe(_ => PlayerAct(PlayerAction.Raise)).AddTo(disposable);
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
            playerPlacePresenter.GlobalBettingMoney = gameData.TablePresenter.MaxPlayerBet;
        }
        
        private void Raise(int value)
        {
            playerPlacePresenter.GlobalBettingMoney += value;
        }

        public void Dispose()
        {
            disposable.Dispose();
            networkCallBacks.Joined -= PlayerJoined;
        }
    }
}
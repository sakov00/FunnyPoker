using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.MVP.Place;
using _Project.Scripts.Services;
using Photon.Pun;
using UniRx;
using Zenject;

namespace _Project.Scripts.GameLogic.PlayerInput
{
    public class PlayerInputHandler
    {
        [Inject] private PlayerInput playerInput;
        [Inject] private GameData gameData;
        [Inject] private RoundService roundService;
        
        private readonly CompositeDisposable disposable = new ();
        
        private PlacePresenter playerPlacePresenter;

        public void Initialize()
        {
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
            playerPlacePresenter.BettingMoney = gameData.TablePresenter.MaxPlayerBet;
        }
        
        private void Raise(int value)
        {
            playerPlacePresenter.BettingMoney += value;
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}
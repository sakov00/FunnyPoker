using _Project.Scripts.GameLogic.Buttons;
using _Project.Scripts.MVP.Place;
using _Project.Scripts.Services;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.GameLogic.PlayerInGame
{
    public class PlaceHandler : MonoBehaviourPun
    {
        [SerializeField] private PlacePresenter placePresenter;
        [SerializeField] private NextStepButton _nextPlayerButton;
        
        [Inject] private PlacesManager _playersInfo;

        private void Start()
        {
            _nextPlayerButton.OnClicked += NextPlayer;
        }

        private void NextPlayer()
        {
            Debug.Log("Player Clicked");
            
            if(PhotonNetwork.LocalPlayer.ActorNumber !=placePresenter.Sync.PlayerActorNumber)
                return;
            
            if(!placePresenter.Sync.IsEnabled)
                return;
            
            placePresenter.Sync.IsEnabled = false;
            placePresenter.Data.Next.Sync.IsEnabled = true;
        } 

        private void OnDestroy()
        {
            _nextPlayerButton.OnClicked -= NextPlayer;
        }
    }
}
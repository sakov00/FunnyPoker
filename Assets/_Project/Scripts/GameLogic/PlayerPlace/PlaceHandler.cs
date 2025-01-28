using System;
using _Project.Scripts.GameLogic.Buttons;
using _Project.Scripts.Services.Game;
using _Project.Scripts.Services.Network;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.PlayerPlace
{
    public class PlaceHandler : MonoBehaviourPun
    {
        [SerializeField] private PlaceInfo _placeInfo;
        [SerializeField] private NextStepButton _nextPlayerButton;
        
        [Inject] private ServicePlaces _playersInfoService;

        private void Start()
        {
            _nextPlayerButton.OnClicked += NextPlayer;
        }

        private void NextPlayer()
        {
            Debug.Log("Player Clicked");
            
            if(PhotonNetwork.LocalPlayer.ActorNumber !=_placeInfo.PlayerActorNumberSync)
                return;
            
            if(!_placeInfo.IsEnableSync)
                return;
            
            _placeInfo.IsEnableSync = false;
            _placeInfo.NextPlace.IsEnableSync = true;
        } 

        private void OnDestroy()
        {
            _nextPlayerButton.OnClicked -= NextPlayer;
        }
    }
}
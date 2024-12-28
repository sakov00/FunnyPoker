using System;
using _Project.Scripts.GameLogic.Buttons;
using _Project.Scripts.Services.Network;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.PlayerPlace
{
    public class PlaceHandler  : MonoBehaviourPun
    {
        [SerializeField] private PlaceInfo _placeInfo;
        [SerializeField] private NextStepButton _nextPlayerButton;
        
        [Inject] private PlayersInfoInRoomService _playersInfoInRoomService;

        private void Start()
        {
            _nextPlayerButton.OnClicked += NextPlayer;
        }

        private void NextPlayer()
        {
            _playersInfoInRoomService.PlayerPlacesInfo.TryGetValue(_placeInfo.NumberPlace, out var playerActorNumber);
            if(PhotonNetwork.LocalPlayer.ActorNumber != playerActorNumber)
                return;
            
            if(!_placeInfo.IsEnable)
                return;
            
            _placeInfo.IsEnable = false;
            _placeInfo.NextPlace.IsEnable = true;
        } 

        private void OnDestroy()
        {
            _nextPlayerButton.OnClicked -= NextPlayer;
        }
    }
}
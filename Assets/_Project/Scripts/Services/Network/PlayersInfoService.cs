using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Bootstrap;
using _Project.Scripts.Factories;
using _Project.Scripts.Services.Game;
using Photon.Pun;
using Photon.Realtime;
using Zenject;

namespace _Project.Scripts.Services.Network
{
    public class PlayersInfoService : MonoBehaviourPun
    {
        [Inject] private NetworkCallBacks _networkCallBacks;
        [Inject] private PlayerFactory _playerFactory;
        [Inject] private ServicePlaces _servicePlaces;

        private void Start()
        {
            _networkCallBacks.PlayerEntered += PlayerEnteredToRoom;
            _networkCallBacks.PlayerJoined += PlayerJoinedToRoom;
        }

        private void PlayerJoinedToRoom()
        {
            var playerPlaceInfo = _servicePlaces.AllPlayerPlaces.First(place => place.IsFreeSync);
            playerPlaceInfo.PlayerActorNumberSync = PhotonNetwork.LocalPlayer.ActorNumber;
            playerPlaceInfo.IsFreeSync = false;

            _playerFactory.CreatePlayer(playerPlaceInfo.PlayerTransform.position, playerPlaceInfo.PlayerTransform.rotation);
        }

        private void PlayerEnteredToRoom(Player player)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            var playerPlaceInfo = _servicePlaces.AllPlayerPlaces.First(place => place.IsFreeSync);
            playerPlaceInfo.IsFreeSync = false;
        }
        
        private void OnDestroy()
        {
            _networkCallBacks.PlayerEntered -= PlayerEnteredToRoom;
            _networkCallBacks.PlayerJoined -= PlayerJoinedToRoom;
        }
    }
}
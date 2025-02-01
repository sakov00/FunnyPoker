using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Bootstrap;
using _Project.Scripts.Data;
using _Project.Scripts.Factories;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Services
{
    public class PlacesManager : MonoBehaviour
    {
        [Inject] private NetworkCallBacks _networkCallBacks;
        [Inject] private PlayerFactory _playerFactory;
        [field: SerializeField] public List<PlaceInfo> AllPlayerPlaces { get; private set; }
        
        private void Awake()
        {
            _networkCallBacks.PlayerEntered += PlayerEnteredToRoom;
            _networkCallBacks.PlayerJoined += PlayerJoinedToRoom;
            _networkCallBacks.PlayerLeft += PlayerLeft;
        }
        
        private void PlayerJoinedToRoom()
        {
            DestroyEmptyPlaces();
            AllPlayerPlaces.ForEach(place => place.LoadInfoFromPhoton());
            
            var playerPlaceInfo = AllPlayerPlaces.First(place => place.IsFreeSync);
            playerPlaceInfo.PlayerActorNumberSync = PhotonNetwork.LocalPlayer.ActorNumber;
            playerPlaceInfo.IsFreeSync = false;

            _playerFactory.CreatePlayer(playerPlaceInfo.PlayerPoint.position, playerPlaceInfo.PlayerPoint.rotation);
        }

        private void PlayerEnteredToRoom(Player player)
        {
            var playerPlaceInfo = AllPlayerPlaces.First(place => place.IsFreeSync);
            playerPlaceInfo.IsFreeSync = false;
        }
        
        private void PlayerLeft(Player player)
        {
            var placeInfo = AllPlayerPlaces.First(place => place.PlayerActorNumberSync == player.ActorNumber);
            placeInfo.IsFreeSync = false;
        }
        
        public void ActivateRandomPlace()
        {
            var random = Random.Range(0, AllPlayerPlaces.Count);
            AllPlayerPlaces.ElementAt(random).IsEnableSync = true;
        }

        private void DestroyEmptyPlaces()
        {
            for (int i = 0; i < AllPlayerPlaces.Count; i++)
            {
                if(i >= PhotonNetwork.CurrentRoom.MaxPlayers)
                    Destroy(AllPlayerPlaces[i].gameObject);
            }
        }

        private void OnDestroy()
        {
            _networkCallBacks.PlayerEntered -= PlayerEnteredToRoom;
            _networkCallBacks.PlayerJoined -= PlayerJoinedToRoom;
            _networkCallBacks.PlayerLeft -= PlayerLeft;
        }
    }
}
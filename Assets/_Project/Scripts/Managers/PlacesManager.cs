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
            _networkCallBacks.PlayerEnteredToRoom += PlayerEnteredToRoom;
            _networkCallBacks.PlayerJoinedToRoom += PlayerJoinedToRoom;
            _networkCallBacks.PlayerLeftRoom += PlayerLeft;
        }
        
        private void PlayerJoinedToRoom()
        {
            DestroyEmptyPlaces();
            AllPlayerPlaces.ForEach(place => place.LoadInfoFromPhoton());
            
            var playerPlaceInfo = AllPlayerPlaces.First(place => place.IsFreeSync);
            _playerFactory.CreatePlayer(playerPlaceInfo.PlayerPoint.position, playerPlaceInfo.PlayerPoint.rotation);
        }

        private void PlayerEnteredToRoom(Player player)
        {
            var playerPlaceInfo = AllPlayerPlaces.First(place => place.IsFreeSync);
            playerPlaceInfo.PlayerActorNumberSync = PhotonNetwork.LocalPlayer.ActorNumber;
            playerPlaceInfo.IsFreeSync = false;
        }
        
        private void PlayerLeft(Player player)
        {
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            var placeInfo = AllPlayerPlaces.First(place => place.PlayerActorNumberSync == player.ActorNumber);
            placeInfo.PlayerActorNumberSync = 0;
            placeInfo.IsFreeSync = false;
        }
        
        public void ActivateRandomPlace()
        {
            var random = Random.Range(0, AllPlayerPlaces.Count);
            AllPlayerPlaces.ElementAt(random).IsEnableSync = true;
        }

        private void DestroyEmptyPlaces()
        {
            for (int i = AllPlayerPlaces.Count - 1; i >= 0; i--)
            {
                if (i >= PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    Destroy(AllPlayerPlaces[i].gameObject);
                    AllPlayerPlaces.RemoveAt(i);
                }
            }
        }

        private void OnDestroy()
        {
            _networkCallBacks.PlayerEnteredToRoom -= PlayerEnteredToRoom;
            _networkCallBacks.PlayerJoinedToRoom -= PlayerJoinedToRoom;
            _networkCallBacks.PlayerLeftRoom -= PlayerLeft;
        }
    }
}
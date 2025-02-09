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
    public class PlacesManager : MonoBehaviourPunCallbacks
    {
        [Inject] private PlayerFactory _playerFactory;
        [field: SerializeField] public List<PlaceInfo> AllPlayerPlaces { get; private set; }
        
        public override void OnJoinedRoom()
        {
            DestroyEmptyPlaces();
            AllPlayerPlaces.ForEach(place => place.LoadInfoFromPhoton());
            
            var playerPlaceInfo = AllPlayerPlaces.First(place => place.IsFreeSync);
            playerPlaceInfo.PlayerActorNumberSync = PhotonNetwork.LocalPlayer.ActorNumber;
            playerPlaceInfo.IsFreeSync = false;
            _playerFactory.CreatePlayer(playerPlaceInfo.PlayerPoint.position, playerPlaceInfo.PlayerPoint.rotation);
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            var playerPlaceInfo = AllPlayerPlaces.First(place => place.IsFreeSync);
            playerPlaceInfo.PlayerActorNumberSync = player.ActorNumber;
            playerPlaceInfo.IsFreeSync = false;
        }
        
        public override void OnPlayerLeftRoom(Player player)
        {
            var placeInfo = AllPlayerPlaces.First(place => place.PlayerActorNumberSync == player.ActorNumber);
            placeInfo.PlayerActorNumberSync = 0;
            placeInfo.IsFreeSync = true;
        }
        
        public void ActivateRandomPlace()
        {
            var random = Random.Range(0, AllPlayerPlaces.Count);
            AllPlayerPlaces.ElementAt(random).IsEnableSync = true;
        }
        
        public void AllPlacesIsEnable(bool isEnable)
        {
            AllPlayerPlaces.ForEach(place => place.IsEnableSync = isEnable);
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
    }
}
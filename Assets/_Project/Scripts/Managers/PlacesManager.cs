using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Factories;
using _Project.Scripts.MVP.Place;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Services
{
    public class PlacesManager : MonoBehaviourPunCallbacks
    {
        [Inject] private PlayerFactory _playerFactory;
        [field: SerializeField] public List<PlacePresenter> AllPlayerPlaces { get; private set; }
        
        public override void OnJoinedRoom()
        {
            DestroyEmptyPlaces();
            AllPlayerPlaces.ForEach(place => place.LoadFromPhoton());
            
            var playerPlaceInfo = AllPlayerPlaces.First(place => place.Sync.IsFree);
            playerPlaceInfo.Sync.PlayerActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            playerPlaceInfo.Sync.IsFree = false;
            _playerFactory.CreatePlayer(playerPlaceInfo.Data.PlayerPoint.position, playerPlaceInfo.Data.PlayerPoint.rotation);
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            var playerPlaceInfo = AllPlayerPlaces.First(place => place.Sync.IsFree);
            playerPlaceInfo.Sync.PlayerActorNumber = player.ActorNumber;
            playerPlaceInfo.Sync.IsFree = false;
        }
        
        public override void OnPlayerLeftRoom(Player player)
        {
            var placeInfo = AllPlayerPlaces.First(place => place.Sync.PlayerActorNumber == player.ActorNumber);
            placeInfo.Sync.PlayerActorNumber = 0;
            placeInfo.Sync.IsFree = true;
        }
        
        public void GetPlaceInfoByNumber(int number)
        {
            var random = Random.Range(0, AllPlayerPlaces.Count);
            AllPlayerPlaces.ElementAt(random).Sync.IsEnabled = true;
        }
        
        public void AllPlacesIsEnable(bool isEnable)
        {
            AllPlayerPlaces.ForEach(place => place.Sync.IsEnabled = isEnable);
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
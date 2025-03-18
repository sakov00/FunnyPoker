using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Factories;
using _Project.Scripts.MVP.Place;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Managers
{
    public class PlacesManager : MonoBehaviour
    {
        [Inject] private PlayerFactory playerFactory;
        [field: SerializeField] public List<PlacePresenter> AllPlayerPlaces { get; private set; }
        
        public void OnJoinedRoom()
        {
            DestroyEmptyPlaces();
            AllPlayerPlaces.ForEach(place => place.LoadFromPhoton());
            
            var playerPlaceInfo = AllPlayerPlaces.First(place => place.IsFree);
            playerPlaceInfo.PlayerActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            playerPlaceInfo.IsFree = false;
            playerFactory.CreatePlayer(playerPlaceInfo.PlayerPoint.position, playerPlaceInfo.PlayerPoint.rotation);
        }

        public void OnPlayerEnteredRoom(Player player)
        {
            var playerPlaceInfo = AllPlayerPlaces.First(place => place.IsFree);
            playerPlaceInfo.PlayerActorNumber = player.ActorNumber;
            playerPlaceInfo.IsFree = false;
        }
        
        public void OnPlayerLeftRoom(Player player)
        {
            var placeInfo = AllPlayerPlaces.First(place => place.PlayerActorNumber == player.ActorNumber);
            placeInfo.PlayerActorNumber = 0;
            placeInfo.IsFree = true;
        }
        
        public void GetPlaceInfoByNumber(int number)
        {
            var random = Random.Range(0, AllPlayerPlaces.Count);
            AllPlayerPlaces.ElementAt(random).IsEnabled = true;
        }
        
        public void AllPlacesIsEnable(bool isEnable)
        {
            AllPlayerPlaces.ForEach(place => place.IsEnabled = isEnable);
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
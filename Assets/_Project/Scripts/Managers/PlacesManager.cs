using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Factories;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.MVP.Place;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Managers
{
    public class PlacesManager
    {
        [Inject] private PlayerFactory playerFactory;
        [Inject] private GameData gameData;
        
        public void OnJoinedRoom()
        {
            DestroyEmptyPlaces();
            gameData.AllPlayerPlaces.ForEach(place => place.LoadFromPhoton());
            
            var playerPlaceInfo = gameData.AllPlayerPlaces.First(place => place.IsFree);
            playerPlaceInfo.PlayerActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            playerPlaceInfo.IsFree = false;
            playerFactory.CreatePlayer(playerPlaceInfo.PlayerPoint.position, playerPlaceInfo.PlayerPoint.rotation);
        }

        public void OnPlayerEnteredRoom(Player player)
        {
            var playerPlaceInfo = gameData.AllPlayerPlaces.First(place => place.IsFree);
            playerPlaceInfo.PlayerActorNumber = player.ActorNumber;
            playerPlaceInfo.IsFree = false;
        }
        
        public void OnPlayerLeftRoom(Player player)
        {
            var placeInfo = gameData.AllPlayerPlaces.First(place => place.PlayerActorNumber == player.ActorNumber);
            placeInfo.PlayerActorNumber = 0;
            placeInfo.IsFree = true;
        }
        
        public void GetPlaceInfoByNumber(int number)
        {
            var random = Random.Range(0, gameData.AllPlayerPlaces.Count);
            gameData.AllPlayerPlaces.ElementAt(random).IsEnabled = true;
        }
        
        public void AllPlacesIsEnable(bool isEnable)
        {
            gameData.AllPlayerPlaces.ForEach(place => place.IsEnabled = isEnable);
        }

        private void DestroyEmptyPlaces()
        {
            for (int i = gameData.AllPlayerPlaces.Count - 1; i >= 0; i--)
            {
                if (i >= PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    Object.Destroy(gameData.AllPlayerPlaces[i].gameObject);
                    gameData.AllPlayerPlaces.RemoveAt(i);
                }
            }
        }
    }
}
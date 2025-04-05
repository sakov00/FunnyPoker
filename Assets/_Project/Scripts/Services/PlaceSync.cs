using System;
using System.Linq;
using _Project.Scripts.Bootstrap;
using _Project.Scripts.Factories;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.MVP.Player;
using Photon.Pun;
using Photon.Realtime;
using Zenject;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Services
{
    public class PlaceSync : IInitializable, IDisposable
    {
        [Inject] private PlayerFactory playerFactory;
        [Inject] private NetworkCallBacks networkCallBacks;
        [Inject] private GameData gameData;
        [Inject] private DataSync dataSync;
        
        public void Initialize()
        {
            networkCallBacks.Joined += PlayerJoined;
            networkCallBacks.Entered += PlayerEntered;
            networkCallBacks.Left += PlayerLeft;
        }
        
        private void PlayerJoined()
        {
            DestroyEmptyPlaces();
            dataSync.LoadFromPhoton();

            var playerPlaceInfo = gameData.AllPlayerPlaces.First(place => place.IsFree);
            playerPlaceInfo.PlayerActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            playerPlaceInfo.IsFree = false;
            playerFactory.CreatePlayer(playerPlaceInfo.PlayerPoint.position, playerPlaceInfo.PlayerPoint.rotation);
        }

        private void PlayerEntered(Player newPlayer)
        {
            var playerPlaceInfo = gameData.AllPlayerPlaces.First(place => place.IsFree);
            playerPlaceInfo.PlayerActorNumber = newPlayer.ActorNumber;
            playerPlaceInfo.IsFree = false;
        }

        private void PlayerLeft(Player otherPlayer)
        {
            var placeInfo = gameData.AllPlayerPlaces.First(place => place.PlayerActorNumber == otherPlayer.ActorNumber);
            placeInfo.PlayerActorNumber = 0;
            placeInfo.IsFree = true;
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
        
        public void Dispose()
        {
            networkCallBacks.Joined -= PlayerJoined;
            networkCallBacks.Entered -= PlayerEntered;
            networkCallBacks.Left -= PlayerLeft;
        }
    }
}
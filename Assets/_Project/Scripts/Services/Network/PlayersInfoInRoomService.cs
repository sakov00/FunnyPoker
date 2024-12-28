using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Factories;
using _Project.Scripts.GameLogic.Player;
using _Project.Scripts.GameLogic.PlayerPlace;
using _Project.Scripts.Services.Game;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Services.Network
{
    public class PlayersInfoInRoomService : MonoBehaviourPun
    {
        [Inject] private PlayerFactory _playerFactory;
        [Inject] private ServicePlaces _servicePlaces;
        public SortedDictionary<int, int> PlayerPlacesInfo { get; private set; } = new();

        public void PlayerJoinedToRoom()
        {
            var playerPlaceInfo = _servicePlaces.AllPlayerPlaces.First(place => place.IsFree);
            playerPlaceInfo.IsFree = false;

            _playerFactory.CreatePlayer(playerPlaceInfo.PlayerTransform.position, playerPlaceInfo.PlayerTransform.rotation);

            PlayerPlacesInfo.Add(playerPlaceInfo.NumberPlace, PhotonNetwork.LocalPlayer.ActorNumber);
        }

        public void PlayerEnteredToRoom(Player newPlayer)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            var playerPlaceInfo = _servicePlaces.AllPlayerPlaces.First(place => place.IsFree);
            playerPlaceInfo.IsFree = false;

            PlayerPlacesInfo.Add(playerPlaceInfo.NumberPlace, newPlayer.ActorNumber);

            photonView.RPC("AddNewPlayerRPC", RpcTarget.Others, playerPlaceInfo.NumberPlace, newPlayer.ActorNumber);
            
            photonView.RPC("ReceivePlayerInfo", newPlayer, PlayerPlacesInfo.Keys.ToArray(),
                PlayerPlacesInfo.Values.ToArray());
        }

        [PunRPC]
        private void AddNewPlayerRPC(int playerPlaceNumber, int playerId)
        {
            PlayerPlacesInfo.TryAdd(playerPlaceNumber, playerId);
        }

        [PunRPC]
        private void RemovePlayerRPC(int playerPlaceNumber)
        {
            PlayerPlacesInfo.Remove(playerPlaceNumber);
        }

        [PunRPC]
        private void ReceivePlayerInfo(int[] playerInfoKeys, int[] playerInfoIds)
        {
            PlayerPlacesInfo.Clear();
            for (var i = 0; i < playerInfoKeys.Length; i++) PlayerPlacesInfo.Add(playerInfoIds[i], playerInfoKeys[i]);
        }
    }
}
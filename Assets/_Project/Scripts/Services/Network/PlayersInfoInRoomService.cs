using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.GameLogic.Player;
using Assets._Project.Scripts.Factories;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Services.Network
{
    public class PlayersInfoInRoomService : MonoBehaviourPun
    {
        [SerializeField] private List<PlayerPlaceInfo> allPlayerPlaces;
        
        [Inject] private PlayerFactory _playerFactory;

        public Dictionary<int, int> PlayerPlacesInfo { get; private set; } = new(8);
        
        public void PlayerJoinedToRoom()
        {
            var playerPlaceInfo = allPlayerPlaces.First(place => place.IsFreePlace);
            playerPlaceInfo.IsFreePlace = false;
            
            _playerFactory.CreatePlayer(playerPlaceInfo.PlayerPlace.position, playerPlaceInfo.PlayerPlace.rotation);
            
            PlayerPlacesInfo.Add(PhotonNetwork.LocalPlayer.ActorNumber, playerPlaceInfo.Id);
        }
        
        public void PlayerEnteredToRoom(Player newPlayer)
        {
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            var playerPlaceInfo = allPlayerPlaces.First(place => place.IsFreePlace);
            playerPlaceInfo.IsFreePlace = false;
            
            PlayerPlacesInfo.Add(newPlayer.ActorNumber, playerPlaceInfo.Id);
            
            photonView.RPC("AddNewPlayerRPC", RpcTarget.Others, newPlayer.ActorNumber, playerPlaceInfo.Id);
            
            var playerInfoList = PlayerPlacesInfo.Select(kvp => new { PlayerId = kvp.Key, PlaceId = kvp.Value }).ToList();
            photonView.RPC("ReceivePlayerInfo", newPlayer, PlayerPlacesInfo.Keys.ToArray(), PlayerPlacesInfo.Values.ToArray());
        }
        
        [PunRPC]
        private void AddNewPlayerRPC(int playerId, int playerPlaceInfoId)
        {
            PlayerPlacesInfo.TryAdd(playerId, playerPlaceInfoId);
        }
        
        [PunRPC]
        private void RemovePlayerRPC(int playerId)
        {
            PlayerPlacesInfo.Remove(playerId);
        }
        
        [PunRPC]
        private void ReceivePlayerInfo(int[] playerInfoKeys, int[] playerInfoIds)
        {
            PlayerPlacesInfo.Clear();
            for (int i = 0; i < playerInfoKeys.Length; i++)
            {
                PlayerPlacesInfo.Add(playerInfoKeys[i], playerInfoIds[i]);
            }
        }
    }
}
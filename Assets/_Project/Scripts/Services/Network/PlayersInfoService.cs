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
        public SortedDictionary<int, int> PlayerPlacesInfo { get; private set; } = new();

        private void Start()
        {
            _networkCallBacks.PlayerEntered += PlayerEnteredToRoom;
            _networkCallBacks.PlayerJoined += PlayerJoinedToRoom;
        }

        private void PlayerJoinedToRoom()
        {
            var playerPlaceInfo = _servicePlaces.AllPlayerPlaces.First(place => place.IsFree);
            playerPlaceInfo.IsFree = false;

            _playerFactory.CreatePlayer(playerPlaceInfo.PlayerTransform.position, playerPlaceInfo.PlayerTransform.rotation);

            PlayerPlacesInfo.Add(playerPlaceInfo.NumberPlace, PhotonNetwork.LocalPlayer.ActorNumber);
        }

        private void PlayerEnteredToRoom(Player player)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            var playerPlaceInfo = _servicePlaces.AllPlayerPlaces.First(place => place.IsFree);
            playerPlaceInfo.IsFree = false;

            PlayerPlacesInfo.Add(playerPlaceInfo.NumberPlace, player.ActorNumber);

            photonView.RPC("AddNewPlayerRPC", RpcTarget.Others, playerPlaceInfo.NumberPlace, player.ActorNumber);
            
            photonView.RPC("ReceivePlayerInfo", player, PlayerPlacesInfo.Keys.ToArray(),
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

        private void OnDestroy()
        {
            _networkCallBacks.PlayerEntered -= PlayerEnteredToRoom;
            _networkCallBacks.PlayerJoined -= PlayerJoinedToRoom;
        }
    }
}
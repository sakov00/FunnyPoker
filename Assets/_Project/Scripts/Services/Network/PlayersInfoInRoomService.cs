using System.Collections.Generic;
using _Project.Scripts.Data;
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
        [SerializeField] private List<Transform> playersPositions = new();
        
        [Inject] private PlayerFactory _playerFactory;

        public Dictionary<int, PlayerData> PlayersData { get; private set; } = new(8);
        public Dictionary<int, PlayerActivity> PlayersActivity { get; private set; } = new(8);
        
        public void PlayerJoinedToRoom()
        {
            var spawnPoint = playersPositions[PhotonNetwork.LocalPlayer.ActorNumber - 1];
            var player = _playerFactory.CreatePlayer(spawnPoint.position, spawnPoint.rotation);
            
            PlayersData.Add(PhotonNetwork.LocalPlayer.ActorNumber, player.GetComponent<PlayerData>());
            PlayersActivity.Add(PhotonNetwork.LocalPlayer.ActorNumber, player.GetComponent<PlayerActivity>());
        }
    }
}
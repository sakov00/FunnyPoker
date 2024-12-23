using System.Collections.Generic;
using _Project.Scripts.Data;
using Assets._Project.Scripts.Factories;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Services.Network
{
    public class PlayersInRoomService : MonoBehaviour
    {
        [Inject] private PlayerFactory _playerFactory;
        
        [SerializeField] private List<PlayerData> _players = new();
        [SerializeField] private List<Transform> _playersPositions = new();

        public void PlayerJoinedToRoom()
        {
            var spawnPoint = _playersPositions[PhotonNetwork.CurrentRoom.PlayerCount - 1];
            var player = _playerFactory.CreatePlayer(spawnPoint.position, spawnPoint.rotation);
            _players.Add(player);
            Object.Destroy(spawnPoint.gameObject);
        }
    }
}
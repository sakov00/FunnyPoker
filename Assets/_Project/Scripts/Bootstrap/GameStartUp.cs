using System;
using _Project.Scripts.Services.Game;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class GameStartUp : MonoBehaviour
    {
        [Inject] private NetworkCallBacks _networkCallBacks;
        [Inject] private ServicePlaces _servicePlaces;

        private void Start()
        {
            _networkCallBacks.PlayerEntered += StartGame;
        }

        public void StartGame(Player player)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            
            if (PhotonNetwork.CurrentRoom.MaxPlayers != PhotonNetwork.CurrentRoom.PlayerCount)
                return;

            _servicePlaces.ActivateRandomPlace();
        }
    }
}
using System;
using System.Linq;
using _Project.Scripts.Services.Network;
using Photon.Pun;
using Zenject;

namespace _Project.Scripts.Services.Game
{
    public class QueuePlayerController : MonoBehaviourPun
    {
        [Inject] private PlayersInfoInRoomService _playersInfoInRoomService;

        private int _currentPlayerActorNumber;

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var playerCount = _playersInfoInRoomService.PlayerPlacesInfo.Count;
                Random rand = new Random();
                var randomIndex = rand.Next(playerCount);
                var playerActorNumber = _playersInfoInRoomService.PlayerPlacesInfo.Keys.ElementAt(randomIndex);
                photonView.RPC("SyncCurrentPlayerIndex", RpcTarget.Others, playerActorNumber);
            }
        }

        /*public void NextPlayer()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var playersActivity = _playersInfoInRoomService.PlayersActivity;

                if (_currentPlayerIndex + 1 > playersActivity.Count || _currentPlayerIndex < 0)
                    return;

                var currentPlayer = playersActivity[_currentPlayerIndex];
                var nextPlayer = playersActivity[_currentPlayerIndex + 1];

                currentPlayer.DisableActivity();
                nextPlayer.EnableActivity();
                
                photonView.RPC("SyncCurrentPlayerIndex", RpcTarget.Others, _currentPlayerActorNumber);
            }
        }*/
        
        [PunRPC]
        private void SyncCurrentPlayerIndex(int playerActorNumber)
        {
            _currentPlayerActorNumber = playerActorNumber;
        }
    }
}
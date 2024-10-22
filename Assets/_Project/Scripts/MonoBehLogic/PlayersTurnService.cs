using Assets._Project.Scripts.Factories;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Project.Scripts.MonoBehLogic
{
    public class PlayersTurnService : MonoBehaviourPunCallbacks
    {
        private List<int> playersList = new List<int>();
        private int currentPlayerActorNumber;

        public void Initialize()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                currentPlayerActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                photonView.RPC("RPC_UpdateCurrentPlayerActorNumber", RpcTarget.All, currentPlayerActorNumber);
            }
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playersList.Add(PhotonNetwork.LocalPlayer.ActorNumber);
            }
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playersList.Add(player.ActorNumber);
                photonView.RPC("RPC_UpdateplayersList", RpcTarget.All, playersList.ToArray());
            }
        }

        public override void OnPlayerLeftRoom(Player player)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playersList.Remove(player.ActorNumber);
                photonView.RPC("RPC_UpdateplayersList", RpcTarget.All, playersList.ToArray());
            }
        }

        public void NextPlayer()
        {
            bool currentPlayerFounded = false;
            foreach (var player in playersList)
            {
                if (player == currentPlayerActorNumber)
                {
                    currentPlayerFounded = true;
                }
                else if(currentPlayerFounded)
                {
                    currentPlayerActorNumber = player;
                    currentPlayerFounded = false;
                    break;
                }
            }
            if (currentPlayerFounded)
            {
                currentPlayerActorNumber = playersList.First();
            }
            photonView.RPC("RPC_UpdateCurrentPlayerActorNumber", RpcTarget.All, currentPlayerActorNumber);
        }

        public bool IsCurrentPlayerActorNumber(int actorNumber)
        {
            return currentPlayerActorNumber == actorNumber;
        }

        [PunRPC]
        private void RPC_UpdateCurrentPlayerActorNumber(int newActorNumber)
        {
            currentPlayerActorNumber = newActorNumber;
        }

        [PunRPC]
        private void RPC_UpdateplayersList(int[] newPlayersList)
        {
            playersList.Clear();
            playersList.AddRange(newPlayersList);
        }
    }
}

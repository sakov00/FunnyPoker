using Assets._Project.Scripts.Factories;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.Menu.Network
{
    public class ConnectionToPhoton : MonoBehaviourPunCallbacks
    {
        public void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
            Debug.Log("OnConnectedToMaster");
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby");
        }
    }
}

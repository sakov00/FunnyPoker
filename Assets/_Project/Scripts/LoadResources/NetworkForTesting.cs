﻿using System;
using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts.LoadResources
{
    public class NetworkForTesting : MonoBehaviourPunCallbacks
    {
        public Action CallBackConnection;

        public void Awake()
        {
            if (PhotonNetwork.IsConnected)
                return;

            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
            Debug.Log("OnConnectedToMaster");
        }

        public override void OnJoinedLobby()
        {
            if (CallBackConnection != null)
                CallBackConnection.Invoke();
            Debug.Log("OnJoinedLobby");
        }
    }
}
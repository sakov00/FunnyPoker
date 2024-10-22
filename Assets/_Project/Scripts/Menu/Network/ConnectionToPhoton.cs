using Photon.Pun;
using UnityEngine;

namespace Assets._Project.Scripts.Menu.Network
{
    public class ConnectionToPhoton : MonoBehaviourPunCallbacks
    {
        public void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.AutomaticallySyncScene = true;
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

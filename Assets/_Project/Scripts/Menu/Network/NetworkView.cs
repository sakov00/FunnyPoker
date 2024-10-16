using Photon.Pun;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Scripts.Menu.Network
{
    public class NetworkView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField playerNameInput;
        [SerializeField] private TMP_InputField nameServerInput;
        [SerializeField] private Button createRoomButton;
        [SerializeField] private Button joinRoomButton;

        [field: SerializeField] public Transform ContentRooms { get; private set; }

        public IObservable<string> OnCreateRoom => createRoomButton.OnClickAsObservable().Select(_ => nameServerInput.text);
        public IObservable<string> OnJoinRoom => joinRoomButton.OnClickAsObservable().Select(_ => nameServerInput.text);

        private void Start()
        {
            playerNameInput.onValueChanged.AddListener(OnPlayerNameChanged);
        }

        private void OnPlayerNameChanged(string name)
        {
            PhotonNetwork.LocalPlayer.NickName = name + PhotonNetwork.LocalPlayer.ActorNumber;
        }
    }
}

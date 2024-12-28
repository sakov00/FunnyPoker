using System.Collections.Generic;
using _Project.Scripts.Menu.Enums;
using _Project.Scripts.Menu.ManagmentPanels;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace _Project.Scripts.Menu.Network
{
    internal class NetworkPresenter : MonoBehaviourPunCallbacks
    {
        [SerializeField] private AssetReference roomListing;

        [Inject] private NetworkModel networkModel;
        [Inject] private NetworkView networkView;
        [Inject] private PanelPresenter panelPresenter;

        private void Start()
        {
            LoadRoomListingAsset();
            SetupEventHandlers();
        }

        private void LoadRoomListingAsset()
        {
            var handle = roomListing.LoadAssetAsync<GameObject>();
            handle.Completed += operation =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                    Debug.Log("Room listing asset loaded successfully.");
                else
                    Debug.LogError("Failed to load room listing asset.");
            };
        }

        private void SetupEventHandlers()
        {
            networkView.OnCreateRoom
                .Subscribe(nameServer => CreateRoom(nameServer))
                .AddTo(this);

            networkView.OnJoinRoom
                .Subscribe(nameServer => JoinToRoom(nameServer))
                .AddTo(this);
        }

        private void CreateRoom(string nameServer)
        {
            if (!PhotonNetwork.IsConnected)
                return;

            var roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 5;
            PhotonNetwork.CreateRoom(nameServer, roomOptions);
        }

        private void JoinToRoom(string nameServer)
        {
            if (!PhotonNetwork.IsConnected)
                return;

            PhotonNetwork.JoinRoom(nameServer);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (var roomElement in networkModel.RoomElements) Destroy(roomElement.LinkedGameObject);
            networkModel.RoomElements.Clear();

            foreach (var roomInfo in roomList)
                if (!roomInfo.RemovedFromList)
                    RoomAdded(roomInfo);
        }

        private void RoomAdded(RoomInfo roomInfoAdded)
        {
            var newRoomElement = new RoomElement
            {
                RoomInfo = roomInfoAdded,
                LinkedGameObject = (GameObject)Instantiate(roomListing.Asset, networkView.ContentRooms)
            };

            newRoomElement.LinkedGameObject.GetComponentInChildren<TextMeshProUGUI>()
                .SetText(newRoomElement.RoomInfo.Name);
            networkModel.RoomElements.Add(newRoomElement);
        }

        public override void OnJoinedRoom()
        {
            panelPresenter.ChangePanel(TypePanel.CurrentRoom);
        }
    }
}
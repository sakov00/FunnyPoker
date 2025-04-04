﻿using System.Linq;
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

namespace _Project.Scripts.Menu.CurrentNetworkRoom
{
    public class CurrentRoomPresenter : MonoBehaviourPunCallbacks
    {
        [SerializeField] private AssetReference roomListing;
        [Inject] private PanelPresenter panelPresenter;

        [Inject] private CurrentRoomModel roomModel;
        [Inject] private CurrentRoomView roomView;


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
            roomView.OnStartGame
                .Subscribe(_ => StartGame())
                .AddTo(this);

            roomView.OnLeaveRoom
                .Subscribe(_ => LeaveRoom())
                .AddTo(this);

            gameObject.ObserveEveryValueChanged(p => p.activeSelf)
                .Where(isActive => isActive)
                .Subscribe(_ => PlayerJoinToRoom())
                .AddTo(this);
        }

        private void PlayerJoinToRoom()
        {
            foreach (var player in PhotonNetwork.PlayerList) AddPlayerToList(player);
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            AddPlayerToList(player);
        }

        public override void OnPlayerLeftRoom(Player player)
        {
            RemovePlayerFromList(player);
        }

        private void StartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.LoadLevel(1);
            }
        }

        private void LeaveRoom()
        {
            foreach (var player in PhotonNetwork.PlayerList) RemovePlayerFromList(player);

            PhotonNetwork.LeaveRoom();
            panelPresenter.ChangePanel(TypePanel.Network);
        }

        private void AddPlayerToList(Player player)
        {
            var newPlayerElement = new PlayerElement
            {
                Player = player,
                LinkedGameObject = (GameObject)Instantiate(roomListing.Asset, roomView.ContentPlayers)
            };

            newPlayerElement.LinkedGameObject.GetComponentInChildren<TextMeshProUGUI>()
                .SetText(newPlayerElement.Player.NickName);
            roomModel.PlayerElements.Add(newPlayerElement);
        }

        private void RemovePlayerFromList(Player player)
        {
            var removedPlayerElement = roomModel.PlayerElements.FirstOrDefault(playerElement =>
                playerElement.Player.ActorNumber == player.ActorNumber);
            if (removedPlayerElement == null)
                return;

            Destroy(removedPlayerElement.LinkedGameObject);
            roomModel.PlayerElements.Remove(removedPlayerElement);
        }
    }
}
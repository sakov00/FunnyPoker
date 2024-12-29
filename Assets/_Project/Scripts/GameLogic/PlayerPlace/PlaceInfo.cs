using System;
using _Project.Scripts.Bootstrap;
using _Project.Scripts.GameLogic.Rendering;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.PlayerPlace
{
    public class PlaceInfo : MonoBehaviourPunCallbacks
    {
        [SerializeField] private BloomPoint _greenButton;
        [SerializeField] private BloomPoint _yellowButton;
        
        [Inject] private NetworkCallBacks _networkCallBacks;
        
        [field: SerializeField] public PlaceInfo PreviousPlace { get; private set; }
        [field: SerializeField] public PlaceInfo NextPlace { get; private set; }
        [field: SerializeField] public int NumberPlace { get; private set; }
        [field: SerializeField] public Transform PlayerTransform { get; set; }

        private bool _isFree;
        private bool IsFree { get; set; }
        public bool IsFreeSync
        {
            get => IsFree;
            set
            {
                IsFree = value;
                SyncProperty(nameof(IsFree) + NumberPlace, value);
            }
        }
        
        private bool _isEnable;
        private bool IsEnable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                _greenButton.SetBloomEnabled(_isEnable);
                _yellowButton.SetBloomEnabled(!_isEnable);
            }
        }
        public bool IsEnableSync
        {
            get => IsEnable;
            set
            {
                IsEnable = value;
                SyncProperty(nameof(IsEnable) + NumberPlace, value);
            }
        }   
        
        private int _playerActorNumber;
        private int PlayerActorNumber { get; set; }
        public int PlayerActorNumberSync
        {
            get => PlayerActorNumber;
            set
            {
                PlayerActorNumber = value;
                SyncProperty(nameof(PlayerActorNumber) + NumberPlace, value);
            }
        }
        
        private void SyncProperty(string key, object value)
        {
            Hashtable property = new Hashtable { { key, value } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(property);
        }
        
        private void LoadFromPhoton()
        {
            var photonPlayer = PhotonNetwork.MasterClient;
            
            if (photonPlayer.CustomProperties.ContainsKey(nameof(IsFree) + NumberPlace))
                IsFree = (bool)photonPlayer.CustomProperties[nameof(IsFree) + NumberPlace];

            if (photonPlayer.CustomProperties.ContainsKey(nameof(IsEnable) + NumberPlace))
                IsEnable = (bool)photonPlayer.CustomProperties[nameof(IsEnable) + NumberPlace];
            
            if (photonPlayer.CustomProperties.ContainsKey(nameof(PlayerActorNumber) + NumberPlace))
                PlayerActorNumber = (int)photonPlayer.CustomProperties[nameof(PlayerActorNumber) + NumberPlace];
        }
        
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(nameof(IsFree) + NumberPlace))
                IsFree = (bool)changedProps[nameof(IsFree) + NumberPlace];

            if (changedProps.ContainsKey(nameof(IsEnable) + NumberPlace))
                IsEnable = (bool)changedProps[nameof(IsEnable) + NumberPlace];
            
            if (changedProps.ContainsKey(nameof(PlayerActorNumber) + NumberPlace))
                PlayerActorNumber = (int)changedProps[nameof(PlayerActorNumber) + NumberPlace];
        }

        private void Awake()
        {
            _networkCallBacks.PlayerJoined += PlayerConnected;
            _networkCallBacks.PlayerLeft += PlayerLeft;
        }

        private void PlayerConnected()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                IsFreeSync = true;
                IsEnableSync = false;
            }
            else
            {
                LoadFromPhoton();
            }
        }
        
        private void PlayerLeft(Player player)
        {
            IsFreeSync = false;
        }

        private void OnDestroy()
        {
            _networkCallBacks.PlayerJoined -= PlayerConnected;
            _networkCallBacks.PlayerLeft -= PlayerLeft;
        }
    }
}
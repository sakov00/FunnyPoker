using System;
using _Project.Scripts.GameLogic.Rendering;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace _Project.Scripts.Data
{
    public class PlaceInfo : MonoBehaviourPunCallbacks
    {
        [SerializeField] private BloomPoint _greenButton;
        [SerializeField] private BloomPoint _yellowButton;
        
        [field: SerializeField] public PlaceInfo PreviousPlace { get; private set; }
        [field: SerializeField] public PlaceInfo NextPlace { get; private set; }
        [field: SerializeField] public int NumberPlace { get; private set; }
        [field: SerializeField] public Transform PlayerTransform { get; set; }
        
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

        private void Awake()
        {
            IsFreeSync = true;
            IsEnableSync = false;
        }

        private void SyncProperty(string key, object value)
        {
            Hashtable property = new Hashtable { { key, value } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(property);
        }
        
        public void LoadInfoFromPhoton()
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
    }
}
using System;
using _Project.Scripts.Bootstrap;
using _Project.Scripts.GameLogic.Rendering;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.PlayerPlace
{
    public class PlaceInfo : MonoBehaviourPun
    {
        [SerializeField] private BloomPoint _greenButton;
        [SerializeField] private BloomPoint _yellowButton;
        
        [Inject] private NetworkCallBacks _networkCallBacks;
        
        [field: SerializeField] public PlaceInfo PreviousPlace { get; private set; }
        [field: SerializeField] public PlaceInfo NextPlace { get; private set; }
        [field: SerializeField] public int NumberPlace { get; private set; }
        [field: SerializeField] public Transform PlayerTransform { get; set; }

        private bool _isFree = true;

        public bool IsFree
        {
            get => _isFree;
            set
            {
                _isFree = value;
                photonView.RPC("SyncIsFree", RpcTarget.Others, value);
            }
        }
        
        private bool _isEnable;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                _greenButton.SetBloomEnabled(_isEnable);
                _yellowButton.SetBloomEnabled(!_isEnable);
                photonView.RPC("SyncIsEnable", RpcTarget.Others, value);
            }
        }

        private void Start()
        {
            _greenButton.SetBloomEnabled(_isEnable);
            _yellowButton.SetBloomEnabled(!_isEnable);

            _networkCallBacks.PlayerEntered += GetLastInfo;
        }

        private void GetLastInfo(Player player)
        {
            if(!PhotonNetwork.IsMasterClient)
               return;
            
            photonView.RPC("SyncIsFree", player, _isFree);
            photonView.RPC("SyncIsEnable", player, _isEnable);
        }

        [PunRPC]
        private void SyncIsFree(bool isFree)
        {
            _isFree = isFree;
        }
        
        [PunRPC]
        private void SyncIsEnable(bool isEnable)
        {
            _isEnable = isEnable;
        }

        private void OnDestroy()
        {
            _networkCallBacks.PlayerEntered -= GetLastInfo;
        }
    }
}
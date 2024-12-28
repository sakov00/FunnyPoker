using _Project.Scripts.GameLogic.Rendering;
using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts.GameLogic.PlayerPlace
{
    public class PlaceInfo : MonoBehaviourPun
    {
        [SerializeField] private BloomPoint _greenButton;
        [SerializeField] private BloomPoint _yellowButton;
        
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
                photonView.RPC("SyncIsFreePlace", RpcTarget.Others, value);
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
                photonView.RPC("SyncIsFreePlace", RpcTarget.Others, value);
            }
        }

        private void Start()
        {
            _greenButton.SetBloomEnabled(_isEnable);
            _yellowButton.SetBloomEnabled(!_isEnable);
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
    }
}
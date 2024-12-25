using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts.GameLogic.Player
{
    public class PlayerPlaceInfo : MonoBehaviourPun
    {
        [SerializeField] private GameObject _redButton;
        [SerializeField] private GameObject _yellowButton;
        
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public Transform PlayerPlace { get; set; }

        private bool _isFreePlace = true;
        public bool IsFreePlace
        {
            get
            {
                return _isFreePlace;
            }
            set
            {
                _isFreePlace = value;
                photonView.RPC("SyncIsFreePlace", RpcTarget.Others, value);
            }
        }
        
        [PunRPC]
        private void SyncIsFreePlace(bool isFreePlace)
        {
            IsFreePlace = isFreePlace;
        }
    }
}
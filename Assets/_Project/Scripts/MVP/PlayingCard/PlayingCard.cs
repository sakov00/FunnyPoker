using _Project.Scripts.Enums;
using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts.MVP.PlayingCard
{
    public class PlayingCard : MonoBehaviour
    {
        [field: SerializeField] public PlayingCardRank Rank { get; set; }
        [field: SerializeField] public PlayingCardSuit Suit { get; set; }
        [field: SerializeField] public PhotonView PhotonView { get; set; }

        private void OnValidate()
        {
            if (PhotonView == null)
                PhotonView = GetComponent<PhotonView>();
        }
    }
}
using _Project.Scripts.Enums;
using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts.MVP.Cards
{
    public class CardData : MonoBehaviourPun
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public PlayingCardRank Rank { get; set; }
        [field: SerializeField] public PlayingCardSuit Suit { get; set; }
    }
}
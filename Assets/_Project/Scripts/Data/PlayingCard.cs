using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.Scripts.Data
{
    public class PlayingCard : MonoBehaviour
    {
        [field: SerializeField] public PlayingCardRank Rank { get; set; }
        [field: SerializeField]public PlayingCardSuit Suit { get; set; }
    }
}
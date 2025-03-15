using System;
using _Project.Scripts.Enums;
using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts.MVP.Cards
{
    [Serializable]
    public class CardData
    {
        [SerializeField] public int id;
        [SerializeField] public PlayingCardRank rank;
        [SerializeField] public PlayingCardSuit suit;
    }
}
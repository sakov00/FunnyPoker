using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts.MVP.Place
{
    [Serializable]
    public class PlaceData
    {
        [field: SerializeField] public int Number { get; private set; }
        
        [field: SerializeField] public PlacePresenter Previous { get; private set; }
        [field: SerializeField] public PlacePresenter Next { get; private set; }
        
        [field: SerializeField] public Transform PlayerPoint { get; private set; }
        [field: SerializeField] public PhotonView CardsParent { get; private set; }
        
        [field: SerializeField] public List<Transform> CardPoints { get; set; } = new();
        [field: SerializeField] public List<PlayingCard.PlayingCard> PlayingCards { get; set; } = new();
    }
}
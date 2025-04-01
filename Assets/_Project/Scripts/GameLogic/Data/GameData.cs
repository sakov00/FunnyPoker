using System.Collections.Generic;
using _Project.Scripts.MVP.Cards;
using _Project.Scripts.MVP.Place;
using _Project.Scripts.MVP.Table;
using UnityEngine;

namespace _Project.Scripts.GameLogic.Data
{
    public class GameData : MonoBehaviour
    {
        [field: SerializeField] public Transform DealerCardsParent { get; private set; }
        [field: SerializeField] public List<PlacePresenter> AllPlayerPlaces { get; private set; }
        [field: SerializeField] public List<CardPresenter> AllPlayingCards { get; private set; } = new();
        [field: SerializeField] public TablePresenter TablePresenter { get; private set; }
    }
}
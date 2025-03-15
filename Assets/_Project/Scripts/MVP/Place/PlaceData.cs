using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts.MVP.Place
{
    [Serializable]
    public class PlaceData
    {
        [SerializeField] public int id;
        [SerializeField] public PlacePresenter previous;
        [SerializeField] public PlacePresenter next;
        [SerializeField] public Transform playerPoint;
        [SerializeField] public Transform playerCardsParent;
        [SerializeField] public List<Transform> cardPoints;
    }
}
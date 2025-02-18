using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts.MVP.Place
{
    [Serializable]
    public class PlaceData : MonoBehaviour
    {
        [field: SerializeField] public int Id { get; private set; }
        
        [field: SerializeField] public PlacePresenter Previous { get; private set; }
        [field: SerializeField] public PlacePresenter Next { get; private set; }
        
        [field: SerializeField] public Transform PlayerPoint { get; private set; }
        [field: SerializeField] public Transform ParentCards { get; private set; }
        
        [field: SerializeField] public List<Transform> CardPoints { get; set; } = new();

    }
}
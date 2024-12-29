using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.GameLogic.PlayerPlace;
using UnityEngine;

namespace _Project.Scripts.Services.Game
{
    public class ServicePlaces : MonoBehaviour
    {
        [field: SerializeField] public List<PlaceInfo> AllPlayerPlaces { get; private set; }

        public void ActivateRandomPlace()
        {
            var random = Random.Range(0, AllPlayerPlaces.Count);
            AllPlayerPlaces.ElementAt(random).IsEnableSync = true;
        }
    }
}
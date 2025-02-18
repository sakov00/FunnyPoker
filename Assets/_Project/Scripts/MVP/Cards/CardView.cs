using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MVP.Cards
{
    public class CardView : MonoBehaviour
    {
        [Inject] private PlacesManager _placesManager;
        
        public void UpdateCardOwner(int ownerPlaceId)
        {
            var ownerPlace = _placesManager.AllPlayerPlaces[ownerPlaceId];
            transform.SetParent(ownerPlace.Data.ParentCards);
            var freeCardPlace = ownerPlace.GetLastOccupiedCardPlace();
            transform.localPosition = freeCardPlace.localPosition;
            transform.localRotation = freeCardPlace.localRotation;
        }
    }
}
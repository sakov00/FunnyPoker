using _Project.Scripts.Enums;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MVP.Cards
{
    public class CardView : MonoBehaviour
    {
        public void UpdateCardPosition(Transform parent, Transform point)
        {
            transform.SetParent(parent);
            transform.localPosition = point.localPosition;
            transform.localRotation = point.localRotation;
        }
    }
}
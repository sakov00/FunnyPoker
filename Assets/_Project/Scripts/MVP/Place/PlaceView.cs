using System;
using _Project.Scripts.GameLogic.Rendering;
using UnityEngine;

namespace _Project.Scripts.MVP.Place
{
    [Serializable]
    public class PlaceView : MonoBehaviour
    {
        [SerializeField] private BloomPoint greenButton;
        [SerializeField] private BloomPoint yellowButton;

        public void UpdateButton(bool isEnabled)
        {
            greenButton.SetBloomEnabled(isEnabled);
            yellowButton.SetBloomEnabled(!isEnabled);
        }
        
        public void UpdateCardOwner(Transform parent, Transform point)
        {
            transform.SetParent(parent);
            transform.localPosition = point.localPosition;
            transform.localRotation = point.localRotation;
        }
    }
}
using System;
using _Project.Scripts.GameLogic.Rendering;
using UnityEngine;

namespace _Project.Scripts.MVP.Views
{
    [Serializable]
    public class PlaceView
    {
        [SerializeField] private BloomPoint _greenButton;
        [SerializeField] private BloomPoint _yellowButton;

        public void UpdateView(bool isEnabled)
        {
            _greenButton.SetBloomEnabled(isEnabled);
            _yellowButton.SetBloomEnabled(!isEnabled);
        }
    }
}
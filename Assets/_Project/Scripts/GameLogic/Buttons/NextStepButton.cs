using System;
using _Project.Scripts.Interfaces;
using UnityEngine;

namespace _Project.Scripts.GameLogic.Buttons
{
    public class NextStepButton : MonoBehaviour, IClickable
    {
        public event Action OnClicked;
        
        public void OnClick()
        {
            OnClicked?.Invoke();
        }
    }
}
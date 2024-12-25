using _Project.Scripts.Services.Game;
using _Project.Scripts.Services.Network;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameLogic.Buttons
{
    public class NextStepButton : MonoBehaviour, IClickable
    {
        [Inject] QueuePlayerController _queuePlayerController;
        
        public void OnClick()
        {
            // _queuePlayerController.NextPlayer();
        }
    }
}
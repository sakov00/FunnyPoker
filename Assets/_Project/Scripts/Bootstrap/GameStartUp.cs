using _Project.Scripts.Services.Network;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.Bootstrap
{
    public class GameStartUp : MonoBehaviour
    {
        [Inject] private PlayersInfoInRoomService _playersInfoInRoomService;
        
        public void StartGame()
        {
            
        }
    }
}
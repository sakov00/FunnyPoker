using UnityEngine;

namespace _Project.Scripts.GameLogic.Player
{
    public class PlayerActivity : MonoBehaviour
    {
        [SerializeField] private PlayerRaycastClick _playerRaycastClick;
        
        public void EnableActivity()
        {
            _playerRaycastClick.IsEnableRaycast = true;
        }
        
        public void DisableActivity()
        {
            _playerRaycastClick.IsEnableRaycast = false;
        }
    }
}
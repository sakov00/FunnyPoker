using UnityEngine;

namespace _Project.Scripts.GameLogic.Player
{
    public class PlayerRaycastClick : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        public bool IsEnableRaycast { get; set; }
        
        void Update()
        {
            if(!IsEnableRaycast)
              return;
            
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.TryGetComponent(out IClickable clickable))
                    {
                        clickable.OnClick();
                    }
                }
            }
        }

    }
}
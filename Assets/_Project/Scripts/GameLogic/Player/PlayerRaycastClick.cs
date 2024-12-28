using _Project.Scripts.Interfaces;
using UnityEngine;

namespace _Project.Scripts.GameLogic.Player
{
    public class PlayerRaycastClick : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                    if (hit.collider.TryGetComponent(out IClickable clickable))
                        clickable.OnClick();
            }
        }
    }
}
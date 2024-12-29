using UnityEngine;

namespace _Project.Scripts.GameLogic.PlayerInGame
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float sensitivity = 10.0f;

        [Header("Rotation Limits")] 
        [SerializeField] private float minimumVert = -45.0f;

        [SerializeField] private float maximumVert = 60.0f;
        [SerializeField] private float minimumHor = -90.0f;
        [SerializeField] private float maximumHor = 90.0f;


        private void Start()
        {
            // Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            var mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            var rotationX = transform.localEulerAngles.x - mouseY;
            var rotationY = transform.localEulerAngles.y + mouseX;

            if (rotationX > 180)
                rotationX -= 360;
            else if (rotationX < -180)
                rotationX += 360;

            rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert);
            rotationY = Mathf.Clamp(rotationY, minimumHor, maximumHor);
            transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.parent.Rotate(Vector3.up * rotationY);
        }
    }
}
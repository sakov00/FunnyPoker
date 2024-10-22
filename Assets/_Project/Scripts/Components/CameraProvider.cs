using System;
using UnityEngine;
using Voody.UniLeo.Lite;

namespace Assets._Project.Scripts.Components
{
    public sealed class CameraProvider : MonoProvider<CameraComponent>
    {
        private void Awake()
        {
            value.CameraTransform = transform;
            value.HorizontalRotation = transform.eulerAngles.y;
            value.VerticalRotation = transform.eulerAngles.x;
        }
    }

    [Serializable]
    public struct CameraComponent
    {
        [NonSerialized] public Transform CameraTransform;
        public float Sensitivity;
        [NonSerialized] public float VerticalRotation;
        [NonSerialized] public float HorizontalRotation;
    }
}

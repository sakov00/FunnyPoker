using System;
using UnityEngine;
using Voody.UniLeo.Lite;

namespace Assets._Project.Scripts.Components
{
    public sealed class CameraProvider : MonoProvider<CameraComponent> { }

    [Serializable]
    public struct CameraComponent
    {
        public Transform Transform;
    }
}

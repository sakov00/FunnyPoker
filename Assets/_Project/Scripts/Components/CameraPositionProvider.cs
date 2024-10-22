using System;
using System.Linq;
using UnityEngine;
using Voody.UniLeo.Lite;

namespace Assets._Project.Scripts.Components
{
    public sealed class CameraPositionProvider : MonoProvider<CameraPositionComponent> { }

    [Serializable]
    public struct CameraPositionComponent
    {
        public Transform CameraPosition;
    }
}

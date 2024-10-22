using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

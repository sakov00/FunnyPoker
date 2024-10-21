using System;
using UnityEngine;
using Voody.UniLeo.Lite;

namespace Assets._Project.Scripts.Components
{
    public sealed class CanvasProvider : MonoProvider<CanvasComponent> { }

    [Serializable]
    public struct CanvasComponent
    {
        public GameObject gameObjectCanvas;
    }
}

using System;
using UnityEngine;
using Voody.UniLeo.Lite;

namespace Assets._Project.Scripts.Components
{
    public sealed class SpawnProvider : MonoProvider<SpawnComponent>
    {
        private void Awake()
        {
            value.Transform = GetComponent<Transform>();
        }
    }

    [Serializable]
    public struct SpawnComponent
    {  
        [NonSerialized] public Transform Transform;
        public int TypeObject;
    }
}

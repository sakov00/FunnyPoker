using UnityEngine;
using Voody.UniLeo.Lite;

namespace Assets._Project.Scripts.Components
{
    public sealed class GameObjectProvider : MonoProvider<GameObjectComponent>
    {
        private void Awake()
        {
            value.GameObject = gameObject;
        }
    }

    public struct GameObjectComponent
    {
        public GameObject GameObject;
    }
}

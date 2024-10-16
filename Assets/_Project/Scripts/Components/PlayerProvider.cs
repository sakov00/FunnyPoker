using System;
using System.Collections.Generic;
using Voody.UniLeo.Lite;

namespace Assets._Project.Scripts.Components
{
    public sealed class PlayerProvider : MonoProvider<PlayerComponent> { }

    [Serializable]
    public struct PlayerComponent 
    {
        public int HealthPoints;
        public int CountLimitCards;
        [NonSerialized] public List<int> ListCardEntities;
        [NonSerialized] public int CameraEntity;
    }
}

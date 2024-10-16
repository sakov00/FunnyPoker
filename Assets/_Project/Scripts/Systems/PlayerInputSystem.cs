using Assets._Project.Scripts.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Assets._Project.Scripts.Systems
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        EcsFilter filter;
        EcsPool<PlayerComponent> players;

        public void Run(EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            filter = world.Filter<PlayerComponent>().End();
            players = world.GetPool<PlayerComponent>();

            foreach (int entityIndex in filter)
            {
                ref PlayerComponent playerComponent = ref players.Get(entityIndex);
                Debug.Log(playerComponent.HealthPoints);
            }
        }
    }
}

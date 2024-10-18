using Assets._Project.Scripts.Components;
using Assets._Project.Scripts.Factories;
using Assets._Project.Scripts.Interfaces;
using Leopotam.EcsLite;
using Zenject;

namespace Assets._Project.Scripts.Systems
{
    public class PlayerSpawnSystem : IEcsInitSystem, IEcsFixedUpdateSystem
    {
        [Inject] private PlayerFactory playerFactory;

        private EcsFilter filter;
        private EcsPool<SpawnComponent> spawnPoints;

        public void Init(EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            filter = world.Filter<SpawnComponent>().End();
            spawnPoints = world.GetPool<SpawnComponent>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (int entityIndex in filter)
            {
                ref SpawnComponent playerComponent = ref spawnPoints.Get(entityIndex);
                playerFactory.CreatePlayer(playerComponent.Transform.position, playerComponent.Transform.rotation);
                spawnPoints.Del(entityIndex);
            }
        }
    }
}

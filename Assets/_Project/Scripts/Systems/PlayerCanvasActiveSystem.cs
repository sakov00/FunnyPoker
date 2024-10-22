using Assets._Project.Scripts.Components;
using Assets._Project.Scripts.Interfaces;
using Assets._Project.Scripts.MonoBehLogic;
using Leopotam.EcsLite;
using Photon.Pun;
using Zenject;

namespace Assets._Project.Scripts.Systems
{
    public class PlayerCanvasActiveSystem : IEcsFixedUpdateSystem
    {
        [Inject] private PlayersTurnService playersTurnService;

        private EcsWorld world;
        private EcsFilter filter;
        private EcsPool<CanvasComponent> canvasPool;

        public void Run(EcsSystems systems)
        {
            world = systems.GetWorld();
            canvasPool = world.GetPool<CanvasComponent>();
            filter = world.Filter<CanvasComponent>().End();

            foreach (int entityIndex in filter)
            {
                ref var canvasComponent = ref canvasPool.Get(entityIndex);

                var IsTurn = playersTurnService.IsCurrentPlayerActorNumber(PhotonNetwork.LocalPlayer.ActorNumber);
                canvasComponent.gameObjectCanvas.SetActive(IsTurn);
            }
        }
    }
}

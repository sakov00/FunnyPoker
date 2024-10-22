using Assets._Project.Scripts.Components;
using Assets._Project.Scripts.Interfaces;
using Leopotam.EcsLite;
using UnityEngine;

namespace Assets._Project.Scripts.Systems
{
    public class CameraSystem : IEcsUpdateSystem
    {
        private EcsWorld world;
        private EcsFilter filter;
        private EcsPool<CameraComponent> cameraPool;

        public void Run(EcsSystems systems)
        {
            world = systems.GetWorld();
            cameraPool = world.GetPool<CameraComponent>();
            filter = world.Filter<CameraComponent>().End();

            foreach (int entityIndex in filter)
            {
                ref var cameraComponent = ref cameraPool.Get(entityIndex);

                float mouseX = Input.GetAxis("Mouse X") * cameraComponent.Sensitivity;
                float mouseY = Input.GetAxis("Mouse Y") * cameraComponent.Sensitivity;

                cameraComponent.HorizontalRotation += mouseX;
                cameraComponent.VerticalRotation -= mouseY;
                cameraComponent.VerticalRotation = Mathf.Clamp(cameraComponent.VerticalRotation, -90f, 90f);

                cameraComponent.CameraTransform.rotation = Quaternion.Euler(cameraComponent.VerticalRotation, cameraComponent.HorizontalRotation, 0f);
            }
        }
    }
}

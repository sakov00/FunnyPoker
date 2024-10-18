using Assets._Project.Scripts.Interfaces;
using Assets._Project.Scripts.Systems;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using Voody.UniLeo.Lite;
using Zenject;

namespace Assets._Project.Scripts.Bootstrap
{
    public class EcsGameStartUp : MonoBehaviour, IInitializable
    {
        [Inject] private EcsWorld world;
        [Inject] private List<IEcsInitSystem> injectedInitSystems;
        [Inject] private List<IEcsFixedUpdateSystem> injectedFixedUpdateSystems;
        [Inject] private List<IEcsUpdateSystem> injectedUpdateSystems;
        [Inject] private List<IEcsLateUpdateSystem> injectedLateUpdateSystems;

        private EcsSystems initSystems;
        private EcsSystems fixedUpdateSystems;
        private EcsSystems updateSystems;
        private EcsSystems lateUpdateSystems;

        public void Initialize()
        {
            DeclareInitSystems();
            DeclareFixedUpdateSystems();
            DeclareUpdateSystems();
            DeclareLateUpdateSystems();
        }

        private void DeclareInitSystems()
        {
            initSystems = new EcsSystems(world);

            foreach (var system in injectedInitSystems)
            {
                initSystems.Add(system);
            }

            initSystems.Init();
        }

        private void DeclareFixedUpdateSystems()
        {
            fixedUpdateSystems = new EcsSystems(world);
            fixedUpdateSystems.ConvertScene();

            foreach (var system in injectedFixedUpdateSystems)
            {
                fixedUpdateSystems.Add(system);
            }

            fixedUpdateSystems.Init();
        }

        private void DeclareUpdateSystems()
        {
            updateSystems = new EcsSystems(world);

            foreach (var system in injectedUpdateSystems)
            {
                updateSystems.Add(system);
            }

            updateSystems.Init();
        }

        private void DeclareLateUpdateSystems()
        {
            lateUpdateSystems = new EcsSystems(world);

            foreach (var system in injectedLateUpdateSystems)
            {
                lateUpdateSystems.Add(system);
            }

            lateUpdateSystems.Init();
        }

        private void FixedUpdate()
            => fixedUpdateSystems?.Run();

        private void Update()
            => updateSystems?.Run();

        private void LateUpdate()
            => lateUpdateSystems?.Run();

        private void OnDestroy()
        {
            initSystems.Destroy();
            fixedUpdateSystems.Destroy();
            updateSystems.Destroy();
            lateUpdateSystems.Destroy();

            world.Destroy();
        }
    }
}
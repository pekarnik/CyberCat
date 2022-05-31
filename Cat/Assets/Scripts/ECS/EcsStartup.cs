using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client
{
    sealed class EcsStartup : MonoBehaviour
    {

        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsSystems _initSystems;
        private EcsSystems _fixedSystems;

        void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _fixedSystems = new EcsSystems(_world);
            _initSystems = new EcsSystems(_world);
            
            _systems
                .Add(new Systems.KeyboardInput())
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();
            
            _fixedSystems
                .Add(new Systems.Move())
                .Init();
            
            _initSystems
                .Add(new Systems.InitPlayer())
                .Init();
        }

        void Update()
        {
            _systems?.Run();
        }

        private void FixedUpdate()
        {
            _fixedSystems?.Run();
        }

        void OnDestroy()
        {
            _systems.Destroy();
            _systems = null;
            _fixedSystems.Destroy();
            _fixedSystems = null;
            _initSystems.Destroy();
            _initSystems = null;
            _world.Destroy();
            _world = null;
        }
    }
}
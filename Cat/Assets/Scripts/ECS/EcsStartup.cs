using System;
using Leopotam.EcsLite;
using UnityEngine;
using Voody.UniLeo.Lite;

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
            
            _initSystems
                .Add(new Systems.InitPlayer())
                .Add(new Systems.InitNpc())
                // .Add(new Systems.InitCamera())
                .Init();
            
            _systems
                .ConvertScene()
                .Add(new Systems.KeyboardInput())
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();
            
            _fixedSystems
                .ConvertScene()
                .Add(new Systems.Move())
                .Add(new Systems.MoveCamera())
                .Add(new Systems.Attack())
                .Add(new Systems.TriggerArea())
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
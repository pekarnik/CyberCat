using Components;
using Leopotam.EcsLite;
using UnityEngine;
using Utils;
using System.Collections.Generic;

namespace Systems {

    sealed class Spawn : IEcsRunSystem, IEcsInitSystem {
        
        private SpawnableConfig[] spawnables = {
            new SpawnableConfig(
                "Prefabs/Enemies/Enemy",
                "spawner_init",
                15
            )
        };

        public void Init (EcsSystems systems) {
            var world = systems.GetWorld();

            var spawnablePool = world.GetPool<Components.Spawnable>();

            foreach (SpawnableConfig entry in spawnables) {
                var entity = world.NewEntity();
                
                spawnablePool.Add(entity);

                ref var spawnableComponent = ref spawnablePool.Get(entity);

                spawnableComponent.active = entry.active;
                spawnableComponent.prefab = (GameObject) Resources.Load(entry.prefabLocation);
                spawnableComponent.spawnerId = entry.spawnerId;
                spawnableComponent.spawnInterval = entry.spawnInterval;
                spawnableComponent.spawnLimit = entry.spawnLimit;
            }
        }

        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<Components.Spawner>().End();
            var spawnerPool = world.GetPool<Components.Spawner>();

            foreach (var entity in filter) {
                ref var spawnerComponent = ref spawnerPool.Get(entity);

                Debug.Log(spawnerComponent.spawnerId);
            }
        }
    }
}
using Components;
using Leopotam.EcsLite;
using UnityEngine;
using Utils;
using System.Collections.Generic;
using System;

namespace Systems {

    sealed class Spawn : IEcsRunSystem, IEcsInitSystem {
        
        private SpawnableConfig[] spawnables = {
            new SpawnableConfig(
                "Prefabs/Enemies/Enemy",
                "spawner_init",
                2,
                0,
                true
            )
        };

        private Dictionary<int, Vector3> sideOffsets = new Dictionary<int, Vector3>{
            {0, new Vector3(3, 1, 0)},
            {1, new Vector3(-3, 1, 0)},
            {2, new Vector3(0, 1, 3)},
            {3, new Vector3(0, 1, -3)},
        };

        private Dictionary<string, int> spawners = new Dictionary<string, int>();

        public void Init (EcsSystems systems) {
            var world = systems.GetWorld();

            var spawnablePool = world.GetPool<Components.Spawnable>();
            var spawnerFilter = world.Filter<Components.Spawner>().End();
            var spawnerPool = world.GetPool<Components.Spawner>();
            foreach (var spawnerEntity in spawnerFilter) {
                var spawnerComponent = spawnerPool.Get(spawnerEntity);
                if (!spawners.ContainsKey(spawnerComponent.spawnerId)) {
                    spawners.Add(spawnerComponent.spawnerId, spawnerEntity);
                }
            }

            foreach (SpawnableConfig entry in spawnables) {
                var entity = world.NewEntity();
                
                spawnablePool.Add(entity);

                ref var spawnableComponent = ref spawnablePool.Get(entity);
                
                spawners.TryGetValue(entry.spawnerId, out int spawnerEntity);
                spawnableComponent.active = entry.active;
                spawnableComponent.prefab = (GameObject) Resources.Load(entry.prefabLocation);
                spawnableComponent.spawnerEntity = spawnerEntity;
                spawnableComponent.spawnInterval = entry.spawnInterval;
                spawnableComponent.spawnLimit = entry.spawnLimit;
                spawnableComponent.lastSpawnedAt = null;
            }
        }

        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<Components.Spawnable>().End();
            var spawnablePool = world.GetPool<Components.Spawnable>();
            var spawnerPool = world.GetPool<Components.Spawner>();

            foreach (var entity in filter) {
                ref var spawnableComponent = ref spawnablePool.Get(entity);

                bool ableToSpawn = (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - spawnableComponent.lastSpawnedAt > spawnableComponent.spawnInterval) || spawnableComponent.lastSpawnedAt == null;

                if (ableToSpawn) {
                    var spawnerComponent = spawnerPool.Get(spawnableComponent.spawnerEntity);
                    
                    int side = UnityEngine.Random.Range(0, 4);

                    sideOffsets.TryGetValue(side, out Vector3 sideOffset);

                    Vector3 targetSpawnPoint = spawnerComponent.transform.position + sideOffset;
                    GameObject.Instantiate(spawnableComponent.prefab, targetSpawnPoint, Quaternion.Euler(0, 0, 0));

                    spawnableComponent.lastSpawnedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                }
            }
        }
    }
}
using Components;
using Leopotam.EcsLite;
using UnityEngine;
using Utils;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Systems {

    sealed class Spawn : IEcsRunSystem, IEcsInitSystem {
        
        private SpawnableConfig[] spawnables = {
            new SpawnableConfig(
                "Prefabs/Enemies/Enemy",
                "SPAWNER_1",
                "SPAWNER_1_PATH",
                2,
                1,
                true
            )
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
                spawners.TryGetValue(entry.spawnerId, out int spawnerEntity);
                ref var spawnerComponent = ref spawnerPool.Get(spawnerEntity);
                ref var spawnableComponent = ref spawnablePool.Get(entity);

                GameObject spawnerPath = spawnerComponent.spawnerPathObject;
                List<Transform> spawnerPathPoints = spawnerPath.GetComponentsInChildren<Transform>().ToList();
                spawnerPathPoints.RemoveAt(0);
                spawnerPathPoints.Sort((a, b) => int.Parse(a.name).CompareTo(int.Parse(b.name)));

                List<PathPointModel> sortedPathPoints = spawnerPathPoints.Select(x => {
                    PathPointModel point = new PathPointModel();
                    point.position = x.transform.position;
                    point.interactive = x.GetComponent<InteractivePathPointController>();

                    return point;
                }).ToList();

                var points = sortedPathPoints;
                
                spawnerComponent.active = entry.active;
                spawnerComponent.spawnObject = (GameObject) Resources.Load(entry.prefabLocation);
                spawnableComponent.navigationPoints = points.ToArray();
                spawnableComponent.spawnerEntity = spawnerEntity;
                spawnerComponent.spawnInterval = entry.spawnInterval;
                spawnerComponent.spawnLimit = entry.spawnLimit;
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
                ref var spawnerComponent = ref spawnerPool.Get(spawnableComponent.spawnerEntity);

                bool ableToSpawn = ((DateTimeOffset.UtcNow.ToUnixTimeSeconds() - spawnableComponent.lastSpawnedAt > spawnerComponent.spawnInterval)
                    || spawnableComponent.lastSpawnedAt == null)
                    && (spawnerComponent.currentSpawned < spawnerComponent.spawnLimit && spawnerComponent.spawnLimit > 0);

                if (ableToSpawn) {
                    Vector3 targetSpawnPoint = spawnerComponent.transform.position;            
                    GameObject spawnedObject = GameObject.Instantiate(spawnerComponent.spawnObject, targetSpawnPoint, Quaternion.Euler(0, 0, 0));
                    spawnedObject.transform.position = targetSpawnPoint;
                    NpcMovementController agent = spawnedObject.GetComponent<NpcMovementController>();
                    agent.SetPositions(spawnableComponent.navigationPoints);
                    spawnerComponent.currentSpawned++;
                    spawnableComponent.lastSpawnedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                }
            }
        }
    }
}
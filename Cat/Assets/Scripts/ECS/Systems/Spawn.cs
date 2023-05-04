using Components;
using Leopotam.EcsLite;
using UnityEngine;
using Utils;
using System.Collections.Generic;
using System;
using System.Linq;
using EventManager;

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
                var spawnerComponent = spawnerPool.Get(spawnerEntity);
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
                
                spawnableComponent.navigationPoints = points.ToArray();
                spawnableComponent.spawnerEntity = spawnerEntity;
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

                bool isDayTimePhaseActive = (spawnerComponent.activePhase == DayTimeEventManager.CurrentDayTimeState || spawnerComponent.activePhase == DayTimeState.NONE);
                bool isSpawnLimitReached = ((spawnerComponent.currentSpawned < spawnerComponent.spawnLimit && spawnerComponent.spawnLimit > 0) || spawnerComponent.spawnLimit == 0);
                bool isSpawnTimeoutReached = ((DateTimeOffset.UtcNow.ToUnixTimeSeconds() - spawnableComponent.lastSpawnedAt > spawnerComponent.spawnInterval)
                    || spawnableComponent.lastSpawnedAt == null);

                bool ableToSpawn = isSpawnTimeoutReached && isSpawnLimitReached && isDayTimePhaseActive && spawnerComponent.active;

                bool shouldDespawned = DayTimeEventManager.PreviousDayTimeState == spawnerComponent.activePhase 
                    && DayTimeEventManager.CurrentDayTimeState != spawnerComponent.activePhase && spawnerComponent.currentSpawned > 0;

                if (ableToSpawn) {
                    Vector3 targetSpawnPoint = spawnerComponent.transform.position;            
                    GameObject spawnedObject = GameObject.Instantiate(spawnerComponent.spawnObject, targetSpawnPoint, Quaternion.Euler(0, 0, 0));
                    spawnedObject.transform.position = targetSpawnPoint;
                    spawnerComponent.spawnedObjects.Add(spawnedObject);
                    NpcMovementController agent = spawnedObject.GetComponent<NpcMovementController>();
                    agent.SetPositions(spawnableComponent.navigationPoints);
                    spawnerComponent.currentSpawned++;
                    spawnableComponent.lastSpawnedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                }

                if (shouldDespawned) {
                    spawnerComponent.currentSpawned = 0;
                    foreach (GameObject objectToDespawn in spawnerComponent.spawnedObjects) {
                        GameObject.Destroy(objectToDespawn);
                    }
                }
            }
        }
    }
}
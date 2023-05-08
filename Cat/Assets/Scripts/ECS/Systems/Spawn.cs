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

            foreach (var entry in spawners) {
                ref var spawnerComponent = ref spawnerPool.Get(entry.Value);


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
                
                spawnerComponent.navigationPoints = points.ToArray();
                spawnerComponent.lastSpawnedAt = null;
            }
        }

        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<Components.Spawner>().End();
            var spawnablePool = world.GetPool<Components.Spawnable>();
            var spawnerPool = world.GetPool<Components.Spawner>();

            foreach (var entity in filter) {
                ref var spawnerComponent = ref spawnerPool.Get(entity);

                bool isDayTimePhaseActive = (spawnerComponent.activePhase == DayTimeEventManager.CurrentDayTimeState || spawnerComponent.activePhase == DayTimeState.NONE);
                bool isSpawnLimitReached = ((spawnerComponent.currentSpawned < spawnerComponent.spawnLimit && spawnerComponent.spawnLimit > 0) || spawnerComponent.spawnLimit == 0);
                bool isSpawnTimeoutReached = ((DateTimeOffset.UtcNow.ToUnixTimeSeconds() - spawnerComponent.lastSpawnedAt > spawnerComponent.spawnInterval)
                    || spawnerComponent.lastSpawnedAt == null);

                bool ableToSpawn = isSpawnTimeoutReached && isSpawnLimitReached && isDayTimePhaseActive && spawnerComponent.active;

                bool shouldDespawned = DayTimeEventManager.PreviousDayTimeState == spawnerComponent.activePhase 
                    && DayTimeEventManager.CurrentDayTimeState != spawnerComponent.activePhase && spawnerComponent.currentSpawned > 0;

                if (ableToSpawn) {
                    Vector3 targetSpawnPoint = spawnerComponent.transform.position;            
                    GameObject spawnedObject = GameObject.Instantiate(spawnerComponent.spawnObject, targetSpawnPoint, Quaternion.Euler(0, 0, 0));
                    spawnedObject.transform.position = targetSpawnPoint;
                    spawnerComponent.spawnedObjects.Add(spawnedObject);
                    NpcMovementController agent = spawnedObject.GetComponent<NpcMovementController>();;
                    agent.SetPositions(spawnerComponent.navigationPoints);
                    spawnerComponent.currentSpawned++;
                    spawnerComponent.lastSpawnedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
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
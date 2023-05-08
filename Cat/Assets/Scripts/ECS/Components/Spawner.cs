using UnityEngine;
using System;
using System.Collections.Generic;

namespace Components {
    [Serializable]
    public struct Spawner
    {
        public Transform transform;
        public string spawnerId;
        public GameObject spawnerPathObject;

        public GameObject spawnObject;

        public int spawnInterval;

        public int spawnLimit;

        public bool active;

        public int currentSpawned;

        public DayTimeState activePhase;

        public List<GameObject> spawnedObjects;

        public PathPointModel[] navigationPoints;

        public long? lastSpawnedAt;
    }
}

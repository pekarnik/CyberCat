using UnityEngine;
using System;

namespace Components {
    [Serializable]
    public struct Spawnable
    {
        public int spawnerEntity;

        public Vector3[] navigationPoints;

        public long? lastSpawnedAt;
    }

    public struct SpawnableConfig {

        public string prefabLocation;
        public string spawnerId;
        public string spawnerPathComponentName;
        public int spawnInterval;
        public int spawnLimit;
        public bool active;

        public SpawnableConfig(string prefabLocation, string spawnerId, string spawnerPathComponentName, int interval, int spawnLimit = 0, bool active = false) {
            this.active = active;
            this.prefabLocation = prefabLocation;
            this.spawnInterval = interval;
            this.spawnLimit = spawnLimit;
            this.spawnerId = spawnerId;
            this.spawnerPathComponentName = spawnerPathComponentName;
        }
    }
}



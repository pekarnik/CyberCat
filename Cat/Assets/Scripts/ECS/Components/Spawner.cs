using UnityEngine;
using System;

namespace Components {
    [Serializable]
    public struct Spawner
    {
        public Transform transform;
        public string spawnerId;
        public string spawnerPathComponent;

        public GameObject prefab;

        public int spawnInterval;

        public int spawnLimit;

        public bool active;

        public int currentSpawned;
    }
}

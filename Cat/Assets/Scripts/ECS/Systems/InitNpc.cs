using Client;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class InitNpc : IEcsInitSystem {
        public void Init (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var npcEntity = world.NewEntity();

            var physicalObjectPool = world.GetPool<Components.PhysicalObject>();
            var triggerAreaPool = world.GetPool<Components.TriggerArea>();

            physicalObjectPool.Add(npcEntity);
            triggerAreaPool.Add(npcEntity);

            ref var triggerAreaComponent = ref triggerAreaPool.Get(npcEntity);
            ref var physicalObjectComponent = ref physicalObjectPool.Get(npcEntity);

            GameObject player = GameObject.FindGameObjectWithTag("NPC");
            physicalObjectComponent.rigidbody = player.GetComponent<Rigidbody>();
            
            triggerAreaComponent.angle = 45f;
            triggerAreaComponent.maxDistance = 10f;
            triggerAreaComponent.maxRadius = 3f;
        }
    }
}
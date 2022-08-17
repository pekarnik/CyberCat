using Client;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class InitPlayer : IEcsInitSystem {
        public void Init (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var playerEntity = world.NewEntity();

            var movablePool = world.GetPool<Components.Movable>();
            var physicalObjectPool = world.GetPool<Components.PhysicalObject>();
            var moveDirectionPool = world.GetPool<Components.MoveDirection>();
            var cameraLeadPool = world.GetPool<Components.CameraLead>();
            var attackerPool = world.GetPool<Components.Attacker>();

            movablePool.Add(playerEntity);
            moveDirectionPool.Add(playerEntity);
            cameraLeadPool.Add(playerEntity);
            physicalObjectPool.Add(playerEntity);
            attackerPool.Add(playerEntity);

            ref var movableComponent = ref movablePool.Get(playerEntity);
            ref var cameraLeadComponent = ref cameraLeadPool.Get(playerEntity);
            ref var physicalObjectComponent = ref physicalObjectPool.Get(playerEntity);
            ref var attackerComponent = ref attackerPool.Get(playerEntity);

            cameraLeadComponent.leaderTag = "Player";
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            attackerComponent.attackSource = player.transform.Find("AttackSource").transform;
            physicalObjectComponent.rigidbody = player.GetComponent<Rigidbody>();
            movableComponent.moveSpeed = 5.0f;
        }
    }
}
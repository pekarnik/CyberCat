using Client;
using Leopotam.EcsLite;
using UnityEngine;
using 

namespace Systems {
    sealed class InitPlayer : IEcsInitSystem {
        public void Init (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var playerEntity = world.NewEntity();

            var movablePool = world.GetPool<Components.Movable>();
            var physicalObjectPool = world.GetPool<Components.PhysicalObject>();
            var jumpPlayerPool = world.GetPool<Components.JumpPlayer>();
            var moveDirectionPool = world.GetPool<Components.MoveDirection>();
            var cameraLeadPool = world.GetPool<Components.CameraLead>();
            var attackerPool = world.GetPool<Components.Attacker>();

            movablePool.Add(playerEntity);
            moveDirectionPool.Add(playerEntity);
            cameraLeadPool.Add(playerEntity);
            physicalObjectPool.Add(playerEntity);
            attackerPool.Add(playerEntity);
            jumpPlayerPool.Add(playerEntity);

            ref var movableComponent = ref movablePool.Get(playerEntity);
            ref var cameraLeadComponent = ref cameraLeadPool.Get(playerEntity);
            ref var physicalObjectComponent = ref physicalObjectPool.Get(playerEntity);
            ref var attackerComponent = ref attackerPool.Get(playerEntity);
            ref var jumpPlayerComponent = ref jumpPlayerPool.Get(playerEntity);

            cameraLeadComponent.leaderTag = "Player";
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            attackerComponent.attackSource = player.transform.Find("AttackSource").transform;
            physicalObjectComponent.rigidbody = player.GetComponent<Rigidbody>();
            physicalObjectComponent.meshFilter = player.GetComponent<MeshFilter>();
            movableComponent.moveSpeed = Components.Movable.DEFAULT_MOVE_SPEED;
            jumpPlayerComponent.jumpHeight = 10.0f;
        }
    }
}
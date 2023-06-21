using Assets.Scripts.ECS.Components;
using Client;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class InitPlayer : IEcsInitSystem {
        public void Init (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var playerEntity = world.NewEntity();

            var movablePool = world.GetPool<Movable>();
            var physicalObjectPool = world.GetPool<PhysicalObject>();
            var jumpPlayerPool = world.GetPool<Components.JumpPlayer>();
            var moveDirectionPool = world.GetPool<MoveDirection>();
            var cameraLeadPool = world.GetPool<CameraLead>();
            var attackerPool = world.GetPool<Components.Attacker>();
            var animatorPool = world.GetPool<AnimatorComponent>();
            var initializeEntityRequestPool = world.GetPool<InitializeEntityRequestComponent>();
            var movePlayerDirectionPool = world.GetPool<MovePlayerDirection>();

            movablePool.Add(playerEntity);
            moveDirectionPool.Add(playerEntity);
            cameraLeadPool.Add(playerEntity);
            physicalObjectPool.Add(playerEntity);
            attackerPool.Add(playerEntity);
            jumpPlayerPool.Add(playerEntity);
            animatorPool.Add(playerEntity);
            initializeEntityRequestPool.Add(playerEntity);
            movePlayerDirectionPool.Add(playerEntity);

            ref var movableComponent = ref movablePool.Get(playerEntity);
            ref var cameraLeadComponent = ref cameraLeadPool.Get(playerEntity);
            ref var physicalObjectComponent = ref physicalObjectPool.Get(playerEntity);
            ref var attackerComponent = ref attackerPool.Get(playerEntity);
            ref var jumpPlayerComponent = ref jumpPlayerPool.Get(playerEntity);
            ref var animatorComponent = ref animatorPool.Get(playerEntity);
            ref var initializeEntityRequestComponent = ref initializeEntityRequestPool.Get(playerEntity);
            ref var movePlayerDirectionComponent = ref movePlayerDirectionPool.Get(playerEntity);

            cameraLeadComponent.leaderTag = "Player";
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            attackerComponent.attackSource = player.transform.Find("AttackSource").transform;
            physicalObjectComponent.rigidbody = player.GetComponent<Rigidbody>();
            physicalObjectComponent.meshFilter = player.GetComponent<MeshFilter>();
            movableComponent.moveSpeed = Movable.DEFAULT_MOVE_SPEED;
            jumpPlayerComponent.jumpHeight = 10.0f;
            animatorComponent.Animator = player.GetComponentInChildren<Animator>();
            initializeEntityRequestComponent.EntityReference = player.GetComponent<EntityReference>();
            movePlayerDirectionComponent.current = new Vector2(1, 0);
            movePlayerDirectionComponent.old = new Vector2(1, 0);
            movePlayerDirectionComponent.transform = player.transform;
            movePlayerDirectionComponent.currentAngle = 0;
            movePlayerDirectionComponent.oldAngle = 0;
        }
    }
}
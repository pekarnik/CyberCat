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
            var moveDirectionPool = world.GetPool<Components.MoveDirection>();

            movablePool.Add(playerEntity);
            moveDirectionPool.Add(playerEntity);

            ref var movableComponent = ref movablePool.Get(playerEntity);
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            movableComponent.rigidbody = player.GetComponent<Rigidbody>();
            movableComponent.moveSpeed = 5.0f;
        }
    }
}
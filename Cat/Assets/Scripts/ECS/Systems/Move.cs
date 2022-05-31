using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class Move : IEcsRunSystem {        
        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<Components.MoveDirection>().Inc<Components.Movable>().End();
            var movablePool = world.GetPool<Components.Movable>();
            var moveDirectionPool = world.GetPool<Components.MoveDirection>();

            foreach (var entity in filter)
            {
                ref var movableComponent = ref movablePool.Get(entity);
                var moveDirectionComponent = moveDirectionPool.Get(entity);
                
                float moveSpeed = movableComponent.moveSpeed; 

                Vector3 moveDirection = new Vector3(moveDirectionComponent.forward, 0, -moveDirectionComponent.right) * moveSpeed * Time.fixedDeltaTime;
                Vector3 movePosition = movableComponent.rigidbody.position + moveDirection;
                movableComponent.rigidbody.MovePosition(movePosition);
            }
            // add your run code here.
        }
    }
}
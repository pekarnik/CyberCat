using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class Move : IEcsRunSystem {        
        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<Components.MoveDirection>().Inc<Components.Movable>().Inc<Components.PhysicalObject>().End();
            var movablePool = world.GetPool<Components.Movable>();
            var moveDirectionPool = world.GetPool<Components.MoveDirection>();
            var physicalObjectPool = world.GetPool<Components.PhysicalObject>();

            foreach (var entity in filter)
            {
                ref var movableComponent = ref movablePool.Get(entity);
                var moveDirectionComponent = moveDirectionPool.Get(entity);
                var physicalObjectComponent = physicalObjectPool.Get(entity);
                
                float moveSpeed = movableComponent.moveSpeed; 

                Vector3 moveDirection = new Vector3(moveDirectionComponent.forward, 0, -moveDirectionComponent.right) * moveSpeed * Time.fixedDeltaTime;
                Vector3 movePosition = physicalObjectComponent.rigidbody.position + moveDirection;
                physicalObjectComponent.rigidbody.MovePosition(movePosition);
            }
        }
    }
}
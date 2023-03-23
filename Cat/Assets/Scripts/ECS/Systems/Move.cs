using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class Move : IEcsRunSystem {        
        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<Components.MoveDirection>().Inc<Components.Movable>().Inc<Components.PhysicalObject>().End();
            var cameraFilter = world.Filter<Components.RotateCamera>().End();
            var cameraPool = world.GetPool<Components.RotateCamera>();
            var movablePool = world.GetPool<Components.Movable>();
            var moveDirectionPool = world.GetPool<Components.MoveDirection>();
            var physicalObjectPool = world.GetPool<Components.PhysicalObject>();

            var mainCameraEntity = cameraFilter.GetRawEntities()[0];

            foreach (var entity in filter)
            {
                ref var movableComponent = ref movablePool.Get(entity);
                var moveDirectionComponent = moveDirectionPool.Get(entity);
                var physicalObjectComponent = physicalObjectPool.Get(entity);

                var isCameraFollowed = cameraPool.Has(mainCameraEntity);

                float moveSpeed = movableComponent.moveSpeed; 

                Vector3 moveDirection = new Vector3(moveDirectionComponent.right, 0, moveDirectionComponent.forward) * moveSpeed * Time.fixedDeltaTime;
                if (isCameraFollowed) {

                    var mainCameraComponent = cameraPool.Get(mainCameraEntity);
                    

                    if (mainCameraComponent.currentRotation == 90f) {
                        moveDirection = new Vector3(moveDirectionComponent.forward, 0, -moveDirectionComponent.right) * moveSpeed * Time.fixedDeltaTime;
                    }
                    if (mainCameraComponent.currentRotation == 180f) {
                        moveDirection = new Vector3(-moveDirectionComponent.right, 0, -moveDirectionComponent.forward) * moveSpeed * Time.fixedDeltaTime;
                    }
                    if (mainCameraComponent.currentRotation == 270f) {
                        moveDirection = new Vector3(-moveDirectionComponent.forward, 0, moveDirectionComponent.right) * moveSpeed * Time.fixedDeltaTime;
                    }

                    physicalObjectComponent.rigidbody.MoveRotation(Quaternion.Euler(0, mainCameraComponent.currentRotation, 0));
                }

                Vector3 movePosition = physicalObjectComponent.rigidbody.position + moveDirection;
                physicalObjectComponent.rigidbody.MovePosition(movePosition);
            }
        }
    }
}
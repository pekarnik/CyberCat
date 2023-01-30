using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class Move : IEcsRunSystem {       
        private readonly float FORWARD_ROTATION = 0f;
        private readonly float RIGHT_ROTATION = 90f;
        private readonly float BACKWARD_ROTATION = 180f;
        private readonly float LEFT_ROTATION = 270f;

        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<Components.MoveDirection>().Inc<Components.Movable>().Inc<Components.PhysicalObject>().Inc<Components.CameraLead>().End();
            var cameraFilter = world.Filter<Components.MainCamera>().Inc<Components.RotateCamera>().End();
            var rotateCameraPool = world.GetPool<Components.RotateCamera>();
            var movablePool = world.GetPool<Components.Movable>();
            var moveDirectionPool = world.GetPool<Components.MoveDirection>();
            var physicalObjectPool = world.GetPool<Components.PhysicalObject>();
            var cameraLeadPool = world.GetPool<Components.CameraLead>();

            foreach (var entity in filter)
            {
                ref var movableComponent = ref movablePool.Get(entity);
                var moveDirectionComponent = moveDirectionPool.Get(entity);
                var physicalObjectComponent = physicalObjectPool.Get(entity);
                var cameraLeadComponent = cameraLeadPool.Get(entity);
                var mainCameraComponent = rotateCameraPool.Get(cameraLeadComponent.cameraEntity);
                
                float cameraCurrentRotation = mainCameraComponent.currentRotation;
                Vector3 moveDirectionRaw = Vector3.zero;
                if (cameraCurrentRotation == RIGHT_ROTATION) {
                    if (moveDirectionComponent.forward == 1 && moveDirectionComponent.right == 0) {
                        moveDirectionRaw = new Vector3(1, 0, 0); 
                    } else if (moveDirectionComponent.forward == 0 && moveDirectionComponent.right == 1) {
                        moveDirectionRaw = new Vector3(0, 0, -1);
                    } else if (moveDirectionComponent.forward == 0 && moveDirectionComponent.right == -1) {
                        moveDirectionRaw = new Vector3(0, 0, 1);
                    } else if (moveDirectionComponent.forward == 1 && moveDirectionComponent.right == -1) {
                        moveDirectionRaw = new Vector3(1, 0, 1);
                    } else if (moveDirectionComponent.forward == -1 && moveDirectionComponent.right == -1) {
                        moveDirectionRaw = new Vector3(-1, 0, 1);
                    }
                    else if (moveDirectionComponent.forward == 1 && moveDirectionComponent.right == 1) {
                        moveDirectionRaw = new Vector3(1, 0, -1);
                    }
                }

                float moveSpeed = movableComponent.moveSpeed; 

                Vector3 moveDirection = moveDirectionRaw * moveSpeed * Time.fixedDeltaTime;
                // float moveRotation = 

                Vector3 movePosition = physicalObjectComponent.rigidbody.position + moveDirection;
                if (physicalObjectComponent.rigidbody.rotation.y != cameraCurrentRotation) {
                    // physicalObjectComponent.rigidbody.MoveRotation(Quaternion.Euler(0, cameraCurrentRotation, 0));
                }
                physicalObjectComponent.rigidbody.MovePosition(movePosition);
            }
        }
    }
}
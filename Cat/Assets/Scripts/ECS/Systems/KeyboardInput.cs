using Client;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class KeyboardInput : IEcsRunSystem {        
        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            MovableHandler(world);
            RotateCameraHandler(world);
            AttackInputHandler(world);
            JumpPlayerHandler(world);
        }

        private void MovableHandler (EcsWorld world) {
            var filter = world.Filter<Components.Movable>().Inc<Components.MoveDirection>().Inc<Components.PhysicalObject>().Inc<Components.JumpPlayer>().End();
            var moveDirectionPool = world.GetPool<Components.MoveDirection>();
            var jumpPlayerPool = world.GetPool<Components.JumpPlayer>();

            foreach (var entity in filter)
            {
                ref var moveDirectionComponent = ref moveDirectionPool.Get(entity);
                var jumpPlayerComponent = jumpPlayerPool.Get(entity);

                if (jumpPlayerComponent.isGrounded) {
                    moveDirectionComponent.forward = Input.GetAxisRaw("Vertical");
                    moveDirectionComponent.right = Input.GetAxisRaw("Horizontal");
                }
            }
        }

        private void RotateCameraHandler (EcsWorld world) {
            var filter = world.Filter<Components.FollowPlayer>().Inc<Components.RotateCamera>().End();
            var rotateCameraPool = world.GetPool<Components.RotateCamera>();

            foreach (var entity in filter)
            {
                ref var rotateCameraComponent = ref rotateCameraPool.Get(entity);

                float newRotation = rotateCameraComponent.currentRotation;
                if (Input.GetKeyDown(KeyCode.E)) {
                    newRotation = rotateCameraComponent.currentRotation + 90f;

                    if (newRotation > 270) {
                        newRotation = 0;
                    }
                }
                
                if (Input.GetKeyDown(KeyCode.Q)) {
                    newRotation = rotateCameraComponent.currentRotation - 90f;

                    if (newRotation < 0) {
                        newRotation = 270;
                    }
                }

                if (rotateCameraComponent.currentRotation != newRotation) {
                    rotateCameraComponent.currentRotation = newRotation;
                }

            }
        }
        
        private void AttackInputHandler (EcsWorld world) {

            if (Input.GetKeyDown(KeyCode.F)) {
                var filter = world.Filter<Components.PhysicalObject>().Inc<Components.Attacker>().End();
                var attackerPool = world.GetPool<Components.Attacker>();
                
                foreach (var entity in filter) {
                    ref var attackerComponent = ref attackerPool.Get(entity);
                    
                    if (!attackerComponent.isAttacked) {
                        attackerComponent.isAttacked = true;
                    }
                }
            }
        }

        private void JumpPlayerHandler (EcsWorld world) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                var filter = world.Filter<Components.PhysicalObject>().Inc<Components.JumpPlayer>().End();
                var jumpPlayerPool = world.GetPool<Components.JumpPlayer>();
                
                foreach (var entity in filter) {
                    ref var jumpPlayerComponent = ref jumpPlayerPool.Get(entity);
                    
                    if (!jumpPlayerComponent.isJump && jumpPlayerComponent.isGrounded) {
                        jumpPlayerComponent.isJump = true;
                    }
                }
            }
        }
    }
}
using System;
using UnityEngine;
using Components;
using Leopotam.EcsLite;

namespace Assets.Scripts.ECS.Systems
{
    sealed class PlayerAnimationSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<AnimatorComponent>().Inc<MoveDirection>().End();

            var moveDirFilter = world.Filter<MovePlayerDirection>().End();

            var animatorPool = world.GetPool<AnimatorComponent>();
            var moveDirectionPool = world.GetPool<MoveDirection>();
            var movePlayerDirectionPool = world.GetPool<MovePlayerDirection>();

            foreach (var entity in filter)
            {
                ref var animatorComponent = ref animatorPool.Get(entity);
                ref var moveDirectionComponent = ref moveDirectionPool.Get(entity);

                var isMoving = Math.Abs(moveDirectionComponent.forward) > 0.2 || Math.Abs(moveDirectionComponent.right) > 0.2;

                animatorComponent.Animator.SetBool("IsRunning", isMoving);
            }

            foreach (var entity in moveDirFilter) {
                ref var moveDirComponent = ref movePlayerDirectionPool.Get(entity);

                if (moveDirComponent.currentAngle != 0 && !float.IsNaN(moveDirComponent.currentAngle)) {
                    moveDirComponent.transform.Rotate(new Vector3(0, moveDirComponent.currentAngle, 0));
                    moveDirComponent.currentAngle = 0;
                }

            }
        }
    }
}
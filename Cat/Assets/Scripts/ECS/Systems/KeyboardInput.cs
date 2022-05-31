using Client;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class KeyboardInput : IEcsRunSystem {        
        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var filter = world.Filter<Components.Movable>().Inc<Components.MoveDirection>().End();
            var moveDirectionPool = world.GetPool<Components.MoveDirection>();

            foreach (var entity in filter)
            {
                ref var moveDirectionComponent = ref moveDirectionPool.Get(entity);

                moveDirectionComponent.forward = Input.GetAxisRaw("Vertical");
                moveDirectionComponent.right = Input.GetAxisRaw("Horizontal");
            }
        }
    }
}
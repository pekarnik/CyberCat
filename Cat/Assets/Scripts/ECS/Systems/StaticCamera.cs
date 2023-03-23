using Components;
using Leopotam.EcsLite;
using UnityEngine;
using Utils;
namespace Systems {
    sealed class StaticCamera : IEcsRunSystem {

        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<Components.StaticCamera>().End();
            var staticCameraPool = world.GetPool<Components.StaticCamera>();

            foreach (var entity in filter) {
                ref var staticCameraComponent = ref staticCameraPool.Get(entity);

                if (!staticCameraComponent.alreadyFixed) {
                    staticCameraComponent.cameraTransform.position = staticCameraComponent.cameraStaticPosition;
                    staticCameraComponent.cameraTransform.rotation = staticCameraComponent.cameraStaticRotation;
                    staticCameraComponent.alreadyFixed = true;
                }
            }
        }
    }
}
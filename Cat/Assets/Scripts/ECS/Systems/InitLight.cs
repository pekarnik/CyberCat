using Client;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    sealed class InitLight : IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var lightEntity = world.NewEntity();

            var lightRotatorPool = world.GetPool<Components.LightRotator>();

            lightRotatorPool.Add(lightEntity);

            ref var lightRotatorEntity = ref lightRotatorPool.Get(lightEntity);

            Transform directionalLightTransform = GameObject.FindGameObjectWithTag("GlobalLight").transform;
            lightRotatorEntity.lightTransform = directionalLightTransform;
            lightRotatorEntity.timeToEnd = 5f * 60;
            lightRotatorEntity.startAngle = 0f;
            lightRotatorEntity.endAngle = 180f;
        }

    }
}

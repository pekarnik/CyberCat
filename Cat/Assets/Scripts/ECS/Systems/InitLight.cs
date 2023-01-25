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

        }
    }
}

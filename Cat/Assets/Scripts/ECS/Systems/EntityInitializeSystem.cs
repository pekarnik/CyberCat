using Assets.Scripts.ECS.Components;
using Leopotam.EcsLite;

namespace Systems
{
    public sealed class EntityInitializeSystem : IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var filter = world.Filter<InitializeEntityRequestComponent>().End();
            var initializeEntityRequestPool = world.GetPool<InitializeEntityRequestComponent>();

            foreach (var entity in filter)
            {
                ref var initializeEntityRequestComponent = ref initializeEntityRequestPool.Get(entity);

                initializeEntityRequestComponent.EntityReference.Entity = entity;
            }
        }
    }
}
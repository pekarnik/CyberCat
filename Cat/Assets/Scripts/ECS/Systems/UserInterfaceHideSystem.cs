using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems
{
    public sealed class UserInterfaceHideSystem : IEcsRunSystem
    {
        private Canvas _userInterface;

        public void Run(EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var filter = world.Filter<LightRotator>().End();
            var lightRotatorPool = world.GetPool<LightRotator>();

            foreach (var entity in filter)
            {
                ref LightRotator lightRotator = ref lightRotatorPool.Get(entity);

                if (lightRotator.lightTransform.rotation.x == 0)
                {
                    _userInterface.gameObject.SetActive(!_userInterface.gameObject.activeSelf);
                }
            }
        }
    }
}
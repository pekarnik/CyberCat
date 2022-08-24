using Components;
using Leopotam.EcsLite;
using UnityEngine;
using Utils;
namespace Systems {
    sealed class TriggerArea : IEcsRunSystem {
        int layerMask = 1 << Config.Layers.PLAYER;

        private Physics physics;

        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<Components.PhysicalObject>().Inc<Components.TriggerArea>().End();
            var triggerAreaPool = world.GetPool<Components.TriggerArea>();
            var physicalObjectPool = world.GetPool<Components.PhysicalObject>();

            foreach (var entity in filter) {
                var triggerAreaComponent = triggerAreaPool.Get(entity);
                var physicalObjectComponent = physicalObjectPool.Get(entity);


                var center = physicalObjectComponent.rigidbody.transform.position;
                var direction = Vector3.forward;

                var radius = triggerAreaComponent.maxRadius;
                var distance = triggerAreaComponent.maxDistance;
                var angle = triggerAreaComponent.angle;

                RaycastHit[] coneHits = physics.ConeCastAll(center, radius, direction, distance, angle);

                if (coneHits.Length > 0) {
                    for (int i = 0; i < coneHits.Length; i++)
                    {
                        coneHits[i].collider.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 1f);
                    }
                }
            }
        }
    }
}
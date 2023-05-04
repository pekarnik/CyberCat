using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class Attack : IEcsRunSystem {
        int layerMask = 1 << Config.Layers.PLAYER;
        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<Components.PhysicalObject>().Inc<Components.Attacker>().End();
            var attackerPool = world.GetPool<Components.Attacker>();

            foreach (var entity in filter) {
                ref var attackerComponent = ref attackerPool.Get(entity);

                if (attackerComponent.isAttacked) {
                    Vector3 attackSource = attackerComponent.attackSource.position;

                    RaycastHit hit;
                    Physics.Raycast(attackSource, Vector3.forward, out hit, 5f, ~layerMask);

                    if (hit.transform?.gameObject?.layer == Config.Layers.ENEMY) {
                        NpcHealthController health = hit.transform?.gameObject.GetComponent<NpcHealthController>();

                        if (health != null) {
                            health.DecreaseHealth();
                        }
                    }

                    attackerComponent.isAttacked = false;
                }
            }
        }
    }
}
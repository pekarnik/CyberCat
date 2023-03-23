using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class JumpPlayer : IEcsRunSystem {        

        int layerMask = ~(1 << WorldConfig.Layers.PLAYER);
        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<Components.JumpPlayer>().Inc<Components.PhysicalObject>().End();
            var physicalObjectPool = world.GetPool<Components.PhysicalObject>();
            var jumpPlayerPool = world.GetPool<Components.JumpPlayer>();
    
            foreach (var entity in filter)
            {
                ref var jumpPlayerComponent = ref jumpPlayerPool.Get(entity);
                var physicalObjectComponent = physicalObjectPool.Get(entity);
                
                if (jumpPlayerComponent.isJump && jumpPlayerComponent.isGrounded) {
                    physicalObjectComponent.rigidbody.AddForce(Vector3.up * jumpPlayerComponent.jumpHeight, ForceMode.Impulse);
                    jumpPlayerComponent.isJump = false;
                }

                Transform transform = physicalObjectComponent.rigidbody.transform;

                float height = physicalObjectComponent.meshFilter.mesh.bounds.extents.y;

                RaycastHit hit;

                Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, height + 0.01f, layerMask);

                jumpPlayerComponent.isGrounded = hit.collider != null;
            }
        }
    }
}
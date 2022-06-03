using Client;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class MoveCamera : IEcsInitSystem, IEcsRunSystem
    {
        private int cameraEntity;
        
        public void Init(EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            cameraEntity = world.NewEntity();
            
            var followPlayerPool = world.GetPool<Components.FollowPlayer>();
            var cameraLeaderPool = world.GetPool<Components.CameraLead>();
            
            followPlayerPool.Add(cameraEntity);
            
            var filter = world.Filter<Components.CameraLead>().End();
            var cameraLeaderEntity = filter.GetRawEntities()[0];
            var cameraLeaderComponent = cameraLeaderPool.Get(cameraLeaderEntity);

            ref var followPlayerComponent = ref followPlayerPool.Get(cameraEntity);
            
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            camera.transform.rotation = Quaternion.Euler(60, 90, 0);
            followPlayerComponent.followerTransform = camera.transform;
            GameObject leader = GameObject.FindGameObjectWithTag(cameraLeaderComponent.leaderTag);
            followPlayerComponent.leaderTransform = leader.transform;
            followPlayerComponent.offset = new Vector3(-1f, 5f, 0f);
            followPlayerComponent.velocity = Vector3.zero;
            followPlayerComponent.smoothness = 0.2f;
        }
        
        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var filter = world.Filter<Components.FollowPlayer>().End();

            var followPlayerPool = world.GetPool<Components.FollowPlayer>();
            
            ref var followPlayerComponent = ref followPlayerPool.Get(cameraEntity);

            var leaderPosition = followPlayerComponent.leaderTransform.position + followPlayerComponent.offset;
            var followerPosition = followPlayerComponent.followerTransform.position;

            followPlayerComponent.followerTransform.position = Vector3.SmoothDamp(followerPosition, leaderPosition, ref followPlayerComponent.velocity, followPlayerComponent.smoothness);

        }
    }
}
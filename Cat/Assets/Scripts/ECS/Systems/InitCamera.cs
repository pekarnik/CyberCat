using Client;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class InitCamera : IEcsInitSystem {
        public void Init (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var cameraEntity = world.NewEntity();
            
            var followPlayerPool = world.GetPool<Components.FollowPlayer>();
            var cameraLeaderPool = world.GetPool<Components.CameraLead>();
            var rotateCameraPool = world.GetPool<Components.RotateCamera>();
            
            followPlayerPool.Add(cameraEntity);
            rotateCameraPool.Add(cameraEntity);
            
            var filter = world.Filter<Components.CameraLead>().End();
            var cameraLeaderEntity = filter.GetRawEntities()[0];
            var cameraLeaderComponent = cameraLeaderPool.Get(cameraLeaderEntity);

            ref var followPlayerComponent = ref followPlayerPool.Get(cameraEntity);
            
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            camera.transform.rotation = Quaternion.Euler(60, 90, 0);
            followPlayerComponent.followerTransform = camera.transform;
            GameObject leader = GameObject.FindGameObjectWithTag(cameraLeaderComponent.leaderTag);
            followPlayerComponent.leaderTransform = leader.transform;
        }
    }
}
using Client;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class MoveCamera : IEcsInitSystem, IEcsRunSystem
    {
        private int cameraEntity;

        private readonly float FORWARD_ROTATION = 0f;
        private readonly float RIGHT_ROTATION = 90f;
        private readonly float BACKWARD_ROTATION = 180f;
        private readonly float LEFT_ROTATION = 270f;

        private readonly Vector3 FORWARD_OFFSET = new Vector3(0, 5, -1);
        private readonly Vector3 RIGHT_OFFSET = new Vector3(-1, 5, 0);
        private readonly Vector3 BACKWARD_OFFSET = new Vector3(0, 5, 1);
        private readonly Vector3 LEFT_OFFSET = new Vector3(1, 5, 0);
        
        public void Init(EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            cameraEntity = world.NewEntity();
            
            var followPlayerPool = world.GetPool<Components.FollowPlayer>();
            var cameraLeaderPool = world.GetPool<Components.CameraLead>();
            var rotateCameraPool = world.GetPool<Components.RotateCamera>();
            var mainCameraPool = world.GetPool<Components.MainCamera>();
            
            followPlayerPool.Add(cameraEntity);
            mainCameraPool.Add(cameraEntity);

            rotateCameraPool.Add(cameraEntity);
            ref var rotateCameraComponent = ref rotateCameraPool.Get(cameraEntity);
            rotateCameraComponent.currentRotation = FORWARD_ROTATION;

            var filter = world.Filter<Components.CameraLead>().End();
            var cameraLeaderEntity = filter.GetRawEntities()[0];
            ref var cameraLeaderComponent = ref cameraLeaderPool.Get(cameraLeaderEntity);

            
            ref var followPlayerComponent = ref followPlayerPool.Get(cameraEntity);
            
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            camera.transform.rotation = Quaternion.Euler(60, 0, 0);
            cameraLeaderComponent.cameraEntity = cameraEntity;
            followPlayerComponent.followerTransform = camera.transform;
            GameObject leader = GameObject.FindGameObjectWithTag(cameraLeaderComponent.leaderTag);
            followPlayerComponent.leaderTransform = leader.transform;
            followPlayerComponent.offset = new Vector3(0f, 5f, -1f);
            followPlayerComponent.velocity = Vector3.zero;
            followPlayerComponent.smoothness = 0.2f;
        }
        
        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var filter = world.Filter<Components.FollowPlayer>().Inc<Components.RotateCamera>().End();

            var followPlayerPool = world.GetPool<Components.FollowPlayer>();
            var rotateCameraPool = world.GetPool<Components.RotateCamera>();
            
            ref var followPlayerComponent = ref followPlayerPool.Get(cameraEntity);
            var rotateCameraComponent = rotateCameraPool.Get(cameraEntity);

            if (rotateCameraComponent.currentRotation == this.FORWARD_ROTATION) {
                followPlayerComponent.offset = this.FORWARD_OFFSET;
            }
            if (rotateCameraComponent.currentRotation == this.RIGHT_ROTATION) {
                followPlayerComponent.offset = this.RIGHT_OFFSET;
            }
            if (rotateCameraComponent.currentRotation == this.LEFT_ROTATION) {
                followPlayerComponent.offset = this.LEFT_OFFSET;
            }
            if (rotateCameraComponent.currentRotation == this.BACKWARD_ROTATION) {
                followPlayerComponent.offset = this.BACKWARD_OFFSET;
            }

            var leaderPosition = followPlayerComponent.leaderTransform.position + followPlayerComponent.offset;
            var followerPosition = followPlayerComponent.followerTransform.position;

            followPlayerComponent.followerTransform.position = Vector3.SmoothDamp(followerPosition, leaderPosition, ref followPlayerComponent.velocity, followPlayerComponent.smoothness);

            if (rotateCameraComponent.currentRotation != followPlayerComponent.followerTransform.rotation.y) {
                followPlayerComponent.followerTransform.rotation = Quaternion.Euler(60, rotateCameraComponent.currentRotation, 0);    
            };
        }
    }
}
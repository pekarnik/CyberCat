using Client;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class MoveCamera : IEcsInitSystem, IEcsRunSystem
    {
        private int cameraEntity;

        public  const float CAMERA_HEIGHT = 7f;
        public const float CAMERA_FORWARD_ANGLE = 45f;

        private readonly float FORWARD_ROTATION = 0f;
        private readonly float RIGHT_ROTATION = 90f;
        private readonly float LEFT_ROTATION = 270f;
        private readonly float BACKWARD_ROTATION = 180f;

        private readonly Vector3 FORWARD_OFFSET = new Vector3(0, CAMERA_HEIGHT, -6);
        private readonly Vector3 RIGHT_OFFSET = new Vector3(-6, CAMERA_HEIGHT, 0);
        private readonly Vector3 LEFT_OFFSET = new Vector3(6, CAMERA_HEIGHT, 0);
        private readonly Vector3 BACKWARD_OFFSET = new Vector3(0, CAMERA_HEIGHT, 6);
        
        public void Init(EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            cameraEntity = world.NewEntity();
            
            var followPlayerPool = world.GetPool<Components.FollowPlayer>();
            var cameraLeaderPool = world.GetPool<Components.CameraLead>();
            var cameraStatePool = world.GetPool<Components.CameraState>();
            var rotateCameraPool = world.GetPool<Components.RotateCamera>();
            
            followPlayerPool.Add(cameraEntity);
            cameraStatePool.Add(cameraEntity);

            rotateCameraPool.Add(cameraEntity);
            ref var rotateCameraComponent = ref rotateCameraPool.Get(cameraEntity);
            rotateCameraComponent.currentRotation = FORWARD_ROTATION;

            var filter = world.Filter<Components.CameraLead>().End();
            var cameraLeaderEntity = filter.GetRawEntities()[0];
            var cameraLeaderComponent = cameraLeaderPool.Get(cameraLeaderEntity);

            
            ref var followPlayerComponent = ref followPlayerPool.Get(cameraEntity);
            ref var cameraStateComponent = ref cameraStatePool.Get(cameraEntity);
            
            cameraStateComponent.state = Components.CameraState.CAMERA_STATE.FOLLOW_PLAYER;
            GameObject camera = GameObject.FindGameObjectWithTag("Camera");
            camera.transform.rotation = Quaternion.Euler(CAMERA_FORWARD_ANGLE, 0, 0);
            followPlayerComponent.followerTransform = camera.transform;
            GameObject leader = GameObject.FindGameObjectWithTag(cameraLeaderComponent.leaderTag);
            followPlayerComponent.leaderTransform = leader.transform;
            followPlayerComponent.offset = new Vector3(0f, CAMERA_HEIGHT, 6f);
            followPlayerComponent.velocity = Vector3.zero;
            followPlayerComponent.smoothness = 0.2f;
        }
        
        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            var filter = world.Filter<Components.FollowPlayer>().Inc<Components.RotateCamera>().End();

            var followPlayerPool = world.GetPool<Components.FollowPlayer>();
            var rotateCameraPool = world.GetPool<Components.RotateCamera>();
            
            foreach (var cameraEntity in filter) {
                ref var followPlayerComponent = ref followPlayerPool.Get(cameraEntity);
                ref var rotateCameraComponent = ref rotateCameraPool.Get(cameraEntity);

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
                    followPlayerComponent.followerTransform.rotation = Quaternion.Euler(CAMERA_FORWARD_ANGLE, rotateCameraComponent.currentRotation, 0);    
                };
            }
        }
    }
}
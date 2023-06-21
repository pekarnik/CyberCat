using Client;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems {
    sealed class KeyboardInput : IEcsRunSystem {        
        public void Run (EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            MovableHandler(world);
            RotateCameraHandler(world);
            AttackInputHandler(world);
            JumpPlayerHandler(world);
            CameraPositionChangeHandler(world);
            MovePlayerDirection(world);
        }

        private void MovableHandler (EcsWorld world) {
            var filter = world.Filter<Components.Movable>().Inc<Components.MoveDirection>().Inc<Components.PhysicalObject>().Inc<Components.JumpPlayer>().End();
            var moveDirectionPool = world.GetPool<Components.MoveDirection>();
            var movablePool = world.GetPool<Components.Movable>();
            var jumpPlayerPool = world.GetPool<Components.JumpPlayer>();

            foreach (var entity in filter)
            {
                ref var moveDirectionComponent = ref moveDirectionPool.Get(entity);
                ref var movableComponent = ref movablePool.Get(entity);
                var jumpPlayerComponent = jumpPlayerPool.Get(entity);

                moveDirectionComponent.forward = Input.GetAxisRaw("Vertical");
                moveDirectionComponent.right = Input.GetAxisRaw("Horizontal");

                movableComponent.moveSpeed = Components.Movable.DEFAULT_MOVE_SPEED;
            }
        }

        private void MovePlayerDirection (EcsWorld world) {
            var filter = world.Filter<Components.MovePlayerDirection>().End();
            var movePlayerDirectionPool = world.GetPool<Components.MovePlayerDirection>();

            foreach (var entity in filter) {
                ref var movePlayerDirectionComponent = ref movePlayerDirectionPool.Get(entity);
                Vector2 current = new Vector2(Input.GetAxisRaw("Vertical"), -Input.GetAxisRaw("Horizontal"));

                if (movePlayerDirectionComponent.current != current && current != new Vector2(0, 0)) {
                    movePlayerDirectionComponent.old = movePlayerDirectionComponent.current;
                    movePlayerDirectionComponent.current = current;

                    float cosin = (movePlayerDirectionComponent.old.x * current.x) + (movePlayerDirectionComponent.old.y * current.y);
                    float modAb = Module(movePlayerDirectionComponent.old) * Module(current);
                    float vectCos = cosin/modAb;

                    float sin = (movePlayerDirectionComponent.old.y * current.x) - (movePlayerDirectionComponent.old.x * current.y);
                    // Debug.Log("SIN " + sin);
                    float vectSin = sin/modAb;

                    float acos = Mathf.Acos(vectCos);

                    float angle = 0;

                    if (vectSin < 0) {
                        angle = (Mathf.Rad2Deg * -acos) % 360;
                    } else {
                        angle = (Mathf.Rad2Deg * acos);
                    }

                    movePlayerDirectionComponent.currentAngle = angle;
                }
            }

        }

        private float Module(Vector2 vector) {
            return Mathf.Pow(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2), 0.5f);
        }

        private void RotateCameraHandler (EcsWorld world) {
            var filter = world.Filter<Components.FollowPlayer>().Inc<Components.RotateCamera>().End();
            var rotateCameraPool = world.GetPool<Components.RotateCamera>();

            foreach (var entity in filter)
            {
                ref var rotateCameraComponent = ref rotateCameraPool.Get(entity);

                float newRotation = rotateCameraComponent.currentRotation;
                if (Input.GetKeyDown(KeyCode.E)) {
                    newRotation = rotateCameraComponent.currentRotation + 90f;

                    if (newRotation > 270) {
                        newRotation = 0;
                    }
                }
                
                if (Input.GetKeyDown(KeyCode.Q)) {
                    newRotation = rotateCameraComponent.currentRotation - 90f;

                    if (newRotation < 0) {
                        newRotation = 270;
                    }
                }

                if (rotateCameraComponent.currentRotation != newRotation) {
                    rotateCameraComponent.currentRotation = newRotation;
                }

            }
        }

        private void CameraPositionChangeHandler (EcsWorld world) {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                var followFilter = world.Filter<Components.FollowPlayer>().Inc<Components.RotateCamera>().End();
                var staticFilter = world.Filter<Components.StaticCamera>().End();
                var cameraStateFilter = world.Filter<Components.CameraState>().End();
                var cameraStatePool = world.GetPool<Components.CameraState>();
                var followPlayerPool = world.GetPool<Components.FollowPlayer>();
                var rotateCameraPool = world.GetPool<Components.RotateCamera>();
                var staticCameraPool = world.GetPool<Components.StaticCamera>();

                foreach (var entity in staticFilter) {

                    var cameraStateComponent = cameraStatePool.Get(entity);

                    if (cameraStateComponent.state == Components.CameraState.CAMERA_STATE.FOLLOW_PLAYER) {
                        break;
                    }

                    followPlayerPool.Add(entity);
                    rotateCameraPool.Add(entity);

                    var cameraLeaderPool = world.GetPool<Components.CameraLead>();
                    var filter = world.Filter<Components.CameraLead>().End();
                    var cameraLeaderEntity = filter.GetRawEntities()[0];
                    var cameraLeaderComponent = cameraLeaderPool.Get(cameraLeaderEntity);

                    ref var followPlayerComponent = ref followPlayerPool.Get(entity);
                    ref var rotateCameraComponent = ref rotateCameraPool.Get(entity);
                    var staticCameraComponent = staticCameraPool.Get(entity);

                    GameObject camera = GameObject.FindGameObjectWithTag("Camera");
                    camera.transform.rotation = Quaternion.Euler(Systems.MoveCamera.CAMERA_FORWARD_ANGLE, 0, 0);
                    followPlayerComponent.followerTransform = staticCameraComponent.cameraTransform;
                    GameObject leader = GameObject.FindGameObjectWithTag(cameraLeaderComponent.leaderTag);
                    followPlayerComponent.leaderTransform = leader.transform;
                    followPlayerComponent.offset = new Vector3(0f, Systems.MoveCamera.CAMERA_HEIGHT, 6f);
                    followPlayerComponent.velocity = Vector3.zero;
                    followPlayerComponent.smoothness = 0.2f;

                    staticCameraPool.Del(entity);   
                }

                foreach (var entity in followFilter) {

                    var cameraStateComponent = cameraStatePool.Get(entity);

                    if (cameraStateComponent.state == Components.CameraState.CAMERA_STATE.STATIC) {
                        break;
                    }
                    
                    rotateCameraPool.Del(entity);
                    staticCameraPool.Add(entity);

                    ref var staticCameraComponent = ref staticCameraPool.Get(entity);
                    var followPlayerComponent = followPlayerPool.Get(entity);

                    staticCameraComponent.cameraStaticPosition = new Vector3(0, 20, 0);
                    staticCameraComponent.cameraStaticRotation = Quaternion.Euler(90, 0, 0);
                    staticCameraComponent.alreadyFixed = false;
                    staticCameraComponent.cameraTransform = followPlayerComponent.followerTransform;

                    followPlayerPool.Del(entity);
                }

                foreach (var entity in cameraStateFilter) {
                    ref var cameraStateComponent = ref cameraStatePool.Get(entity);

                    if (cameraStateComponent.state == Components.CameraState.CAMERA_STATE.STATIC) {
                        cameraStateComponent.state = Components.CameraState.CAMERA_STATE.FOLLOW_PLAYER;
                    } else {
                        cameraStateComponent.state = Components.CameraState.CAMERA_STATE.STATIC;
                    }
                }

            }
        }
        
        private void AttackInputHandler (EcsWorld world) {

            if (Input.GetKeyDown(KeyCode.F)) {

                var filter = world.Filter<Components.PhysicalObject>().Inc<Components.Attacker>().End();
                var attackerPool = world.GetPool<Components.Attacker>();
                
                foreach (var entity in filter) {
                    ref var attackerComponent = ref attackerPool.Get(entity);
                    
                    if (!attackerComponent.isAttacked) {
                        attackerComponent.isAttacked = true;
                    }
                }
            }
        }

        private void JumpPlayerHandler (EcsWorld world) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                var filter = world.Filter<Components.PhysicalObject>().Inc<Components.JumpPlayer>().End();
                var jumpPlayerPool = world.GetPool<Components.JumpPlayer>();
                
                foreach (var entity in filter) {
                    ref var jumpPlayerComponent = ref jumpPlayerPool.Get(entity);
                    
                    if (!jumpPlayerComponent.isJump && jumpPlayerComponent.isGrounded) {
                        jumpPlayerComponent.isJump = true;
                    }
                }
            }
        }
    }
}
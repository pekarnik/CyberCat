using Cinemachine;
using UnityEngine;

using Assets.Scripts.Extensions;

namespace Assets.Scripts.CameraController
{
    [RequireComponent(
        typeof(BoxCollider),
        typeof(WallVisionComponent))]
    public class CameraTransition : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _roomCamera;
        [SerializeField] private CinemachineBrain _mainCamera;

        private void Awake()
        {
            _roomCamera.IfNullLogError();
            _mainCamera.IfNullLogError();

            _playerFollowingCamera = transform.root.TryGetComponentInChildren<PlayerFollowingCamera>();

            if (!TryGetComponent(out _wallVisionComponent))
            {
                Debug.LogError("CameraTransition: component WallVisionComponent not set on game object");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerMovementController>(out _))
            {
                _mainCamera.ActiveVirtualCamera.Priority = 10;
                _playerFollowingCamera.Camera.Priority = 10;
                _roomCamera.Priority = 50;

                _wallVisionComponent.HideWalls(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_mainCamera.ActiveVirtualCamera.Equals(_roomCamera)) return;

            if (other.TryGetComponent<PlayerMovementController>(out _))
            {
                _roomCamera.Priority = 10;
                _playerFollowingCamera.Camera.Priority = 50;

                _wallVisionComponent.HideWalls(false);
            }
        }

        private PlayerFollowingCamera _playerFollowingCamera;
        private WallVisionComponent _wallVisionComponent;
    }
}
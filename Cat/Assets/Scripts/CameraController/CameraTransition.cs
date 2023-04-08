using Cinemachine;
using UnityEngine;

using Assets.Scripts.Extensions;

namespace Assets.Scripts.CameraController
{
    [RequireComponent(typeof(BoxCollider))]
    public class CameraTransition : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _roomCamera;

        private void Awake()
        {
            _roomCamera.IfNullLogError();

            _playerFollowingCamera = transform.root.TryGetComponentInChildren<PlayerFollowingCamera>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerMovementController>(out _))
            {
                _roomCamera.Priority = 50;
                _playerFollowingCamera.Camera.Priority = 10;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerMovementController>(out _))
            {
                _roomCamera.Priority = 10;
                _playerFollowingCamera.Camera.Priority = 50;
            }
        }

        private PlayerFollowingCamera _playerFollowingCamera;
    }
}
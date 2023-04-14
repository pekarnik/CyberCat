using Assets.Scripts.Extensions;

using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.CameraController
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
            if (!other.TryGetComponent<PlayerMovementController>(out _)) return;

            bool isCurrentCameraEqualsPlayerFollowingCamera =
                CameraTransitionsData.CountOfRoomCollidersPlayerCrossed == 0;

            // Переход с активной камеры, которая следит за игроком, на камеру комнаты
            if (isCurrentCameraEqualsPlayerFollowingCamera)
            {
                SetActiveCamera(_roomCamera);
                HideWalls(true);
            }
            // Если активная камера является камерой комнаты, то запоминаю, на какую вирутальную
            // камеру нужно будет переключиться при выходе из коллайдера комнаты
            else
            {
                CameraTransitionsData.NextTransitionCamera = _roomCamera;
            }

            CameraTransitionsData.CountOfRoomCollidersPlayerCrossed++;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<PlayerMovementController>(out _)) return;

            // Ситуация, когда игрок находится в одном коллайдере комнаты.
            // Переход на камеру, которая следит за игроком.
            if (CameraTransitionsData.CountOfRoomCollidersPlayerCrossed == 1)
            {
                SetActiveCamera(_playerFollowingCamera.Camera);
                HideWalls(false);
            }
            // Обработка ситуации, когда геймдизайнер задал коллайдеры комнаты таким образом, что они
            // пересекаются, и игрок находится в пересечении этих коллайдеров
            else if (CameraTransitionsData.CountOfRoomCollidersPlayerCrossed >= 2)
            {
                // Если игрок вышел из предыдущего коллайдера комнаты,
                // то активируется камера следующего коллайдера комнаты
                if (!_roomCamera.Equals(CameraTransitionsData.NextTransitionCamera))
                {
                    SetActiveCamera(CameraTransitionsData.NextTransitionCamera);
                    HideWalls(true);
                }
            }

            CameraTransitionsData.CountOfRoomCollidersPlayerCrossed--;
        }

        private void SetActiveCamera(CinemachineVirtualCamera camera)
        {
            _mainCamera.ActiveVirtualCamera.Priority = 10;
            camera.Priority = 50;
        }

        private void HideWalls(bool value)
        {
            _wallVisionComponent.HideWalls(value);
        }

        private PlayerFollowingCamera _playerFollowingCamera;
        private WallVisionComponent _wallVisionComponent;
    }
}
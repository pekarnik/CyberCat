using Cinemachine;
using UnityEngine;

using Assets.Scripts.Extensions;

namespace Assets.Scripts.CameraController
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class PlayerFollowingCamera : MonoBehaviour
    {
        public CinemachineVirtualCamera Camera { get; private set; }

        private void Awake()
        {
            Camera = gameObject.TryGetComponent<CinemachineVirtualCamera>();
        }
    }
}
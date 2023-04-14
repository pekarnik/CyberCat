using Assets.Scripts.Extensions;

using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.CameraController
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
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.CameraController
{
    public class WallVisionComponent : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _hidableWalls;

        public void HideWalls(bool value)
        {
            foreach(var wall in _hidableWalls)
            {
                wall.enabled = !value;
            }
        }

        private void Awake()
        {
            if (_hidableWalls == null)
            {
                Debug.LogError("WallVisionComponent: input walls array not set");
            }
        }
    }
}
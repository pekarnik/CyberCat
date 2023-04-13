using UnityEngine;

namespace Assets.Scripts.CameraController
{
    public class WallVisionComponent : MonoBehaviour
    {
        [SerializeField] private GameObject[] _hidableWalls;

        public void HideWalls(bool value)
        {
            foreach(var wall in _hidableWalls)
            {
                wall.SetActive(!value);
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
using Assets.Scripts.Extensions;

using TMPro;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.Entities.Fridge
{
    public class FridgeView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentProductCountText;
        [SerializeField] private TextMeshProUGUI _maxProductCountText;
        [SerializeField] private Transform _mainCamera;

        public void UpdateCurrentProductCount(uint count)
        {
            _currentProductCountText.text = count.ToString();
        }

        public void UpdateMaxProductCount(uint count)
        {
            _maxProductCountText.text = count.ToString();
        }

        private void Awake()
        {
            _currentProductCountText.IfNullLogError();
            _maxProductCountText.IfNullLogError();
            _mainCamera.IfNullLogError();
        }

        private void Update()
        {
            transform.LookAt(_mainCamera.transform.position);
        }
    }
}
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.Entities.Fridge
{
    public class Fridge : MonoBehaviour
    {
        [SerializeField] private uint _maxProductCount;
        [SerializeField] private uint _currentProductCount;

        public void GetProduct()
        {
            if (_currentProductCount == 0) return;

            _currentProductCount--;
            _view?.UpdateCurrentProductCount(_currentProductCount);
        }

        private void Awake()
        {
            _view = GetComponentInChildren<FridgeView>();

            _view?.UpdateCurrentProductCount(_currentProductCount);
            _view?.UpdateMaxProductCount(_maxProductCount);
        }

        private FridgeView _view;
    }
}
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.Player
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ChargeableItem : MonoBehaviour
    {
        [SerializeField] private Material _normalMaterial;
        [SerializeField] private Material _chargedMaterial;

        private void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        public void Charge()
        {
            if (_isCharged) return;

            _isCharged = true;
            _renderer.material = _chargedMaterial;
        }

        public void Discharge()
        {
            if (!_isCharged) return;

            _isCharged = false;
            _renderer.material = _normalMaterial;
        }

        public bool IsCharged() => _isCharged;

        private MeshRenderer _renderer;

        private bool _isCharged;
    }
}
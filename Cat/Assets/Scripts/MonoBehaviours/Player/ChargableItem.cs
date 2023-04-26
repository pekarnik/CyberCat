using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.Player
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ChargableItem : MonoBehaviour
    {
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

        public bool CanBeCharged()
        {
            return !_isCharged;
        }

        private MeshRenderer _renderer;

        private bool _isCharged;
    }
}
using Assets.Scripts.MonoBehaviours.ChargeableItems.Interfaces;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.ChargeableItems.Base
{
    public abstract class ChangeMaterialAbilityBase : MonoBehaviour, IAbility
    {
        [SerializeField] protected Material _material;

        public virtual void Execute()
        {
            if (_material != null)
            {
                _renderer.material = _material;
            }
        }

        public virtual void Stop()
        {
        }

        private void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        private MeshRenderer _renderer;
    }
}
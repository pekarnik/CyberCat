using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.ChargeableItems
{
    public class LampBehaviour : ChargeableItemBehaviour
    {
        [SerializeField] private Light _light;

        private void Start()
        {
            _light.gameObject.SetActive(false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _itemStateComponent.StateChanged += OnStateComponentChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _itemStateComponent.StateChanged -= OnStateComponentChanged;
        }

        private void OnStateComponentChanged(ChargeableItemStateComponent.States state)
        {
            switch (state)
            {
                case ChargeableItemStateComponent.States.Charged:
                    _light?.gameObject.SetActive(true);
                    break;
                case ChargeableItemStateComponent.States.Normal:
                    _light?.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
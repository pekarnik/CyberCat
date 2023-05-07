using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.ChargeableItems
{
    [RequireComponent(
        typeof(ChargeableItemStateComponent),
        typeof(ChargeableItemAttackComponent)
        )]
    public class ChargeableItemBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            _itemStateComponent = GetComponent<ChargeableItemStateComponent>();
            _itemAttackComponent = GetComponent<ChargeableItemAttackComponent>();
        }

        protected virtual void OnEnable()
        {
            _itemStateComponent.StateChanged += OnStateComponentChangedInternal;
        }

        protected virtual void OnDisable()
        {
            _itemStateComponent.StateChanged -= OnStateComponentChangedInternal;
        }

        private void OnStateComponentChangedInternal(ChargeableItemStateComponent.States state)
        {
            switch (state)
            {
                case ChargeableItemStateComponent.States.Charged:
                    _itemAttackComponent.StartAttack();
                    break;
                case ChargeableItemStateComponent.States.Normal:
                    _itemAttackComponent.StopAttack();
                    break;
            }
        }

        protected ChargeableItemStateComponent _itemStateComponent;
        protected ChargeableItemAttackComponent _itemAttackComponent;
    }
}
using System.Collections.Generic;
using Assets.Scripts.MonoBehaviours.ChargeableItems.Interfaces;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.ChargeableItems
{
    public class Subject : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] _chargedMonoAbility;
        [SerializeField] private MonoBehaviour[] _normalMonoAbility;
        [SerializeField] private MonoBehaviour[] _capturedMonoAbility;

        public SubjectStateComponent.States CurrentState => _stateComponent.CurrentState;

        public void ChangeState(SubjectStateComponent.States state)
        {
            _stateComponent.ChangeState(state);
        }

        private void Awake()
        {
            InitializeAbilities();

            _stateComponent = new SubjectStateComponent(SubjectStateComponent.States.Normal);
        }

        private void OnEnable()
        {
            _stateComponent.StateChanged += OnStateComponentChangedInternal;
            _stateComponent.StateChanged += DebugLog;
        }

        private void OnDisable()
        {
            _stateComponent.StateChanged -= OnStateComponentChangedInternal;
            _stateComponent.StateChanged -= DebugLog;
        }

        private void OnStateComponentChangedInternal(SubjectStateComponent.States state)
        {
            switch (state)
            {
                case SubjectStateComponent.States.Charged:
                    StopAllNormalAbilities();
                    ExecuteAllChargedAbilities();
                    break;

                case SubjectStateComponent.States.Normal:
                    StopAllChargedAbilities();
                    StopAllCapturedAbilities();
                    ExecuteAllNormalAbilities();
                    break;

                case SubjectStateComponent.States.Captured:
                    StopAllNormalAbilities();
                    ExecuteAllCapturedAbilities();
                    break;
            }
        }

        private void ExecuteAllChargedAbilities()
        {
            foreach (var ability in _chargedAbilities)
            {
                ability.Execute();
            }
        }

        private void ExecuteAllNormalAbilities()
        {
            foreach (var ability in _normalAbilities)
            {
                ability.Execute();
            }
        }

        private void ExecuteAllCapturedAbilities()
        {
            foreach (var ability in _capturedAbilities)
            {
                ability.Execute();
            }
        }

        private void StopAllChargedAbilities()
        {
            foreach (var ability in _chargedAbilities)
            {
                ability.Stop();
            }
        }

        private void StopAllNormalAbilities()
        {
            foreach (var ability in _normalAbilities)
            {
                ability.Stop();
            }
        }

        private void StopAllCapturedAbilities()
        {
            foreach (var ability in _capturedAbilities)
            {
                ability.Stop();
            }
        }

        private void DebugLog(SubjectStateComponent.States state)
        {
            Debug.Log($"Состояние изменено на {state}");
        }

        private void InitializeAbilities()
        {
            foreach (var ability in _chargedMonoAbility)
            {
                if (ability is IChargedAbility chargedAbility)
                {
                    _chargedAbilities?.Add(chargedAbility);
                }
            }
            foreach (var ability in _normalMonoAbility)
            {
                if (ability is INormalAbility normalAbility)
                {
                    _normalAbilities?.Add(normalAbility);
                }
            }
            foreach (var ability in _capturedMonoAbility)
            {
                if (ability is ICapturedAbility capturedAbility)
                {
                    _capturedAbilities?.Add(capturedAbility);
                }
            }
        }

        private SubjectStateComponent _stateComponent;

        private List<IChargedAbility>_chargedAbilities = new();
        private List<INormalAbility> _normalAbilities = new();
        private List<ICapturedAbility>_capturedAbilities = new();
    }
}
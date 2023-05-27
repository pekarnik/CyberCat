using Assets.Scripts.Extensions;
using Assets.Scripts.MonoBehaviours.ChargeableItems;

using System;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.Player
{
    public class EnergyComponent : MonoBehaviour
    {
        [SerializeField] private EnergyView _energyView;

        [SerializeField] private int _maxEnergy;
        [SerializeField] private int _currentEnergy;

        [SerializeField] private float _chargeRadius;
        [SerializeField] string[] _chargedLayers;

        public void ChangeEnergyCount(int value)
        {
            _currentEnergy += value;
            if (_currentEnergy < 0)
            {
                _currentEnergy = 0;
            }
            if (_currentEnergy > _maxEnergy)
            {
                _currentEnergy = _maxEnergy;
            }

            SetEnergyInViewIfViewNotNull();
        }

        private void Awake()
        {
            CheckForNull();
            CheckValuesForCorrectness();
            SetEnergyInViewIfViewNotNull();
        }

        private void Update()
        {
            bool isEnergyButtonPressedNow = Input.GetKey(KeyCode.E);
            if (isEnergyButtonPressedNow && !_isEnergyButtonPressedInPreviousMoment)
            {
                _isEnergyButtonPressedInPreviousMoment = true;

                ChargeOrDischargeNearbySubject();
                
            }
            else if(!isEnergyButtonPressedNow)
            {
                _isEnergyButtonPressedInPreviousMoment = false;
            }
        }

        private void ChargeOrDischargeNearbySubject()
        {
            if (!_energyView.gameObject.activeSelf) return;

            Collider[] hitted = new Collider[50];

            var hittedCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                _chargeRadius,
                hitted,
                LayerMask.GetMask(_chargedLayers));

            if (hittedCount <= 0) return;

            if (!hitted[0].TryGetComponent<Subject>(out var chargeableItem)) return;

            if(chargeableItem.CurrentState == SubjectStateComponent.States.Charged)
            {
                chargeableItem.ChangeState(SubjectStateComponent.States.Normal);
                ChangeEnergyCount(1);
            }
            else if(_currentEnergy > 0)
            {
                chargeableItem.ChangeState(SubjectStateComponent.States.Charged);
                ChangeEnergyCount(-1);
            }
        }

        private void SetEnergyInViewIfViewNotNull()
        {
            if (_energyView != null)
            {
                _energyView.MaxEnergy = _maxEnergy;
                _energyView.CurrentEnergy = _currentEnergy;
            }
        }

        private void CheckForNull()
        {
            try
            {
                _energyView.IfNullThrowException();
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e);
            }
        }

        private void CheckValuesForCorrectness()
        {
            if (_maxEnergy <= 0 ||
                _currentEnergy <= 0 ||
                _currentEnergy > _maxEnergy)
            {
                Debug.LogError("Max energy or current energy in EnergyComponent set incorrectly");
            }
        }

        private bool _isEnergyButtonPressedInPreviousMoment;
    }
}
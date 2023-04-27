using System;
using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.Player
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private HealthView _healthView;
        
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;

        private void Awake()
        {
            CheckForNull();
            CheckValuesForCorrectness();
            SetHealthInViewIfViewNotNull();
        }

        public void ChangeHealthCount(int value)
        {
            _currentHealth += value;
            if (_currentHealth < 0)
            {
                _currentHealth = 0;
            }
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }

            SetHealthInViewIfViewNotNull();
        }

        private void SetHealthInViewIfViewNotNull()
        {
            if (_healthView != null)
            {
                _healthView.MaxHealth = _maxHealth;
                _healthView.CurrentHealth = _currentHealth;
            }
        }

        private void CheckForNull()
        {
            try
            {
                _healthView.IfNullThrowException();
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e);
            }
        }

        private void CheckValuesForCorrectness()
        {
            if (_maxHealth <= 0 ||
                _currentHealth <= 0 ||
                _currentHealth > _maxHealth)
            {
                Debug.LogError("Max health or current health in HealthComponent set incorrectly");
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MonoBehaviours.Player
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Image[] _healthPartsImages;

        private int _maxHealth;
        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
            }
        }

        private int _currentHealth;
        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                UpdateHealthBar();
            }
        }

        void ChangeDayTime(DayTimeState state) {
            Debug.Log(state.ToString());
        }


        private void Awake()
        {
            CheckForNull();
        }

        private void Start()
        {
            if (_healthPartsImages.Length != _maxHealth)
            {
                Debug.LogError("Count of health parts images not equal to max health");
            }

            DayTimeEventManager.SubscribeToDayTime(ChangeDayTime);
        }

        private void UpdateHealthBar()
        {
            for (int i = 0; i < _currentHealth; i++)
            {
                _healthPartsImages[i].enabled = true;
            }

            for (int j = _maxHealth - _currentHealth - 1; j >= 0; j--)
            {
                _healthPartsImages[j].enabled = false;
            }
        }

        private void CheckForNull()
        {
            if (_healthPartsImages == null)
            {
                Debug.Log("HealthView: HealthPartsImages not set");
            }
        }
    }
}
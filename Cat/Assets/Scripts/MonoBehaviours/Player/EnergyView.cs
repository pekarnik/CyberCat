using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MonoBehaviours.Player
{
    public class EnergyView : MonoBehaviour
    {
        [SerializeField] private Image[] _energyPartsImages;

        private int _maxEnergy;
        public int MaxEnergy
        {
            get => _maxEnergy;
            set
            {
                _maxEnergy = value;
            }
        }

        private int _currentEnergy;
        public int CurrentEnergy
        {
            get => _currentEnergy;
            set
            {
                _currentEnergy = value;
                UpdateEnergyBar();
            }
        }

        private void Awake()
        {
            CheckForNull();
        }

        private void Start()
        {
            if (_energyPartsImages.Length != _maxEnergy)
            {
                Debug.LogError("Count of energy parts images not equal to max energy");
            }
        }

        private void UpdateEnergyBar()
        {
            for (int i = 0; i < _currentEnergy; i++)
            {
                _energyPartsImages[i].enabled = true;
            }

            for (int j = _maxEnergy - _currentEnergy - 1; j >= 0; j--)
            {
                _energyPartsImages[j].enabled = false;
            }
        }

        private void CheckForNull()
        {
            if (_energyPartsImages == null)
            {
                Debug.Log("EnergyView: EnergyPartsImages not set");
            }
        }
    }
}
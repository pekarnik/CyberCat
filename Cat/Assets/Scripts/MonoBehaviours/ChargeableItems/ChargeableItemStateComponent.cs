using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.ChargeableItems
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ChargeableItemStateComponent : MonoBehaviour
    {
        [SerializeField] private Material _normalMaterial;
        [SerializeField] private Material _chargedMaterial;

        [SerializeField] private States _currentState;
        public States CurrentState => _currentState;

        public delegate void StateChangedHandler(States state);

        public event StateChangedHandler StateChanged;

        public enum States
        {
            /// <summary>
            /// ���������� ���������
            /// </summary>
            Normal,

            /// <summary>
            /// �������
            /// </summary>
            Charged,

            /// <summary>
            /// � �������� �������
            /// </summary>
            InProgressOfCapturing,

            /// <summary>
            /// ������ �������������
            /// </summary>
            CaptureSuspended,

            /// <summary>
            /// ��������
            /// </summary>
            Captured
        }

        private void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        public void Charge()
        {
            if (_currentState == States.Charged) return;

            _currentState = States.Charged;

            if (_chargedMaterial != null)
            {
                _renderer.material = _chargedMaterial;
            }

            OnStateChanged();
        }

        public void Discharge()
        {
            if (_currentState != States.Charged) return;

            _currentState = States.Normal;

            if (_normalMaterial != null)
            {
                _renderer.material = _normalMaterial;
            }

            OnStateChanged();
        }

        private void OnStateChanged()
        {
            StateChanged?.Invoke(_currentState);
        }

        private MeshRenderer _renderer;
    }
}
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.Player
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ChargeableItem : MonoBehaviour
    {
        [SerializeField] private Material _normalMaterial;
        [SerializeField] private Material _chargedMaterial;

        [SerializeField] private States _currentState;
        public States CurrentState => _currentState;

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

            _renderer.material = _chargedMaterial;
        }

        public void Discharge()
        {
            if (_currentState != States.Charged) return;

            _currentState = States.Normal;

            _renderer.material = _normalMaterial;
        }

        private MeshRenderer _renderer;
    }
}
namespace Assets.Scripts.MonoBehaviours.ChargeableItems
{
    public class SubjectStateComponent : SubjectEventsHandler
    {
        public SubjectStateComponent(States state)
        {
            _currentState = state;
            OnStateChanged(_currentState);
        }

        private States _currentState;
        public States CurrentState => _currentState;

        public enum States
        {
            /// <summary>
            /// Нормальное состояние
            /// </summary>
            Normal,

            /// <summary>
            /// Заряжен
            /// </summary>
            Charged,

            /// <summary>
            /// В процессе захвата
            /// </summary>
            InProgressOfCapturing,

            /// <summary>
            /// Захват приостановлен
            /// </summary>
            CaptureSuspended,

            /// <summary>
            /// Захвачен
            /// </summary>
            Captured
        }

        public void ChangeState(States state)
        {
            switch (state)
            {
                case States.Normal:
                    SetNormalState();
                    break;

                case States.Charged:
                    SetChargeState();
                    break;

                case States.Captured:
                    SetCaptureState();
                    break;
            }
        }

        private void SetChargeState()
        {
            if (_currentState != States.Normal) return;

            _currentState = States.Charged;
            OnStateChanged(_currentState);
        }

        private void SetNormalState()
        {
            if (_currentState != States.Charged) return;

            _currentState = States.Normal;
            OnStateChanged(_currentState);
        }

        private void SetCaptureState()
        {
            if (_currentState != States.Normal) return;

            _currentState = States.Captured;
            OnStateChanged(_currentState);
        }
    }
}
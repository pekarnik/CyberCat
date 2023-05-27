namespace Assets.Scripts.MonoBehaviours.ChargeableItems
{
    public class SubjectEventsHandler
    {
        public delegate void StateChangedHandler(SubjectStateComponent.States state);

        public event StateChangedHandler StateChanged;

        protected void OnStateChanged(SubjectStateComponent.States state)
        {
            StateChanged?.Invoke(state);
        }
    }
}
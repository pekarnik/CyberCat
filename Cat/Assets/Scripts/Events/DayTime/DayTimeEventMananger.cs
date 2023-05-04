using UnityEngine.Events;

namespace EventManager {
    public static class DayTimeEventManager {
        

        public static UnityEvent<DayTimeState> DayTimeChanged = new UnityEvent<DayTimeState>();
        public static DayTimeState CurrentDayTimeState = DayTimeState.DAY;
        public static DayTimeState PreviousDayTimeState = DayTimeState.DAY;

        public static void SubscribeToDayTime (UnityAction<DayTimeState> callback) {
            DayTimeChanged.AddListener(callback);
        }

        public static void UnsubscribeFromDayTime (UnityAction<DayTimeState> callback) {
            DayTimeChanged.RemoveListener(callback);
        }

        public static void Cleanup() {
            DayTimeChanged.RemoveAllListeners();
        }

        public static void ChangeDayTime (DayTimeState state) {
            PreviousDayTimeState = CurrentDayTimeState;
            CurrentDayTimeState = state;
            DayTimeChanged.Invoke(state);
        }
    }
}
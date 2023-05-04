using UnityEngine;
using TMPro;
using EventManager;

public class DayNightIndicatorController : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI dayTimeText;

    void Start() {
        DayTimeEventManager.SubscribeToDayTime(ChangeDayTime);
    }

    void Destroy() {
        DayTimeEventManager.UnsubscribeFromDayTime(ChangeDayTime);
    }

    void ChangeDayTime(DayTimeState state) {
        dayTimeText.text = state.ToString();
    }
}
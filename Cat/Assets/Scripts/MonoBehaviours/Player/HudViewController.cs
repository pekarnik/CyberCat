using EventManager;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.Player
{
    public class HudViewController : MonoBehaviour
    {
        [SerializeField] private bool _startOnAwake = true;

        private void OnEnable()
        {
            DayTimeEventManager.SubscribeToDayTime(HideOrShowHud);
        }

        private void OnDisable()
        {
            DayTimeEventManager.UnsubscribeFromDayTime(HideOrShowHud);
        }

        public void Start()
        {
            if (_startOnAwake)
            {
                DayTimeEventManager.ChangeDayTime(DayTimeEventManager.CurrentDayTimeState);
            }
        }

        private void HideOrShowHud(DayTimeState dayState)
        {
            switch (dayState)
            {
                case DayTimeState.DAY:

                    gameObject.SetActive(false);
                    break;

                case DayTimeState.NIGHT:

                    gameObject.SetActive(true);
                    break;
            }
        }
    }
}
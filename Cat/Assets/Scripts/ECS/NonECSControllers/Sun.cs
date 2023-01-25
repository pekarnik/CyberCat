using System;
using UnityEngine;
namespace NonECS
{

    public enum DayState
    {
        NONE = 0,
        DAY = 1,
        NIGHT = 2
    }
    public delegate DayState ChangeDay();
    public class Sun:MonoBehaviour
    {
        event ChangeDay _changeDay;
        [SerializeField]
        private float _dayLength = 5.0f * 60;
        [SerializeField]
        private float _rotateToNight;
        [SerializeField]
        private float _rotateToDay;
        [SerializeField]
        private float _nightLength = 5.0f * 60;
        private float _curTime;
        DayState _dayState;
        private float _startRot;
        private void Start()
        {
            RunDay(DayState.DAY);
            _curTime = _dayLength;
        }
        private void Update()
        {
            if (_curTime <= 0.0f)
            {
                _changeDay?.Invoke();
                _dayState = DayState.NONE;
            }
            if(_dayState == DayState.DAY || _dayState == DayState.NIGHT)
            {
                _curTime -= Time.deltaTime;
                RotateSun();
            }
        }
        private void RunDay(DayState day)
        {
            _dayState = day;
            _startRot = transform.rotation.eulerAngles.x;
        }
        private void RotateSun()
        {
            float lerp = 0;
            float timeRot = 0;
            if (_dayState == DayState.DAY)
            {
                timeRot = (_dayLength - _curTime) / _dayLength;
                lerp = Mathf.Lerp(_startRot, _rotateToNight, timeRot);
            }
            else if(_dayState == DayState.NIGHT)
            {
                timeRot = (_nightLength - _curTime) / _nightLength;
                lerp = Mathf.Lerp(_startRot, _rotateToNight, timeRot);
            }
            transform.rotation = Quaternion.Euler(lerp, -30, 0);
        }
    }
}
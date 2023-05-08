using UnityEngine;
using System;

namespace Components {
    [Serializable]
    public struct DayTime
    {
        public float currentTime;
        public float nightDuration;
        public float dayDuration;
    }
}
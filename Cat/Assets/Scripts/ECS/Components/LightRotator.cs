using UnityEngine;
using System;

namespace Components {
    [Serializable]
    public struct LightRotator
    {
        public Transform lightTransform;

        public float timeToEnd;
        
        public float startAngle;
        public float endAngle;
    }
}
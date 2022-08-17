using UnityEngine;
using System;

namespace Components {
    [Serializable]
    public struct Attacker
    {
        public Transform attackSource;

        public bool isAttacked;
    }
}
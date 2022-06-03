using UnityEngine;
using System;

namespace Components {
    [Serializable]
    public struct Movable
    {
        public Rigidbody rigidbody;
        public float moveSpeed;
    }
}
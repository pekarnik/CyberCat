using UnityEngine;
using System;

namespace Components {
    [Serializable]
    public struct Movable
    {

        public static readonly float DEFAULT_MOVE_SPEED = 5.0f;
        public static readonly float JUMPING_MOVE_SPEED = 2.5f;
        public float moveSpeed;
    }
}
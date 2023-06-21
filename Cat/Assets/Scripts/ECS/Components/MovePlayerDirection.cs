using UnityEngine;
using System;

namespace Components {
    [Serializable]
    public struct MovePlayerDirection
    {

        public static readonly float DEFAULT_MOVE_SPEED = 5.0f;
        public static readonly float JUMPING_MOVE_SPEED = 2.5f;
        public float moveSpeed;

        public Vector2 current;
        public Vector2 old;

        public float currentAngle;
        public float oldAngle;

        public Transform transform;
    }
}
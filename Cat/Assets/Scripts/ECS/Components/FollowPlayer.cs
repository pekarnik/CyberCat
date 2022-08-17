using UnityEngine;
using System;

namespace Components
{
    public struct FollowPlayer
    {
        public Transform followerTransform;
        public Transform leaderTransform;

        public Vector3 offset;
        public Vector3 velocity;
        public float smoothness;
    }
}
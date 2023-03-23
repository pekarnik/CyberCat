using UnityEngine;
using System;

namespace Components {

    public struct CameraState
    {
        public enum CAMERA_STATE {
            STATIC,
            FOLLOW_PLAYER
        }

        public CAMERA_STATE state;
    }
}
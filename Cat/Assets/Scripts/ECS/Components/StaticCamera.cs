using UnityEngine;

namespace Components 
{
    public struct StaticCamera {
        public Vector3 cameraStaticPosition;
        public Quaternion cameraStaticRotation;
        public bool alreadyFixed;

        public Transform cameraTransform;
    }
}
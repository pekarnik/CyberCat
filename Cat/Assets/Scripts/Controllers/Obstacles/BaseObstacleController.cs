using System.Collections;
using UnityEngine;

class BaseObstacleController : MonoBehaviour, ResetableGameObject {
    public GameObject CurrentGameObject{
        get => gameObject;
    }
}
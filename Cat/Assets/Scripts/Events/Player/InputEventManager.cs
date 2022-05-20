using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputEventManager
{
    
    public InputEventManager () {}

    public delegate void MethodContainer(Vector3 movement);

    public event MethodContainer PlayerMovement;

    public void MovePlayer (Vector3 moveDirection) {
        PlayerMovement.Invoke(moveDirection);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEventController
{
    private PlayerModel playerModel;

    public InputEventManager inputEventManager = new InputEventManager();

    public InputEventController (PlayerModel model) {
        playerModel = model;
    } 

    public bool IsGrounded () {
        RaycastHit hit;

        Physics.Raycast(playerModel.transform.position, playerModel.transform.TransformDirection(Vector3.down), out hit, playerModel.Height + 0.01f, PlayerModel.LAYER_MASK);

        return hit.collider != null;
    }

    public void OnUpdate() {
        ProcessMovement();
    }

    void ProcessMovement() {
    
        float moveX = Input.GetAxisRaw("Vertical");
        float moveZ = Input.GetAxisRaw("Horizontal");

        bool isGrounded = IsGrounded();

        Vector3 forwardVector = playerModel.transform.forward * moveX;
        Vector3 sideVector = playerModel.transform.right * moveZ;
        Vector3 moveDirection;
        if (isGrounded) {
            moveDirection = forwardVector + sideVector;
        }
        else
        {
            moveDirection = (forwardVector + sideVector) * playerModel.SpeedDowned;
        }

        if (Vector3.zero != moveDirection) {
            inputEventManager.MovePlayer(moveDirection);
        }
    }
}

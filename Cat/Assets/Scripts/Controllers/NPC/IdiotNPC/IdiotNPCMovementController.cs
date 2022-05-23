using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IdiotNPCMovementController : MonoBehaviour {
    Rigidbody rb;
    
    float cameraY;
    private Vector3 moveDirection;
    private Vector3 lastDirection;


    float mouseX;
    [SerializeField]
    Transform face;
    [SerializeField]
    private KeyCode forwardKey = KeyCode.G;
    [SerializeField]
    private KeyCode leftKey = KeyCode.V;
    [SerializeField]
    private KeyCode backwardKey = KeyCode.B;
    [SerializeField]
    private KeyCode rightKey = KeyCode.N;
    [SerializeField]
    private float moveSpeed = 0.5f;
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update () {
        ProcessInputs();
    }

    void FixedUpdate() {
        Move();
    }

    void ProcessInputs () {

        float moveX = 0.0f;
        float moveZ = 0.0f;
        
        if (Input.GetKey(forwardKey)) {
            moveX = 1.0f;
        }
        if (Input.GetKey(backwardKey)) {
            moveX = -1.0f;
        }
        if (Input.GetKey(rightKey)) {
            moveZ = 1.0f;
        }
        if (Input.GetKey(leftKey)) {
            moveZ = -1.0f;
        }
        
        Vector3 forwardVector = transform.forward * moveX;
        Vector3 sideVector = transform.right * moveZ;


        moveDirection = forwardVector + sideVector;
        Vector3 rotateDirection = forwardVector - sideVector;
        
        if (rotateDirection != Vector3.zero) {
            lastDirection = rotateDirection;
        }

        if (lastDirection != null) {
            Rotate(lastDirection);
        }
    }

    void Move () {
        Vector3 movePosition = rb.position + (moveDirection * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(movePosition);

    }

    void Rotate (Vector3 moveDirection) {
        float angle = Vector3.SignedAngle(moveDirection, transform.forward, Vector3.up);
    
        face.rotation = Quaternion.Euler(0, angle, 0);
    }
}